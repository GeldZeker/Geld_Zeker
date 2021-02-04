using GameStudio.GeldZeker.Player.PlayerDialogue;
using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.Settings
{
    /// <summary>A behaviour representing a button that loads a minigame after dialogue</summary>
    public class LoadMiniGameAfterDialogue : LoadSceneAfterDialogueButton
    {
        [Header("MiniGame Settings")]
        [SerializeField]
        private MiniGameDifficulty difficulty = MiniGameDifficulty.Easy;

        [SerializeField]
        private MiniGameSetting setting = null;

        [SerializeField]
        private bool minigameMode = false;

        protected override void OnDialogueFinish()
        {
            base.OnDialogueFinish();

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