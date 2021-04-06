using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.Settings
{
    /// <summary>Setting representing debit card playment settings</summary>
    [CreateAssetMenu(menuName = "Minigames/Settings/DebitCardPayment")]
    public class DebitCardPaySetting : MiniGameSetting
    {
        public const int PINCODE_LENGTH = 4;
        private string[] pincode = new string[] { "0", "0", "0", "0" };
        
        public string[] GenerateRandomPincode()
        {
            for (int i = 0; i < PINCODE_LENGTH; i++)
            {
                pincode[i] = Random.Range(0, 10).ToString();
            }

            return pincode;
        }
    }
}