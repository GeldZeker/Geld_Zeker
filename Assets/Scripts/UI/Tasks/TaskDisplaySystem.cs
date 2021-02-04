using BWolf.Utilities;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.Tasks
{
    /// <summary>The behaviour class for managing the tasks displayed when interaction with the task button happened</summary>
    public class TaskDisplaySystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float toggleTime = 0.125f;

        [Header("References")]
        [SerializeField]
        private Button btnTaskDisplay = null;

        [SerializeField]
        private RectTransform screenTransform = null;

        [SerializeField]
        private TaskDisplay display = null;

        private bool isToggling;
        private bool isScreenVisible;

        private void Awake()
        {
            btnTaskDisplay.onClick.AddListener(ToggleNotificationScreen);
        }

        /// <summary>Toggles the notification screen its current state</summary>
        private void ToggleNotificationScreen()
        {
            if (!isToggling)
            {
                if (!isScreenVisible)
                {
                    PopulateDisplay();
                }

                StartCoroutine(NotificationScreenToggleEnumerator());
            }
        }

        private void Update()
        {
            if (isScreenVisible && (RectTransformExtensions.PressOutsideTransform(screenTransform) || RectTransformExtensions.PressOutsideTransform(screenTransform)))
            {
                //if the navigation has focus and there is a touch outside the navigation, close it
                ToggleNotificationScreen();
            }
        }

        /// <summary>Shows the active quests on screen</summary>
        private void PopulateDisplay()
        {
            //show only as much quests as there are displays
            List<Quest> activeQuests = QuestManager.Instance.ActiveQuests;
            PopulateDisplay(activeQuests.Count == 1 ? activeQuests[0] : null);
        }

        /// <summary>Tries populating the display at given index with quest</summary>
        private void PopulateDisplay(Quest quest)
        {
            if (quest != null)
            {
                display.Populate(quest);
                display.SetActive(true);
            }
            else
            {
                display.SetActive(false);
            }
        }

        /// <summary>Returns an Enumerator that toggles the notification screen its state over time</summary>
        private IEnumerator NotificationScreenToggleEnumerator()
        {
            isToggling = true;

            float startScale = isScreenVisible ? 1.0f : 0.0f;
            float endScale = isScreenVisible ? 0.0f : 1.0f;

            isScreenVisible = !isScreenVisible;

            LerpValue<Vector3> scale = new LerpValue<Vector3>(new Vector3(startScale, startScale), new Vector3(endScale, endScale), toggleTime);
            while (scale.Continue())
            {
                screenTransform.localScale = Vector3.Lerp(scale.start, scale.end, scale.perc);
                yield return null;
            }

            isToggling = false;
        }
    }
}