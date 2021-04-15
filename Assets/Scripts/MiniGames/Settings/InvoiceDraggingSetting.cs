using GameStudio.GeldZeker.MiniGames.Settings;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames
{
    [CreateAssetMenu(menuName = "Minigames/Settings/InvoiceDragging")]
    public class InvoiceDraggingSetting : MiniGameSetting
    {
        [Header("Easy Values")]
        [SerializeField]
        private int easyTimerSeconds = 0;

        [Header("Medium Values")]
        [SerializeField]
        private int mediumTimerSeconds = 0;

        [Header("Hard Values")]
        [SerializeField]
        private int hardTimerSeconds = 0;

        public float GetAmountOfSeconds()
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy: return easyTimerSeconds;
                case MiniGameDifficulty.Medium: return mediumTimerSeconds;
                case MiniGameDifficulty.Hard: return hardTimerSeconds;
            }
            return 0;
        }
    }
}