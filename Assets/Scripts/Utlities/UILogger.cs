using BWolf.Behaviours.SingletonBehaviours;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>Utility class for logging debug messages to the screen using a canvas</summary>
    public class UILogger : SingletonBehaviour<UILogger>
    {
        [SerializeField]
        private GameObject prefabUILogText = null;

        [SerializeField]
        private GameObject layout = null;

        private Text[] logs = new Text[MAX_LOG_COUNT];

        private const int MAX_LOG_COUNT = 8;

        private int logIndex = 0;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            for (int i = 0; i < logs.Length; i++)
            {
                logs[i] = Instantiate(prefabUILogText, layout.transform).GetComponent<Text>();
            }
        }

        /// <summary>Logs given message to the screen</summary>
        public void Log(object log)
        {
            if (!layout.activeInHierarchy)
            {
                layout.SetActive(true);
            }

            string text = log.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                GetTextComponent().text = text;
                logIndex++;
            }
        }

        private void ClearLogs()
        {
            for (int i = 0; i < logs.Length; i++)
            {
                logs[i].text = string.Empty;
            }
        }

        /// <summary>returns a text object based on log index</summary>
        private Text GetTextComponent()
        {
            if (logIndex == MAX_LOG_COUNT)
            {
                logIndex = 0;
                ClearLogs();
            }

            return logs[logIndex];
        }
    }
}