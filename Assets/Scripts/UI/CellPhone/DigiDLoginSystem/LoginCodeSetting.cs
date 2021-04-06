using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>Setting representing pin login settings</summary>
    public class LoginCodeSetting : MonoBehaviour
    {
        public const int PIN_LENGTH = 4;
        private string[] pin = new string[] { "0", "0", "0", "0" };
        
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