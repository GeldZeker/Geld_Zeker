//documentation links
//https://docs.unity3d.com/Packages/com.unity.mobile.notifications@1.3/manual/Android.html
//https://docs.unity3d.com/Packages/com.unity.mobile.notifications@1.3/api/Unity.Notifications.Android.AndroidNotificationCenter.html
//https://docs.unity3d.com/Packages/com.unity.mobile.notifications@1.3/api/Unity.Notifications.Android.AndroidNotificationChannel.html
//-----------------------------

using BWolf.Behaviours.SingletonBehaviours;
using Unity.Notifications.Android;
using UnityEngine;
using System;

namespace GameStudio.GeldZeker.Utilities
{
    public class GlobalNotificationsSystem : SingletonBehaviour<GlobalNotificationsSystem>
    {
        [Header("Channel Settings")]
        [SerializeField]
        private string channelId = "GeldZeker_App";

        [SerializeField]
        private string channelName = "GeldZeker_Channel";

        [SerializeField]
        private string channelDescription = "Geldzeker Notifications";

        [Header("Icon settings")]
        [SerializeField]
        private string hungerIconId = "hungericon";

        [SerializeField]
        private string happinessIconId = "happyicon";

        [SerializeField]
        private string smallIconId = "appicon";

        [SerializeField]
        private Importance importance = Importance.Default;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            var channel = new AndroidNotificationChannel
            {
                Id = channelId,
                Name = channelName,
                Importance = importance,
                Description = channelDescription,
                EnableVibration = true
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
        }

        /// <summary>Sends a notification of given type with given title, text and at given date</summary>
        public void SendNotification(NotificationType notificationType, string title, string text, DateTime dateTime)
        {
            switch (notificationType)
            {
                case NotificationType.Hunger:
                    SendNotification(title, text, hungerIconId, dateTime);
                    break;

                case NotificationType.Happiness:
                    SendNotification(title, text, happinessIconId, dateTime);
                    break;
            }
        }

        /// <summary>Sends a notification with given title, text and at given date</summary>
        private void SendNotification(string title, string text, string iconId, DateTime dateTime)
        {
            AndroidNotificationCenter.SendNotification(new AndroidNotification
            {
                Title = title,
                Text = text,
                FireTime = dateTime,
                LargeIcon = iconId,
                SmallIcon = smallIconId
            },
            channelId);
        }
    }

    public enum NotificationType
    {
        Hunger,
        Happiness
    }
}