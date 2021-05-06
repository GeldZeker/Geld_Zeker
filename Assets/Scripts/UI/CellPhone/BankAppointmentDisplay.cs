using BWolf.Utilities;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    /// <summary>A cell phone display for making an appointment to get a bank account</summary>
    public class BankAppointmentDisplay : CellPhoneDisplay
    {
        [Header("References")]
        [SerializeField]
        private Sprite finishedSprite = null;

        [SerializeField]
        private Button btnMakeAppointment = null;

        [Header("Progression")]
        [SerializeField]
        private BankAppointmentProperty appointmentProperty = null;

        [SerializeField]
        private Quest createbankAccountQuest = null;

        public GameObject magnifierGameObjectZoom = null;

        [SerializeField]
        private RectTransform documentTransform = null;

        protected override void Awake()
        {
            base.Awake();

            btnMakeAppointment.onClick.AddListener(MakeAppointment);
        }

        private void OnDestroy()
        {
            btnMakeAppointment.onClick.RemoveListener(MakeAppointment);
        }

        public void MakeAppointment()
        {
            SetAppointmentState(true);

            if (createbankAccountQuest.IsUpdatable)
            {
                //update player property and task related to action
                appointmentProperty.UpdateAppointment(BankAppointmentType.CreateAccount);

                DoOnceTask orderMailTask = createbankAccountQuest.GetTask<DoOnceTask>("BankAfspraak");
                orderMailTask.SetDoneOnce();
            }
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.BankAppointmentSet);
            magnifierGameObjectZoom.SetActive(true);
        }

        /// <summary>Sets the display state by either showing the appointment screen with the appointment button or appointment finished screen</summary>
        public void SetAppointmentState(bool value)
        {
            display.sprite = value ? finishedSprite : startSprite;
            btnMakeAppointment.gameObject.SetActive(!value);
        }

        public override void SetActive(bool value)
        {
            base.SetActive(value);

            if (!value)
            {
                //if made inactive, stop showing the appointment screen
                SetAppointmentState(false);
            }
            else
            {
                btnMakeAppointment.interactable = createbankAccountQuest.IsActive && !createbankAccountQuest.GetTask("BankAfspraak").IsDone;
            }

        }

        public void MagnifyDocument()
        {
            magnifierGameObjectZoom.SetActive(false);
            StartCoroutine(MagnifyEnumerator());
        }

        private IEnumerator MagnifyEnumerator()
        {
            float offsetStretch;
            float offsetMoveUp;

            switch (Screen.height)
            {
                case 1440:
                    {
                        offsetStretch = Screen.height * 0.25f;
                        offsetMoveUp = Screen.height * 0.01f;
                    }
                    break;
                default:
                    {
                        offsetStretch = 450;
                        offsetMoveUp = 50;
                    }
                    break;
            }

            LerpValue<float> stretch = new LerpValue<float>(0, offsetStretch, 1.1f, LerpSettings.Cosine);
            LerpValue<float> moveup = new LerpValue<float>(0, offsetMoveUp, 1.1f, LerpSettings.Cosine);
            while (stretch.Continue() && moveup.Continue())
            {
                Vector2 size = new Vector2(Mathf.Lerp(stretch.start, stretch.end, stretch.perc), Mathf.Lerp(stretch.start, stretch.end, stretch.perc));
                documentTransform.sizeDelta = size;


                Vector2 position = new Vector2(documentTransform.anchoredPosition.x, Mathf.Lerp(moveup.start, moveup.end, moveup.perc));
                documentTransform.anchoredPosition = position;

                yield return null;
            }
        }

        /// <summary>Tries going back to the start screen. Returns if it did succesfully</summary>
        public override bool GoBack()
        {
            if (display.sprite == finishedSprite)
            {
                SetAppointmentState(false);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}