using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.UI.Progression
{
    /// <summary>Behaviour for handling a button that resets the quest progress</summary>
    public class QuestResetButton : AudableButton
    {
        [Header("Reset Settings")]
        [SerializeField]
        private string notifyMessage = "Taken zijn hersteld";

        [SerializeField]
        private string verifyMessage = "Weet u zeker dat u uw taken wilt herstellen?";

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
            NotificationUtility.Instance.Verify(verifyMessage, ResetQuestProgress);
        }

        private void ResetQuestProgress()
        {
            NotificationUtility.Instance.Notify(notifyMessage, NotificationStayTime.Short);

            //reset all quest progression
            QuestManager.Instance.ResetProgress();

            if (SceneManager.GetActiveScene().name != NavigationSystem.NameOfHomeScreen)
            {
                //if this action was done in a scene other than the home screen scene return to the home screen scene
                MainCanvasManager.Instance.ToggleSettings();
                SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, NavigationSystem.NameOfHomeScreen, LoadSceneMode.Additive);
            }
        }
    }
}