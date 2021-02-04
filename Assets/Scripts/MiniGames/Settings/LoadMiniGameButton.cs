using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.Settings
{
    /// <summary>A behaviour representing a button that loads a minigame</summary>
    public class LoadMiniGameButton : SceneTransitioning.LoadSceneButton
    {
        [Header("MiniGame Settings")]
        [SerializeField]
        private MiniGameDifficulty difficulty = MiniGameDifficulty.Easy;

        [SerializeField]
        private bool minigameMode = false;

        [SerializeField]
        private MiniGameSetting setting = null;

        public MiniGameSetting Setting
        {
            get { return setting; }
        }

        public MiniGameDifficulty Difficulty
        {
            get { return difficulty; }
        }

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(SetMiniGameDifficulty);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(SetMiniGameDifficulty);
        }

        public void SetSetting(MiniGameSetting setting, bool minigameMode)
        {
            this.setting = setting;
            this.minigameMode = minigameMode;
        }

        private void SetMiniGameDifficulty()
        {
            if (setting)
            {
                setting.SetDifficulty(difficulty);
                setting.SetMinigameMode(minigameMode);
            }
            else
            {
                Debug.LogWarning($"no difficulty setting has been referened in the {gameObject}. Make sure to have a difficulty setting for each minigame");
            }
        }
    }
}