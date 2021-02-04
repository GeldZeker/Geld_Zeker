using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.Settings
{
    /// <summary>Setting representing mail ordering settings</summary>
    [CreateAssetMenu(menuName = "Minigames/Settings/MailOrdering")]
    public class MailOrderSetting : MiniGameSetting
    {
        [Header("Easy Values")]
        [SerializeField]
        public float easyStartDelay;

        [SerializeField]
        public float easyReverseDelay;

        [SerializeField]
        public float easyTurnTime;

        [Header("Medium Values")]
        [SerializeField]
        public float mediumStartDelay;

        [SerializeField]
        public float mediumReverseDelay;

        [SerializeField]
        public float mediumTurnTime;

        [Header("Hard Values")]
        [SerializeField]
        public float hardStartDelay;

        [SerializeField]
        public float hardReverseDelay;

        [SerializeField]
        public float hardMinTurnTime;

        [SerializeField]
        public float hardMaxTurnTime;

        public float GetStartDelay()
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy: return easyStartDelay;
                case MiniGameDifficulty.Medium: return mediumStartDelay;
                case MiniGameDifficulty.Hard: return hardStartDelay;
            }

            return 0.0f;
        }

        public float GetReverseDelay()
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy: return easyReverseDelay;
                case MiniGameDifficulty.Medium: return mediumReverseDelay;
                case MiniGameDifficulty.Hard: return hardReverseDelay;
            }

            return 0.0f;
        }

        public float GetTurnTime()
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy: return easyTurnTime;
                case MiniGameDifficulty.Medium: return mediumTurnTime;
                case MiniGameDifficulty.Hard: return Random.Range(hardMinTurnTime, hardMaxTurnTime);
            }

            return 0.0f;
        }
    }
}