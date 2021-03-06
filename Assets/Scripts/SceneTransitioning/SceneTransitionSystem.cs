﻿using BWolf.Behaviours.SingletonBehaviours;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.SceneTransitioning
{
    /// <summary>System responsible for providing transitions between scenes</summary>
    public class SceneTransitionSystem : SingletonBehaviour<SceneTransitionSystem>
    {
        [SerializeField]
        private string nameOfTransitionUIScene = string.Empty;

        public event Action<Scene, LoadSceneMode> TransitionCompleted;

        public event Action<string, LoadSceneMode> TransitionStarted;

        public bool IsTransitioning { get; private set; }

        private string[] scenesInBuild;

        private Dictionary<string, ITransitionProvider> providers = new Dictionary<string, ITransitionProvider>();

        public const string DefaultTransition = "Fade";

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            int sceneCount = SceneManager.sceneCountInBuildSettings;
            scenesInBuild = new string[sceneCount];

            for (int i = 0; i < sceneCount; i++)
            {
                string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
                int lastSlash = scenePath.LastIndexOf("/");
                scenesInBuild[i] = (scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".") - lastSlash - 1));
            }

            StartCoroutine(LoadTransitionUIScene());
        }

        /// <summary>Starts transition with given transitionName set by transition provider to a scene with given scene name and in given load mode</summary>
        public void Transition(string transitionName, string sceneName, LoadSceneMode loadMode, Action onFinish = null)
        {
            if (IsTransitioning || !SceneIsLoadable(sceneName) || !providers.ContainsKey(transitionName))
            {
                return;
            }

            TransitionStarted?.Invoke(sceneName, loadMode);

            ITransitionProvider provider = providers[transitionName];
            provider.OnProgressUpdated(0.0f);

            switch (loadMode)
            {
                case LoadSceneMode.Single:
                    StartCoroutine(LoadRoutine(sceneName, loadMode, provider, onFinish));
                    break;

                case LoadSceneMode.Additive:
                    StartCoroutine(TransitionRoutine(sceneName, loadMode, provider, onFinish));
                    break;
            }
        }

        /// <summary>Starts transition with given transition provider to a scene with given scene name and in given load mode</summary>
        public void Transition(ITransitionProvider provider, string sceneName, LoadSceneMode loadMode)
        {
            if (IsTransitioning || !SceneIsLoadable(sceneName) || !providers.ContainsValue(provider))
            {
                return;
            }

            TransitionStarted?.Invoke(sceneName, loadMode);

            provider.OnProgressUpdated(0.0f);

            switch (loadMode)
            {
                case LoadSceneMode.Single:
                    StartCoroutine(LoadRoutine(sceneName, loadMode, provider));
                    break;

                case LoadSceneMode.Additive:
                    StartCoroutine(TransitionRoutine(sceneName, loadMode, provider));
                    break;
            }
        }

        /// <summary>Returns whether given scene name corresponds to a scene that is stored in the build settings</summary>
        private bool SceneIsLoadable(string sceneName)
        {
            for (int i = 0; i < scenesInBuild.Length; i++)
            {
                if (sceneName == scenesInBuild[i])
                {
                    return true;
                }
            }

            Debug.LogWarning("Scene with sceneName: " + sceneName + " is not loadable because it doesn't exist in the build settings");
            return false;
        }

        /// <summary>Returns a routine for transitiong from the current active scene to a scene with given name in given load mode and
        /// with given provider</summary>
        private IEnumerator TransitionRoutine(string sceneName, LoadSceneMode mode, ITransitionProvider provider, Action onFinish = null)
        {
            IsTransitioning = true;

            yield return UnLoadRoutine(provider);
            yield return LoadRoutine(sceneName, mode, provider);

            IsTransitioning = false;
            onFinish?.Invoke();
        }

        /// <summary>Returns a routine that unloads the current active scene based on given provider</summary>
        private IEnumerator UnLoadRoutine(ITransitionProvider provider)
        {
            yield return provider.Outro();

            AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
            while (!unloadOperation.isDone)
            {
                provider.OnProgressUpdated(unloadOperation.progress / 2.0f);
                yield return null;
            }
        }

        /// <summary>Returns a routine that loads a scene with given name in given load mode and
        /// with given provider</summary>
        private IEnumerator LoadRoutine(string sceneName, LoadSceneMode mode, ITransitionProvider provider, Action onFinish = null)
        {
            AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName, mode);
            while (!loadOperation.isDone)
            {
                provider.OnProgressUpdated(mode == LoadSceneMode.Single ? loadOperation.progress : ((1.0f + loadOperation.progress) / 2.0f));
                yield return null;
            }

            Scene sceneLoaded = SceneManager.GetSceneByName(sceneName);
            SceneManager.SetActiveScene(sceneLoaded);

            yield return provider.Intro();

            TransitionCompleted?.Invoke(sceneLoaded, mode);
            onFinish?.Invoke();
        }

        /// <summary>Returns a routine that loads the transition ui scene and stores its Transition providers</summary>
        private IEnumerator LoadTransitionUIScene()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(nameOfTransitionUIScene, LoadSceneMode.Additive);

            while (!operation.isDone)
            {
                yield return null;
            }

            GameObject[] gameObjects = SceneManager.GetSceneByName(nameOfTransitionUIScene).GetRootGameObjects();
            foreach (GameObject go in gameObjects)
            {
                foreach (ITransitionProvider provider in go.GetComponentsInChildren<ITransitionProvider>())
                {
                    providers.Add(provider.TransitionName, provider);
                }
            }
        }
    }
}