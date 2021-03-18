using BWolf.Utilities.CharacterDialogue;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Interaction;
using GameStudio.GeldZeker.MiniGames.MailOrdering;
using GameStudio.GeldZeker.Player.GameNotifications;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.UI;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.PlayerDialogue
{
    public class BankDialogueStarter : MonoBehaviour
    {
        [Header("Player Properties")]
        [SerializeField]
        private BankAppointmentProperty appointmentProperty = null;

        [SerializeField]
        private PlayerMailProperty mailProperty = null;

        [Header("Quests")]
        [SerializeField]
        private Quest createbankAccountQuest = null;

        [Header("Appointments")]
        [SerializeField]
        private Appointment[] appointments = null;

        private Dictionary<BankAppointmentType, Appointment> appointmentpairs = new Dictionary<BankAppointmentType, Appointment>();

        private void Awake()
        {
            for (int i = 0; i < appointments.Length; i++)
            {
                Appointment app = appointments[i];
                appointmentpairs.Add(app.AppointmentType, app);
            }
        }

        private void Start()
        {
            TryStartDialogue();
        }

        private void TryStartDialogue()
        {
            BankAppointmentType playerAppointment = appointmentProperty.Value;
            if (playerAppointment == BankAppointmentType.None)
            {
                //stop if no appointment was made
                return;
            }

            appointmentProperty.Restore();

            switch (playerAppointment)
            {
                case BankAppointmentType.CreateAccount:
                    MainCanvasManager.Instance.StartDialogue(appointmentpairs[playerAppointment].AppointmentDialogue, OnCreateAccountDialogueFinished);

                    break;
            }
        }

        private void OnCreateAccountDialogueFinished()
        {
            mailProperty.AddMail(MailType.Bank);

            MainCanvasManager.Instance.AddNotification(GameNotificationType.Mail);

            if (createbankAccountQuest.IsUpdatable)
            {
                DoOnceTask holdBankAccountDialogue = createbankAccountQuest.GetTask<DoOnceTask>("BankAccountGesprek");
                holdBankAccountDialogue.SetDoneOnce();
            }

            PlayerPropertyManager.Instance.GetProperty<BooleanProperty>("Heeft Bankrekening").UpdateValue(true, false);
        }

        [System.Serializable]
        private struct Appointment
        {
#pragma warning disable 0649
            public BankAppointmentType AppointmentType;
            public Dialogue AppointmentDialogue;
#pragma warning restore 0649
        }
    }
}