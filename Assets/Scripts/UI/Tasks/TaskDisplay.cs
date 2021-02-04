using BWolf.Utilities.PlayerProgression.Quests;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.Tasks
{
    /// <summary>A behaviour class for displaying a task with its description, progressbar and subtasks</summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class TaskDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text txtTaskDescription = null;

        [SerializeField]
        private Image progressBar = null;

        [SerializeField]
        private SubTaskDisplay[] subTaskDisplays = null;

        private CanvasGroup group;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();
        }

        /// <summary>Populates the display elements with qiven quest</summary>
        public void Populate(Quest quest)
        {
            txtTaskDescription.text = quest.Description;
            progressBar.fillAmount = quest.Progress;

            for (int i = 0; i < subTaskDisplays.Length; i++)
            {
                SubTaskDisplay display = subTaskDisplays[i];
                display.SetActive(false);
                display.SetBolded(false);
            }

            PopulateSubTasks(quest.Tasks);
        }

        private void PopulateSubTasks(QuestTask[] tasks)
        {
            bool bolded = false;

            for (int i = 0; i < tasks.Length; i++)
            {
                QuestTask task = tasks[i];
                SubTaskDisplay display = subTaskDisplays[i];

                display.description = task.TaskDescription;
                display.progress = task.ProgressString;
                display.SetActive(true);

                if (!bolded && task.Progress != 1.0f)
                {
                    bolded = true;
                    display.SetBolded(true);
                }
            }
        }

        /// <summary>Sets the active state of this display using its canvasgroup componenent</summary>
        public void SetActive(bool value)
        {
            group.alpha = value ? 1.0f : 0.0f;
            group.interactable = value;
            group.blocksRaycasts = value;
        }

        [System.Serializable]
        private struct SubTaskDisplay
        {
#pragma warning disable 0649

            [SerializeField]
            private Text Description;

            [SerializeField]
            private Text Progress;

#pragma warning restore 0649

            public string description
            {
                set { Description.text = $"- {value}"; }
            }

            public string progress
            {
                set { Progress.text = value; }
            }

            public void SetBolded(bool value)
            {
                Description.fontStyle = value ? FontStyle.Bold : FontStyle.Normal;
            }

            public void SetActive(bool value)
            {
                Description.gameObject.SetActive(value);
            }
        }
    }
}