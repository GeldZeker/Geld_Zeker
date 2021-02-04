// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using GameStudio.GeldZeker.Player;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>A Quest Task for doing something once</summary>
    [CreateAssetMenu(menuName = "PlayerProgression/QuestTasks/DoOnceTask")]
    public class DoOnceTask : QuestTask
    {
        [SerializeField]
        private bool isDoneOnce = false;

        public override string TaskDescription
        {
            get
            {
                return description;
            }
        }

        public override bool IsDone
        {
            get
            {
                return isDoneOnce;
            }
        }

        public override float Progress
        {
            get { return isDoneOnce ? 1.0f : 0.0f; }
        }

        public override string ProgressString
        {
            get
            {
                return string.Format("({0}/1)", isDoneOnce ? 1 : 0);
            }
        }

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(DoOnceTask)}/{name}";
            if (GameFileSystem.LoadFromFile(path, out bool outValue))
            {
                isDoneOnce = outValue;
            }
        }

        public override void Restore()
        {
            isDoneOnce = false;
            SaveToFile();

#if UNITY_EDITOR
            //make sure that in the editor, restoring the task marks it as dirty so it can be saved
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        public override void SaveToFile()
        {
            string path = $"{FOLDER_PATH}/{nameof(DoOnceTask)}/{name}";
            GameFileSystem.SaveToFile(path, isDoneOnce);
        }

        /// <summary>Sets this task to done</summary>
        public void SetDoneOnce()
        {
            if (!isDoneOnce)
            {
                isDoneOnce = true;
                SaveToFile();
                Completed?.Invoke(this);
            }
        }
    }
}