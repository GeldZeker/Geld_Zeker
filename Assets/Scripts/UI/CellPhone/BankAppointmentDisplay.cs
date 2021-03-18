using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
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

        public GameObject magnifierGameObject = null;

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
                magnifierGameObject.SetActive(true);

                DoOnceTask orderMailTask = createbankAccountQuest.GetTask<DoOnceTask>("BankAfspraak");
                orderMailTask.SetDoneOnce();
            }
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
            // Magnify Event
            Debug.Log("Magnifier clicked!");
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