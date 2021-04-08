using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>Setting representing Pin Login settings</summary>
    [CreateAssetMenu(menuName = "UI/CellPhone/DigiDLoginSystem/LoginCode")]
    public class LoginCodeSetting : ScriptableObject
    {
        public const int PIN_LENGTH = 4;
        private string[] pin = new string[] { "0", "0", "0", "0" };

        /// <summary>Function to generate a random Pin with 4 characters from 0-9.</summary>
        public string[] GenerateRandomPin()
        {
            for (int i = 0; i < PIN_LENGTH; i++)
            {
                pin[i] = Random.Range(0, 10).ToString();
            }

            return pin;
        }
    }
}