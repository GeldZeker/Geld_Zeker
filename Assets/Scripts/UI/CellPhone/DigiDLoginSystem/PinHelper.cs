using GameStudio.GeldZeker.MiniGames.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>A behaviour showing pin text to players at the DigiD pin Login</summary>
    public class PinHelper : MonoBehaviour
    {
        [SerializeField]
        private Text txtPin = null;

        [SerializeField]
        private LoginCodeSetting setting = null;

        /// <summary>The static pin available in DigiD</summary>
        public static string[] Pin { get; private set; }

        private void Awake()
        {
            Pin = setting.GenerateRandomPin();
            txtPin.text = $"Je Pin is {Pin[0]}{Pin[1]}{Pin[2]}{Pin[3]}";
        }
    }
}