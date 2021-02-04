using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.MiniGames.Settings;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.GameHall
{
    /// <summary>A behaviour for locking a playable minigame based on whether it is completed in story mode</summary>
    public class MinigameLock : MonoBehaviour
    {
        [SerializeField]
        private MiniGame minigame = MiniGame.DebitCardPayment;

        [SerializeField]
        private GameObject lockObject = null;

        private void Awake()
        {
            MiniGameSetting setting = MinigameSystem.Instance.GetSetting(minigame);
            lockObject.SetActive(!setting.IsCompletedInStoryMode);
        }
    }
}