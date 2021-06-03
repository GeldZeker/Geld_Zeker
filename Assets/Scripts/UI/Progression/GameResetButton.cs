using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.UI.Progression
{
    /// <summary>A behaviour used for resetting the game elements</summary>
    public class GameResetButton : AudableButton
    {
        [Header("Settings")] 
        [SerializeField]
        private string notifyMessage = "De game is hersteld";

        [SerializeField]
        private string verifyMessage = "Weet u zeker dat u de game wilt herstellen?";

        [Header("Assets to restore")]
        [SerializeField]
        private MiniGameSettingAsset miniGameSetting = null;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(OnClick);
        }

        protected override void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        /// <summary>Called when the reset button has been clicked to start the reset progress verification process</summary>
        private void OnClick()
        {
            NotificationUtility.Instance.Verify(verifyMessage, ResetAllElements);
        }

        /// <summary>Resets all the game elments</summary>
        private void ResetAllElements()
        {
            /// <summary>Resets the Quests</summary>
            QuestManager.Instance.ResetProgress();
            /// <summary>Resets the Introduction</summary>
            IntroductionManager.Instance.RestoreIntroductions();
            /// <summary>Resets the PlayerProperties</summary>
            PlayerPropertyManager.Instance.ResetProgression();
            /// <summary>Resets the Time</summary>
            TimeController.instance.ResetDateTime();

            NotificationUtility.Instance.Notify(notifyMessage, NotificationStayTime.Short);

            miniGameSetting.Restore();

            if (SceneManager.GetActiveScene().name != NavigationSystem.NameOfHomeScreen)
            {
                //if this action was done in a scene other than the home screen scene return to the home screen scene
                MainCanvasManager.Instance.ToggleSettings();
                SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, NavigationSystem.NameOfHomeScreen, LoadSceneMode.Additive);
            }
        }
    }
}