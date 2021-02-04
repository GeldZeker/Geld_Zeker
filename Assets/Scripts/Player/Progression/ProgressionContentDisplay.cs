using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.UI.Progression;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Player.Progression
{
    /// <summary>Behaviour class for showing the progression content on screen</summary>
    public class ProgressionContentDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image progressBar = null;

        [SerializeField]
        private Text txtProgressPercentage = null;

        [SerializeField]
        private Transform taskLayout = null;

        private CanvasGroup group = null;
        private ProgressionDisplay[] displays;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
            displays = taskLayout.GetComponentsInChildren<ProgressionDisplay>();
        }

        /// <summary>Populates the content of this display with given list of quests</summary>
        public void PopulateDisplay(List<Quest> quests)
        {
            foreach (ProgressionDisplay display in displays)
            {
                display.SetActive(false);
            }

            if (quests.Count == 0)
            {
                return;
            }

            float totalProgress = 0.0f;
            for (int i = 0; i < quests.Count; i++)
            {
                Quest quest = quests[i];
                ProgressionDisplay display = displays[i];

                display.description = quest.Description;
                display.completed = quest.IsCompleted;
                display.SetActive(true);
                totalProgress += quest.Progress;
            }

            float percentageCompleted = Mathf.Clamp01(totalProgress / quests.Count);
            progressBar.fillAmount = percentageCompleted;
            txtProgressPercentage.text = $"{(percentageCompleted * 100.0f).ToString("0.00")}%";
        }

        /// <summary>Sets the active state of this display using its canvasgroup componenent</summary>
        public void SetActive(bool value)
        {
            group.alpha = value ? 1.0f : 0.0f;
            group.interactable = value;
            group.blocksRaycasts = value;
        }
    }
}