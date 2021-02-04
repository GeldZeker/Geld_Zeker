using GameStudio.GeldZeker.MiniGames.Settings;
using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames
{
    /// <summary>Setting representing mail picking settings</summary>
    [CreateAssetMenu(menuName = "Minigames/Settings/MailPicking")]
    public class MailPickingSetting : MiniGameSetting
    {
        [Header("Easy Values")]
        [SerializeField]
        public int easyAmountOfLayers;

        [SerializeField]
        public bool easySameEnvelopes;

        [Header("Medium Values")]
        [SerializeField]
        public int mediumAmountOfLayers;

        [SerializeField]
        public bool mediumSameEnvelopes;

        [Header("Hard Values")]
        [SerializeField]
        public int hardAmountOfLayers;

        [SerializeField]
        public bool hardSameEnvelopes;

        public float GetAmountOfLayers()
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy: return easyAmountOfLayers;
                case MiniGameDifficulty.Medium: return mediumAmountOfLayers;
                case MiniGameDifficulty.Hard: return hardAmountOfLayers;
            }
            return 0.0f;
        }

        public bool GetHardEnvelopes()
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy: return easySameEnvelopes;
                case MiniGameDifficulty.Medium: return mediumSameEnvelopes;
                case MiniGameDifficulty.Hard: return hardSameEnvelopes;
            }
            return true;
        }
    }
}