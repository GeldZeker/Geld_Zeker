using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    public class CellPhoneDigiDAppNavigation : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        public DigiDProperty hasDigiDAccountProperty = null;

        public void Start()
        {
            
        }

        public void Update()
        {
            
        }

        public void LoginButtonClicked()
        {
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDLoginPin);
        }

        public void AllowancesButtonClicked()
        {
            MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDAllowances);
        }
    }
}