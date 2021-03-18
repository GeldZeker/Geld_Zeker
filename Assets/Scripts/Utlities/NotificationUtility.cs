using BWolf.Behaviours.SingletonBehaviours;
using BWolf.Utilities;
using GameStudio.GeldZeker.Audio;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>A utility singleton class for managing notifications for things that are updated or are and need a warning</summary>
    public class NotificationUtility : SingletonBehaviour<NotificationUtility>
    {
        [Header("NotifySettings")]
        [SerializeField]
        private RectTransform notificationTransform = null;

        [SerializeField]
        private Text txtNotification = null;

        [SerializeField]
        private Image imgNotification = null;

        [SerializeField]
        private Sprite defaultIcon = null;

        [SerializeField]
        private Color defaultIconColor = Color.black;

        [SerializeField]
        private float notifySpeed = 1.0f;

        [SerializeField]
        private float notificationTopSpacing = 25f;

        [Header("VerifySettings")]
        [SerializeField]
        private GameObject verificationObject = null;

        [SerializeField]
        private Text verificationTextComponent = null;

        [SerializeField]
        private Button continueButton = null;

        [SerializeField]
        private Button stopButton = null;

        private RectTransform verificationTransform;

        private bool isNotifying;
        private bool isVerifying;

        private Queue<QueableNotification> notificationQueue = new Queue<QueableNotification>();

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            verificationTransform = verificationObject.GetComponent<RectTransform>();

            stopButton.onClick.AddListener(OnVerificationButtonClick);
        }

        private void Update()
        {
            if (isVerifying && (RectTransformExtensions.PressOutsideTransform(verificationTransform) || RectTransformExtensions.PressOutsideTransform(verificationTransform)))
            {
                StopVerification();
            }
        }

        /// <summary>Notifies the player with given message</summary>
        public void Notify(string message, NotificationStayTime speed, Sprite icon = null)
        {
            var queable = new QueableNotification { Message = message, StayTime = speed, Icon = icon };
            if (!isNotifying)
            {
                StartNofication(queable);
            }
            else
            {
                notificationQueue.Enqueue(queable);
            }
        }

        /// <summary>Starts the notification popup with given message</summary>
        private void StartNofication(QueableNotification notification)
        {
            if (notification.Icon != null)
            {
                imgNotification.sprite = notification.Icon;
                imgNotification.color = Color.white;
            }
            else
            {
                imgNotification.sprite = defaultIcon;
                imgNotification.color = defaultIconColor;
            }

            txtNotification.text = notification.Message;
            StartCoroutine(NotifyEnumerator((int)notification.StayTime));
        }

        /// <summary>Shows a verification screen with given message, execution the onContinue method when the user continues anyway</summary>
        public void Verify(string message, UnityAction onContinue)
        {
            if (!verificationObject.activeInHierarchy)
            {
                verificationObject.SetActive(true);
                verificationTextComponent.text = message;
                continueButton.onClick.AddListener(onContinue);
                continueButton.onClick.AddListener(OnVerificationButtonClick);

                isVerifying = true;
            }
            else
            {
                Debug.LogWarning("Tried verifying while another verification was already on screen :: this is not intended behaviour");
            }
        }

        /// <summary>Called when the verification button has been clicked to reset the screen</summary>
        private void OnVerificationButtonClick()
        {
            MusicPlayer.Instance.PlaySFXSound(SFXSound.DefaultButtonClick);
            StopVerification();
        }

        /// <summary>Stops the verification process</summary>
        private void StopVerification()
        {
            continueButton.onClick.RemoveAllListeners();
            verificationObject.SetActive(false);

            isVerifying = false;
        }

        /// <summary>Returns an enumerator that notifies the player by showing a small screen on top of the screen and then hiding it after a delay</summary>
        private IEnumerator NotifyEnumerator(float stayTime)
        {
            isNotifying = true;

            float offset = notificationTransform.rect.height;
            LerpValue<float> movedownward = new LerpValue<float>(offset, -notificationTopSpacing, notifySpeed, LerpSettings.Cosine);
            while (movedownward.Continue())
            {
                Vector2 position = new Vector2(notificationTransform.anchoredPosition.x, Mathf.Lerp(movedownward.start, movedownward.end, movedownward.perc));
                notificationTransform.anchoredPosition = position;
                yield return null;
            }

            yield return new WaitForSeconds(stayTime + 4f);

            LerpValue<float> moveup = new LerpValue<float>(-notificationTopSpacing, offset, notifySpeed, LerpSettings.Cosine);
            while (moveup.Continue())
            {
                Vector2 position = new Vector2(notificationTransform.anchoredPosition.x, Mathf.Lerp(moveup.start, moveup.end, moveup.perc));
                notificationTransform.anchoredPosition = position;
                yield return null;
            }

            if (notificationQueue.Count != 0)
            {
                StartNofication(notificationQueue.Dequeue());
            }
            else
            {
                isNotifying = false;
            }
        }

        private struct QueableNotification
        {
            public string Message;
            public NotificationStayTime StayTime;
            public Sprite Icon;
        }
    }

    public enum NotificationStayTime
    {
        Short = 1,
        Average = 2,
        Long = 3
    }
}