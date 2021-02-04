using GameStudio.GeldZeker.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.GameNotifications
{
    /// <summary>The system responsible for handling game notifications</summary>
    public class GameNotificationSystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private DisplayInfo[] displayInfo = null;

        private Dictionary<GameNotificationType, DisplayInfo> displayInfoPairs = new Dictionary<GameNotificationType, DisplayInfo>();

        private void Awake()
        {
            for (int i = 0; i < displayInfo.Length; i++)
            {
                DisplayInfo display = displayInfo[i];
                displayInfoPairs.Add(display.NotificationType, display);
            }
        }

        /// <summary>Adds a notification with given description</summary>
        public void AddNotification(GameNotificationType notificationType)
        {
            DisplayInfo info = displayInfoPairs[notificationType];
            NotificationUtility.Instance.Notify(info.ShortMessage, NotificationStayTime.Average, info.Sprite);
        }

        [System.Serializable]
        private struct DisplayInfo
        {
#pragma warning disable 0649
            public GameNotificationType NotificationType;
            public Sprite Sprite;
            public string ShortMessage;
#pragma warning restore 0649
        }
    }
}