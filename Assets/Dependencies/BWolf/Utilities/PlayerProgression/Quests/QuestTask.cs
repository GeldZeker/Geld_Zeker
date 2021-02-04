// Created By: Benjamin van der Wolf @ https://bvanderwolf.github.io/
// Version: 1.0
//----------------------------------

using System;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>An abstract class for defining a quest task to be part of completing a quest</summary>
    public abstract class QuestTask : ScriptableObject
    {
        [SerializeField]
        protected string description = string.Empty;

        protected const string FOLDER_PATH = "ProgressSaves/Quests/QuestTasks";

        public Action<QuestTask> Completed;

        public abstract bool IsDone { get; }

        public abstract string TaskDescription { get; }
        public abstract string ProgressString { get; }
        public abstract float Progress { get; }

        public abstract void LoadFromFile();

        public abstract void SaveToFile();

        public abstract void Restore();
    }
}