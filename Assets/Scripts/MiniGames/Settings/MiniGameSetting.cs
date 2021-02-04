using GameStudio.GeldZeker.Player;
using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.Settings
{
    public class MiniGameSetting : ScriptableObject
    {
        [Header("Scene")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        [Header("General")]
        [SerializeField]
        private MiniGame type = MiniGame.DebitCardPayment;

        [SerializeField]
        private bool minigameMode = false;

        [Header("Progression")]
        [SerializeField]
        private Progression progression = default;

        [Header("Difficulty")]
        [SerializeField]
        protected MiniGameDifficulty difficulty = MiniGameDifficulty.Easy;

        public const string FOLDER_PATH = "ProgressSaves/MinigameSettings";

        public MiniGameDifficulty Difficulty
        {
            get { return difficulty; }
        }

        public string NameOfScene
        {
            get { return nameOfScene; }
        }

        public MiniGame Type
        {
            get { return type; }
        }

        public bool MinigameMode
        {
            get { return minigameMode; }
        }

        public bool IsCompletedInStoryMode
        {
            get { return progression.IsCompletedInStoryMode; }
        }

        public void SetMinigameMode(bool value)
        {
            minigameMode = value;
        }

        public void SetDifficulty(MiniGameDifficulty newDifficulty)
        {
            difficulty = newDifficulty;
        }

        public bool HasCompletedDifficulty(MiniGameDifficulty difficulty)
        {
            return progression.HasCompletedDifficulty(difficulty);
        }

        public void SetCurrentDifficultyCompleted()
        {
            if (progression.SetDifficultyCompleted(difficulty))
            {
                SaveProgressionToFile();
            }
        }

        public void UnlockAll()
        {
            progression.SetIsCompletedInStoryMode();

            foreach (MiniGameDifficulty difficulty in System.Enum.GetValues(typeof(MiniGameDifficulty)))
            {
                progression.SetDifficultyCompleted(difficulty);
            }

            SaveProgressionToFile();
        }

        public void SetIsCompletedInStoryMode()
        {
            if (!progression.IsCompletedInStoryMode)
            {
                progression.SetIsCompletedInStoryMode();
                SaveProgressionToFile();
            }
        }

        [ContextMenu("Restore")]
        public void Restore()
        {
            progression.Restore();
            SaveProgressionToFile();
        }

        private void SaveProgressionToFile()
        {
            string path = $"{FOLDER_PATH}/{name}";
            GameFileSystem.SaveToFile(path, progression);
        }

        public void LoadProgressionFromFile()
        {
            string path = $"{FOLDER_PATH}/{name}";
            if (GameFileSystem.LoadFromFile(path, out Progression progress))
            {
                progression.Override(progress);
            }
        }

        [System.Serializable]
        private class Progression
        {
            public bool IsCompletedInStoryMode = false;

            public DifficultySetting[] DifficultySettings = null;

            public void SetIsCompletedInStoryMode()
            {
                IsCompletedInStoryMode = true;
            }

            public bool HasCompletedDifficulty(MiniGameDifficulty difficulty)
            {
                for (int i = 0; i < DifficultySettings.Length; i++)
                {
                    if (DifficultySettings[i].Type == difficulty)
                    {
                        return DifficultySettings[i].Completed;
                    }
                }
                return false;
            }

            public void Override(Progression progression)
            {
                IsCompletedInStoryMode = progression.IsCompletedInStoryMode;
                DifficultySettings = progression.DifficultySettings;
            }

            public bool SetDifficultyCompleted(MiniGameDifficulty difficulty)
            {
                for (int i = 0; i < DifficultySettings.Length; i++)
                {
                    if (DifficultySettings[i].Type == difficulty)
                    {
                        if (!DifficultySettings[i].Completed)
                        {
                            DifficultySettings[i].Completed = true;
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }

                return false;
            }

            public void Restore()
            {
                IsCompletedInStoryMode = false;

                for (int i = 0; i < DifficultySettings.Length; i++)
                {
                    DifficultySettings[i].Completed = false;
                }
            }

            [System.Serializable]
            public struct DifficultySetting
            {
#pragma warning disable 0649
                public MiniGameDifficulty Type;
                public bool Completed;
#pragma warning restore 0649
            }
        }
    }
}