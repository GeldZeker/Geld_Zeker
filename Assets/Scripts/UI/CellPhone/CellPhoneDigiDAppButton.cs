using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    /// <summary>Behaviour used for opening a DigiD app screen based on whether the player has an account or not</summary>
    public class CellPhoneDigiDAppButton : AudableButton
    {
        [Header("References")]
        [SerializeField]
        public DigiDProperty hasDigiDAccountProperty = null;

        [SerializeField]
        private GameObject LoginUnPwAnimationObject = null;
        [SerializeField]
        private GameObject LoginUnPwButtonObject = null;

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
            // Develop Phase
            LoginUnPwAnimationObject.SetActive(false);
            hasDigiDAccountProperty = PlayerPropertyManager.Instance.GetProperty<DigiDProperty>("DigiD");

            bool heeftAccount = true;

            if (!heeftAccount) MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDGeneralEmpty);
            else {
                MainCanvasManager.Instance.OpenCellPhoneScreen(CellPhoneScreen.DigiDLoginUnPw);
                LoginUnPwAnimationObject.SetActive(true);
                if (LoginUnPwButtonObject.activeInHierarchy) LoginUnPwButtonObject.SetActive(false);
                StartCoroutine(LateButtonShow());
            }
        }

        private IEnumerator LateButtonShow()
        {
            yield return new WaitForSeconds(2);
            LoginUnPwButtonObject.SetActive(true);
        }
    }
}