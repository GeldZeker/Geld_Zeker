using GameStudio.GeldZeker.MiniGames.Settings;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.GameHall
{
    public class MinigameDifficultyButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Color uninteractableColor = Color.grey;

        [Header("References")]
        [SerializeField]
        private Image icon = null;

        private LoadMiniGameButton minigameButton;
        private Button button;

        private void Awake()
        {
            minigameButton = GetComponent<LoadMiniGameButton>();
            button = GetComponent<Button>();
        }

        public void SetSetting(MiniGameSetting setting)
        {
            MiniGameDifficulty difficulty = minigameButton.Difficulty;
            if (difficulty != MiniGameDifficulty.Easy)
            {
                bool hasCompletedDifficulty = setting.HasCompletedDifficulty(difficulty - 1);
                button.interactable = hasCompletedDifficulty;
                icon.color = hasCompletedDifficulty ? Color.white : uninteractableColor;
            }
        }
    }
}