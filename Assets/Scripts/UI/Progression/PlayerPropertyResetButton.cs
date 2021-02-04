using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.UI.Progression
{
    /// <summary>A behaviour used for resetting player progress</summary>
    public class PlayerPropertyResetButton : AudableButton
    {
        [Header("Settings")]
        [SerializeField]
        private string notifyMessage = "Personage is hersteld";

        [SerializeField]
        private string verifyMessage = "Weet u zeker dat u uw personage wilt herstellen?";

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
            NotificationUtility.Instance.Verify(verifyMessage, ResetProgress);
        }

        private void ResetProgress()
        {
            //reset all player properties
            PlayerPropertyManager.Instance.ResetProgression();
            NotificationUtility.Instance.Notify(notifyMessage, NotificationStayTime.Short);

            if (SceneManager.GetActiveScene().name != NavigationSystem.NameOfHomeScreen)
            {
                //if this action was done in a scene other than the home screen scene return to the home screen scene
                MainCanvasManager.Instance.ToggleSettings();
                SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, NavigationSystem.NameOfHomeScreen, LoadSceneMode.Additive);
            }
        }
    }
}