using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.GameHall
{
    /// <summary>A behaviour for opening the game start screen on button click</summary>
    public class OpenGameStartScreenButton : AudableButton
    {
        [Header("Settings")]
        [SerializeField]
        private MiniGame miniGameToOpen = MiniGame.DebitCardPayment;

        [Header("References")]
        [SerializeField]
        private GameStartDisplay startDisplay = null;

        [SerializeField]
        private GameObject lockObject = null;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(OpenMinigame);

            MiniGameSetting setting = MinigameSystem.Instance.GetSetting(miniGameToOpen);
            lockObject.SetActive(!setting.IsCompletedInStoryMode);
            button.interactable = setting.IsCompletedInStoryMode;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(OpenMinigame);
        }

        private void OpenMinigame()
        {
            startDisplay.SetMiniGame(miniGameToOpen);
            startDisplay.ToggleActiveState();
        }
    }
}