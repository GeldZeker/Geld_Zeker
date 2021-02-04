using BWolf.Behaviours.SingletonBehaviours;
using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.UI;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.Player.Introductions
{
    /// <summary>Singleton class for managing introductions</summary>
    public class IntroductionManager : SingletonBehaviour<IntroductionManager>
    {
        [Header("References")]
        [SerializeField]
        private IntroductionAsset asset = null;

        public event Action<Introduction> IntroFinished;

        public bool IsActive { get; private set; }
        public Introduction ActiveIntro { get; private set; }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;

            foreach (Introduction introduction in asset)
            {
                introduction.OnFinish += OnIntroFinished;
                introduction.OnStart += OnIntroStarted;
                introduction.LoadFromFile();
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            SceneManager.sceneLoaded -= OnSceneLoaded;

            foreach (Introduction introduction in asset)
            {
                introduction.OnFinish -= OnIntroFinished;
                introduction.OnStart -= OnIntroStarted;
            }
        }

        private void OnIntroStarted(Introduction intro)
        {
            IsActive = true;
            ActiveIntro = intro;
        }

        /// <summary>Called when a introduction has been finished to fire event and set active state</summary>
        private void OnIntroFinished(Introduction introduction)
        {
            IsActive = false;
            ActiveIntro = null;
            IntroFinished?.Invoke(introduction);
        }

        /// <summary>Restores the introductions to their default state</summary>
        [ContextMenu("RestoreIntroductions")]
        public void RestoreIntroductions()
        {
            asset.Restore();
        }

        /// <summary>Disables introductions by setting them all to a finished state</summary>
        [ContextMenu("DisableIntroduction")]
        public void DisableIntroduction()
        {
            asset.SetAllFinished();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            //start scene intro if there is one available
            string nameOfScene = scene.name;
            SceneIntroduction intro = asset.GetSceneIntroduction(nameOfScene);
            MinigameSystem system = MinigameSystem.Instance;
            if (intro != null && !(system.IsGameScene(nameOfScene) && system.GetSetting(nameOfScene).MinigameMode))
            {
                intro.Start();
            }
        }
    }
}