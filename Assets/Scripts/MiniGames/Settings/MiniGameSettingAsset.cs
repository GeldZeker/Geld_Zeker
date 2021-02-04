using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.Settings
{
    [CreateAssetMenu(fileName = ASSET_NAME, menuName = "Minigames/Settings/Asset")]
    public class MiniGameSettingAsset : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Are Quests restored to their original default state when building the application")]
        private bool restoreOnBuild = true;

        [Header("MinigameSettings")]
        [SerializeField]
        private MiniGameSetting[] settings = null;

        public const string ASSET_NAME = "MinigameAsset";

        public bool RestoreOnBuild
        {
            get { return restoreOnBuild; }
        }

        public string[] SceneNames
        {
            get
            {
                string[] names = new string[settings.Length];
                for (int i = 0; i < settings.Length; i++)
                {
                    names[i] = settings[i].NameOfScene;
                }
                return names;
            }
        }

        public void LoadFromFile()
        {
            foreach (MiniGameSetting setting in settings)
            {
                setting.LoadProgressionFromFile();
            }
        }

        public void Restore()
        {
            foreach (MiniGameSetting setting in settings)
            {
                setting.Restore();
            }
        }

        public void UnlockAll()
        {
            foreach (MiniGameSetting setting in settings)
            {
                setting.UnlockAll();
            }
        }

        public MiniGameSetting GetSetting(MiniGame minigame)
        {
            foreach (MiniGameSetting setting in settings)
            {
                if (setting.Type == minigame)
                {
                    return setting;
                }
            }

            return null;
        }

        public MiniGameSetting GetSetting(string nameOfScene)
        {
            foreach (MiniGameSetting setting in settings)
            {
                if (setting.NameOfScene == nameOfScene)
                {
                    return setting;
                }
            }

            return null;
        }

        public bool IsGameScene(string nameOfScene)
        {
            foreach (MiniGameSetting setting in settings)
            {
                if (setting.NameOfScene == nameOfScene)
                {
                    return true;
                }
            }

            return false;
        }
    }
}