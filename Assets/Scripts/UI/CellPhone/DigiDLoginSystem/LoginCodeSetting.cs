using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>Setting representing pin login settings</summary>
    [CreateAssetMenu(menuName = "UI/CellPhone/DigiDLoginSystem/LoginCode")]
    public class LoginCodeSetting : ScriptableObject
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