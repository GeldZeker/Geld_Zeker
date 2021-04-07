using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    public class CellPhoneDigiDAppNavigation : MonoBehaviour
    {
        [SerializeField]
        public static CellPhoneDigiDAppNavigation instance;

        [Header("References")]
        [SerializeField]
        public DigiDProperty hasDigiDAccountProperty = null;

        [SerializeField]
        private GameObject pinHelper = null;

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            
        }

        private void Update()
        {
            
        }

        public void LoginButtonClicked()
        {
            pinHelper.SetActive(true);
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDLoginPin);           
        }

        public void PinLoginCorrect()
        {
            hasDigiDAccountProperty = PlayerPropertyManager.Instance.GetProperty<DigiDProperty>("DigiD");

            pinHelper.SetActive(false);
            if (hasDigiDAccountProperty) { MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDGeneral); }
            else { MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDGeneralEmpty); }
        }

        public void AllowancesButtonClicked()
        {
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDAllowances);
        }
    }
}