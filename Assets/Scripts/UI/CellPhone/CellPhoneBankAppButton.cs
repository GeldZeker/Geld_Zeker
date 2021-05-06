using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    /// <summary>Behaviour used for opening a bank app screen based on whether the player has an account or not</summary>
    public class CellPhoneBankAppButton : AudableButton
    {
        [Header("References")]
        [SerializeField]
        public BooleanProperty hasAccountProperty = null;

        [SerializeField]
        private BankAppointmentProperty appointmentProperty = null;

        [SerializeField]
        private GameObject appointmentDisplay = null;

        [SerializeField]
        private GameObject magnifierGameObjectZoom = null;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(OnAppClicked);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(OnAppClicked);
        }

        private void OnAppClicked()
        {
            hasAccountProperty = (BooleanProperty)PlayerPropertyManager.Instance.GetProperty("Heeft Bankrekening");
            appointmentProperty = PlayerPropertyManager.Instance.GetProperty<BankAppointmentProperty>("BankAfspraak");

            if (appointmentProperty.Value.ToString().Equals("CreateAccount"))
            {
                appointmentDisplay.GetComponent<RectTransform>().sizeDelta = new Vector2();
                appointmentDisplay.GetComponent<RectTransform>().anchoredPosition = new Vector2();
                magnifierGameObjectZoom.SetActive(true);
                MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.BankAppointmentSet);
            }
            else
            {
                MainCanvasManager.Instance.OpenCellPhoneScreen(hasAccountProperty.Value ? CellPhoneScreen.BankAccount : CellPhoneScreen.BankAppointment);
            }
        }
    }
}