using BWolf.Behaviours.SingletonBehaviours;
using BWolf.Utilities.CharacterDialogue;
using GameStudio.GeldZeker.Player.GameNotifications;
using GameStudio.GeldZeker.Monologues;
using GameStudio.GeldZeker.UI.CellPhone;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Player.Tamagotchi;
using System.Linq;
using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.MiniGames.Settings;

namespace GameStudio.GeldZeker.UI
{
    /// <summary>The singleton class for managing the PersistantUICanvas</summary>
    public class MainCanvasManager : SingletonBehaviour<MainCanvasManager>
    {
        [Header("Settings")]
        [SerializeField]
        private string[] settingScenes = { "HomeScreen", "SelectCharacterScene" };

        [Header("References")]
        [SerializeField]
        private GameObject settingsObject = null;

        [SerializeField]
        private CanvasGroup normalModeButtonGroup = null;

        [SerializeField]
        private CanvasGroup minigameModeButtonGroup = null;

        [Header("Systems")]
        [SerializeField]
        private DialogueSystem dialogueSystem = null;

        [SerializeField]
        private MonologueSystem monologueSystem = null;

        [SerializeField]
        private CellPhoneDisplaySystem cellPhoneDisplaySystem = null;

        [SerializeField]
        private GameNotificationSystem gameNotificationSystem = null;

        [SerializeField]
        private NavigationSystem navigationSystem = null;

        [SerializeField]
        private ShowTamagotchiElements tamagotchiElements = null;

        /// <summary>Returns whether the current active scene is one in which the persistantUICanvas can be active in</summary>
        public bool IsInAnInActiveScene
        {
            get
            {
                return IsAnActiveScene(SceneManager.GetActiveScene().name);
            }
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            // END Session
            TimeController.instance.DisplaySessionTime();
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public void SetTamagotchiHappinessIcon(HappinesIcon icon)
        {
            tamagotchiElements.SetHappinessIcon(icon);
        }

        /// <summary>called when a new scene has been loaded to set the button state based on if it is a scene in which the persistantUICanvas can be active in</summary>
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            string name = scene.name;
            if (IsAnActiveScene(name))
            {
                if (name == NavigationSystem.NameOfGameHall)
                {
                    SetNormalModeButtonState(false);
                    SetMinigameModeButtonState(true);
                }
                else
                {
                    SetNormalModeButtonState(true);
                    SetMinigameModeButtonState(false);
                }
            }
            else
            {
                SetNormalModeButtonState(false);
                SetMinigameModeButtonState(false);
            }
        }

        /// <summary>Outputs a parentscene object with given scene name. Returns whether it was succesfull</summary>
        public ParentScene GetParentScene(string nameOfParent)
        {
            return navigationSystem.GetParentScene(nameOfParent);
        }

        /// <summary>Toggles the active state of the settings menu</summary>
        public void ToggleSettings()
        {
            settingsObject.SetActive(!settingsObject.activeInHierarchy);
        }

        /// <summary>Starts a dialogue using the given dialogue and an optional on finish callback</summary>
        public void StartDialogue(Dialogue dialogue, Action onFinish = null)
        {
            dialogueSystem.StartDialogue(dialogue, onFinish);
        }

        public void StartMonologue(Monologue monologue, Action onFinish = null)
        {
            monologueSystem.StartMonologue(monologue, onFinish);
        }

        public void OpenCellPhoneScreen(CellPhoneScreen phoneScreen)
        {
            cellPhoneDisplaySystem.OpenScreen(phoneScreen);
        }

        public void AddNotification(GameNotificationType notificationType)
        {
            gameNotificationSystem.AddNotification(notificationType);
        }

        /// <summary>Sets the active state of normal mode buttons using the CanvasGroup to set alpha, interactable and blocksraycasts values</summary>
        private void SetNormalModeButtonState(bool active)
        {
            normalModeButtonGroup.alpha = active ? 1.0f : 0.0f;
            normalModeButtonGroup.interactable = active;
            normalModeButtonGroup.blocksRaycasts = active;
        }

        /// <summary>Sets the active state of minigame mode buttons using the CanvasGroup to set alpha, interactable and blocksraycasts values</summary>
        public void SetMinigameModeButtonState(bool active)
        {
            minigameModeButtonGroup.alpha = active ? 1.0f : 0.0f;
            minigameModeButtonGroup.interactable = active;
            minigameModeButtonGroup.blocksRaycasts = active;
        }

        /// <summary>Returns whether the scene with given scene name is one in which the persistantUICanvas can be active in</summary>
        public bool IsAnActiveScene(string sceneName)
        {
            string[] inActiveScenes = MinigameSystem.Instance.SceneNames.Concat(settingScenes).ToArray();

            for (int i = 0; i < inActiveScenes.Length; i++)
            {
                if (sceneName == inActiveScenes[i])
                {
                    return false;
                }
            }

            return true;
        }
    }
}