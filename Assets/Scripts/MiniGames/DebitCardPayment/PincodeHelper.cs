using GameStudio.GeldZeker.MiniGames.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.MiniGames.DebitCardPayment
{
    /// <summary>A behaviour showing pincode text to players playing the debit card pay game in mini game mode</summary>
    public class PincodeHelper : MonoBehaviour
    {
        [SerializeField]
        private DebitCardPaySetting setting = null;

        [SerializeField]
        private Text txtPincode = null;

        /// <summary>The static pincode available in mini game mode</summary>
        public static string[] Pincode { get; private set; }

        private void Awake()
        {
            if (setting.MinigameMode && setting.Difficulty != MiniGameDifficulty.Easy)
            {
                Pincode = setting.GenerateRandomPincode();
                txtPincode.text = $"Je Pincode is {Pincode[0]}{Pincode[1]}{Pincode[2]}{Pincode[3]}";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}