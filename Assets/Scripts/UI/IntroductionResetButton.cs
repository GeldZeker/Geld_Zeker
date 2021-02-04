using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.UI
{
    public class IntroductionResetButton : AudableButton
    {
        [Header("Reset Settings")]
        [SerializeField]
        private string notifyMessage = "Introductie is hersteld";

        [SerializeField]
        private string verifyMessage = "Weet u zeker dat u de introductie wilt herstellen?";

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(VerifyReset);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(VerifyReset);
        }

        private void VerifyReset()
        {
            NotificationUtility.Instance.Verify(verifyMessage, ResetIntroduction);
        }

        private void ResetIntroduction()
        {
            NotificationUtility.Instance.Notify(notifyMessage, NotificationStayTime.Short);
            IntroductionManager.Instance.RestoreIntroductions();

            if (SceneManager.GetActiveScene().name != NavigationSystem.NameOfHomeScreen)
            {
                //if this action was done in a scene other than the home screen scene return to the home screen scene
                MainCanvasManager.Instance.ToggleSettings();
                SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, NavigationSystem.NameOfHomeScreen, LoadSceneMode.Additive);
            }
        }
    }
}