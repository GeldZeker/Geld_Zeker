using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    /// <summary>A behaviour representing some navigation aspects of the DigiD App in-game.</summary>
    public class CellPhoneDigiDAppNavigation : MonoBehaviour
    {
        [SerializeField]
        public static CellPhoneDigiDAppNavigation instance;

        [Header("References")]
        [SerializeField]
        public DigiDProperty hasDigiDAccountProperty = null;

        [SerializeField]
        private GameObject pinHelper = null;

        [SerializeField]
        private Quest digidBekijkenQuest = null;

        [Header("Tamagotchi")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private int happinessOnCompletion = 10;

        private void Awake()
        {
            instance = this;
        }

        /// <summary>OnClick function to Login with Username and Password. Redirects to Login with Pin.</summary>
        public void LoginButtonClicked()
        {
            pinHelper.SetActive(true);
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDLoginPin);           
        }

        /// <summary>PinLogin function gets called when Pin is correct. Redirects to DigiDGeneral or DigiDGeneralEmpty based on DigiDAccount Property</summary>
        public void PinLoginCorrect()
        {
            hasDigiDAccountProperty = PlayerPropertyManager.Instance.GetProperty<DigiDProperty>("DigiD");

            if (pinHelper.activeSelf) pinHelper.SetActive(false);
            if (hasDigiDAccountProperty.Value) { MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDGeneral); }
            else { MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDGeneralEmpty); }
        }

        /// <summary>OnClick function for DigiDGeneral screen. Redirects to Allowances screen.</summary>
        public void AllowancesButtonClicked()
        {
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDAllowances);
            if (digidBekijkenQuest.IsUpdatable)
            {
                DoOnceTask watchAllowancesTask = digidBekijkenQuest.GetTask<DoOnceTask>("1KeerToeslagenBekijken");
                watchAllowancesTask.SetDoneOnce();
                happiness.AddValue(happinessOnCompletion);
            }
        }
    }
}