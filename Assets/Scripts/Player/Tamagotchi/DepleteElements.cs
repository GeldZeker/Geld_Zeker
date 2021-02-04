using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using System;
using System.Collections;
using Unity.Notifications.Android;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Tamagotchi
{
    /// <summary>Slowly depletes the Tamagotchi elements</summary>
    public class DepleteElements : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string happinessAtZeroTitle = "Je personage is ongelukkig!";

        [SerializeField]
        private string happinessAtZeroMessage = "Kom snel terug om je personage weer gelukkig te maken.";

        [SerializeField]
        private string hungerAtZeroTitle = "Je personage heeft honger!";

        [SerializeField]
        private string hungerAtZeroMessage = "Kom snel terug om je personage eten te geven.";

        [Header("Interval/Each ?s")]
        [SerializeField]
        private int intervalTime = 360;

        [Header("References")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private TamagotchiElementProperty hunger = null;

        private IEnumerator coroutine;

        private static bool focused;

        private const string FILE_NAME = "Time/DateTimeClosed";

        // Start is called before the first frame update
        private void Start()
        {
            focused = true;

            LoadDateTimeClosed();

            coroutine = WaitBeforeSubtract(intervalTime);
            StartCoroutine(coroutine);
        }

        /// <summary>Loads the last datetime at which the application closed and initializes the happiness and hunger percentages based on it</summary>
        private void LoadDateTimeClosed()
        {
            if (GameFileSystem.LoadFromFile(FILE_NAME, out long outValue))
            {
                DateTime lastDateTimeClosed = DateTime.FromFileTime(outValue);
                double seconds = (DateTime.Now - lastDateTimeClosed).TotalSeconds;

                int happinessPercentageLost = Mathf.RoundToInt((float)seconds * ((float)happiness.Depletion / intervalTime));
                happiness.RemoveValue(happinessPercentageLost);

                int hungerPercentageLost = Mathf.RoundToInt((float)seconds * ((float)hunger.Depletion / intervalTime));
                hunger.RemoveValue(hungerPercentageLost);
            }
        }

        /// <summary>Saves the current datatime as file time</summary>
        private void SaveDateTimeClosed()
        {
            GameFileSystem.SaveToFile(FILE_NAME, DateTime.Now.ToFileTime());
        }

        private IEnumerator WaitBeforeSubtract(float waitTime)
        {
            while (true)
            {
                yield return new WaitForSeconds(waitTime);
                DepleteTamagotchiElements();
            }
        }

        private void OnApplicationFocus(bool focus)
        {
            focused = focus;
        }

        private void OnApplicationPause(bool isPaused)
        {
            if (isPaused && focused)
            {
                SendNotifications();
                SaveDateTimeClosed();
            }
        }

        private void OnApplicationQuit()
        {
            SendNotifications();
        }

        /// <summary>schedules notificatins for hunger and happiness properties reaching zero</summary>
        private void SendNotifications()
        {
            AndroidNotificationCenter.CancelAllScheduledNotifications();

            DateTime firetime = DateTime.Now.AddDays(1d);
            GlobalNotificationsSystem.Instance.SendNotification(NotificationType.Happiness, happinessAtZeroTitle, happinessAtZeroMessage, firetime);
            GlobalNotificationsSystem.Instance.SendNotification(NotificationType.Hunger, hungerAtZeroTitle, hungerAtZeroMessage, firetime);
        }

        private void DepleteTamagotchiElements()
        {
            happiness.Deplete();
            hunger.Deplete();
        }
    }
}