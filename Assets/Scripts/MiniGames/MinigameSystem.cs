using UnityEngine;
using GameStudio.GeldZeker.MiniGames.Settings;
using BWolf.Behaviours.SingletonBehaviours;

namespace GameStudio.GeldZeker.MiniGames
{
    public class MinigameSystem : SingletonBehaviour<MinigameSystem>
    {
        [SerializeField]
        private MiniGameSettingAsset settingsAsset = null;

        protected override void Awake()
        {
            base.Awake();

            if (!isDuplicate)
            {
                settingsAsset.LoadFromFile();
            }
        }

        public string[] SceneNames
        {
            get
            {
                return settingsAsset.SceneNames;
            }
        }

        [ContextMenu("Restore")]
        public void Restore()
        {
            settingsAsset.Restore();
        }

        [ContextMenu("UnlockAll")]
        public void UnlockAll()
        {
            settingsAsset.UnlockAll();
        }

        public MiniGameSetting GetSetting(MiniGame minigame)
        {
            return settingsAsset.GetSetting(minigame);
        }

        public MiniGameSetting GetSetting(string nameOfScene)
        {
            return settingsAsset.GetSetting(nameOfScene);
        }

        public bool IsGameScene(string nameOfScene)
        {
            return settingsAsset.IsGameScene(nameOfScene);
        }
    }
}