using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Player.Progression
{
    /// <summary>A behaviour class for managing a button showing content of a level's progression</summary>
    [RequireComponent(typeof(Button))]
    public class ProgressionLevelButton : AudableButton
    {
        [Header("Settings")]
        [SerializeField]
        private ProgressionLevel level = ProgressionLevel.ONE;

        [SerializeField]
        private bool activeOnAwake = false;

        [Header("References")]
        [SerializeField]
        private ProgressionContentDisplay display = null;

        [SerializeField]
        private GameObject activityIndicator = null;

        private static ProgressionLevelButton activeProgressionLevel;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(OpenProgressionView);
        }

        private void OnEnable()
        {
            if (activeOnAwake)
            {
                activeProgressionLevel = this;
                SetActiveState(true);
            }
        }

        private void OnDisable()
        {
            if (activeOnAwake && activeProgressionLevel == this)
            {
                activeProgressionLevel = null;
            }

            SetActiveState(false);
        }

        protected override void OnDestroy()
        {
            base.Awake();

            button.onClick.RemoveListener(OpenProgressionView);

            if (activeProgressionLevel == this)
            {
                activeProgressionLevel = null;
            }
        }

        /// <summary>Opens the its view by activating itself and deactiving the current active view</summary>
        private void OpenProgressionView()
        {
            activeProgressionLevel.SetActiveState(false);
            SetActiveState(true);

            activeProgressionLevel = this;
        }

        /// <summary>Sets active state content inside the display</summary>
        public void SetActiveState(bool value)
        {
            display.SetActive(value);
            activityIndicator.SetActive(value);
            if (value)
            {
                display.PopulateDisplay(QuestManager.Instance.GetQuestsOfLevel(level));
            }
        }
    }
}