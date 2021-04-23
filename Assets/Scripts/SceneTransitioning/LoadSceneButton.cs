using Assets.Scripts.SceneTransitioning;
using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.UI.CellPhone;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.SceneTransitioning
{
    /// <summary>A simple behaviour attachable to a button to make it load a scene to be defined in the inspector</summary>
    public class LoadSceneButton : AudableButton
    {
        [Header("Load Settings")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        [SerializeField]
        private string nameOfTransition = "Fade";

        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

        [SerializeField]
        private bool disableOnClick = true;

        public event Action Clicked;

        [Header("CellPhone Display")]
        [SerializeField]
        private CellPhoneDisplaySystem cellPhoneDisplaySystem = null;

        [Header("Options Screen")]
        [SerializeField]
        private GameObject optionsScreen = null;

        public string NameOfSceneLoading
        {
            get { return nameOfScene; }
            set { nameOfScene = value; }
        }

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(LoadScene);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(LoadScene);
        }

        /// <summary>Called when the button has been clicked to transition to a new scene</summary>
        private void LoadScene()
        {
            string nameOfActiveScene = SceneManager.GetActiveScene().name;
            if (!string.IsNullOrEmpty(nameOfScene) && nameOfScene != nameOfActiveScene && !SceneTransitionSystem.Instance.IsTransitioning)
            {
                Clicked?.Invoke();

                if (disableOnClick)
                {
                    SetInteractable(false);
                }

                if (MinigameSystem.Instance.IsGameScene(nameOfActiveScene) && MinigameSystem.Instance.GetSetting(nameOfActiveScene).MinigameMode)
                {
                    //load the gamehall if the current scene is a minigame scene and it is in minigame mode
                    SceneTransitionSystem.Instance.Transition(nameOfTransition, NavigationSystem.NameOfGameHall, loadSceneMode);
                }
                else
                {
                    string finalizedScene = null;
                    switch (nameOfScene)
                    {
                        case "Bank":
                            {
                                if (TimeController.instance.latestDayNightCyclePart == "n") finalizedScene = "BankClosed";
                            }
                            break;
                        case "FruitDepartment":
                            {
                                if (TimeController.instance.latestDayNightCyclePart == "n") finalizedScene = "ShopClosed";
                            }
                            break;
                        case "InvoiceDraggingGame":
                            {
                                cellPhoneDisplaySystem.ToggleCellPhone();
                            }
                            break;
                        case "GameHall":
                            {
                                BackToScene.instance.backScene = nameOfActiveScene;
                            }
                            break;
                        case "HomeScreen":
                            {
                                string scene = "HomeScreen";
                                if (nameOfActiveScene == "GameHall") scene = BackToScene.instance.backScene;
                                finalizedScene = scene;
                                optionsScreen.SetActive(false);
                            }
                            break;
                        default: break;
                    }
                    if (finalizedScene != null) SceneTransitionSystem.Instance.Transition(nameOfTransition, finalizedScene, loadSceneMode);
                    else SceneTransitionSystem.Instance.Transition(nameOfTransition, nameOfScene, loadSceneMode);
                }
            }
        }

        public void SetNameOfScene(string nameOfScene)
        {
            this.nameOfScene = nameOfScene;
        }

        /// <summary>Sets the interactable state of the button on this behaviour its game object</summary>
        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }
    }
}