// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Progression;
using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A scriptable object for storing quest information, containing tasks to complete it</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/Quests/Quest")]
    public class Quest : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField]
        private string description = string.Empty;

        [SerializeField]
        private bool isCompleted = false;

        [SerializeField]
        private bool isActive = false;

        [SerializeField]
        private ProgressionLevel level = ProgressionLevel.ONE;

        [Header("References")]
        [SerializeField]
        private QuestTask[] tasks = null;

        [Space]
        [SerializeField]
        private Quest requiredQuest = null;

        public event Action<Quest, bool> ActiveStateChanged;

        public event Action<Quest> Completed;

        private const string FOLDER_PATH = "ProgressSaves/Quests/ActiveQuests";

        public string Description
        {
            get { return description; }
        }

        public bool IsActive
        {
            get { return isActive; }
        }

        public bool IsActivatable
        {
            get { return !isActive && FinishedRequiredQuest; }
        }

        public bool IsUpdatable
        {
            get { return isActive && !isCompleted; }
        }

        private bool FinishedRequiredQuest
        {
            get { return requiredQuest == null || requiredQuest.isCompleted; }
        }

        public Quest RequiredQuest
        {
            get { return requiredQuest; }
        }

        public QuestTask[] Tasks
        {
            get { return tasks; }
        }

        public ProgressionLevel Level
        {
            get { return level; }
        }

        public bool IsCompleted
        {
            get { return isCompleted; }
        }

        public float Progress
        {
            get
            {
                float totalProgress = 0.0f;
                for (int i = 0; i < tasks.Length; i++)
                {
                    totalProgress += tasks[i].Progress;
                }

                return Mathf.Clamp01(totalProgress / tasks.Length);
            }
        }

        /// <summary>Updates this quest by checking the progress of the tasks</summary>
        public void Update()
        {
            if (isCompleted)
            {
                return;
            }

            float totalProgress = 0.0f;
            for (int i = 0; i < tasks.Length; i++)
            {
                totalProgress += tasks[i].Progress;
            }

            if (totalProgress >= tasks.Length)
            {
                Completed?.Invoke(this);
                SetActive(false);
                isCompleted = true;
            }
        }

        /// <summary>Returns a Task of Type T based on given name</summary>
        public T GetTask<T>(string nameOfTask) where T : QuestTask
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].name == nameOfTask)
                {
                    return (T)tasks[i];
                }
            }

            return null;
        }

        /// <summary>Returns a Task based on given name</summary>
        public QuestTask GetTask(string nameOfTask)
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                if (tasks[i].name == nameOfTask)
                {
                    return tasks[i];
                }
            }

            return null;
        }

        /// <summary>Loads the task data from local storage</summary>
        public void LoadTasksFromFile()
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].LoadFromFile();
            }
        }

        /// <summary>Sets the active state of this quest</summary>
        public void SetActive(bool value, bool fromFile = false)
        {
            if (value != isActive)
            {
                isActive = value;
                ActiveStateChanged?.Invoke(this, isActive);

                if (!fromFile)
                {
                    SaveActiveStateToFile();
                }
            }
        }

        /// <summary>Resets this quest, resetting its tasks and setting it to be inactive and incomplete</summary>
        public void Restore()
        {
            for (int i = 0; i < tasks.Length; i++)
            {
                tasks[i].Restore();
            }

#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                ActiveStateChanged = null;
#endif

            isCompleted = false;
            SetActive(false);

#if UNITY_EDITOR
            //make sure that in the editor, restoring the quest marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves the active state of this quest to local storage</summary>
        public void SaveActiveStateToFile()
        {
            string path = $"{FOLDER_PATH}/{name}";
            GameFileSystem.SaveToFile(path, isActive);
        }

        /// <summary>Loads the active state of this quest from local storage and assigns its value</summary>
        public void LoadActiveStateFromFile()
        {
            string path = $"{FOLDER_PATH}/{name}";
            if (GameFileSystem.LoadFromFile(path, out bool outValue))
            {
                SetActive(outValue, true);
            }
        }
    }
}