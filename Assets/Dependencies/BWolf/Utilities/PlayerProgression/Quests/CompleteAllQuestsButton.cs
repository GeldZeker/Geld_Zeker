using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.UI
{
    /// <summary>An AudableButton script that represents completing all tasks and quests ingame.</summary>
    /// <summary>Use with CAUTION!!! Game is not in representable state after skipping tasks. Certain elements can not be loaded!</summary>
    public class CompleteAllQuestsButton : AudableButton
    {
        [Header("WARNING: Use with caution!")]
        [Header("Game can not be loading in certain elements!")]
        [SerializeField]
        private QuestAsset questAsset = null;

        // Initialized in the Unity Editor to skip to certain tasks.
        [SerializeField]
        private string stopAtTask = "";

        /// <summary>Adds listener to button on Awake.</summary>
        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(CompleteQuests);
        }

        /// <summary>Removes listener from button OnDestroy.</summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(CompleteQuests);
        }

        /// <summary>Method that cycles through quests and calls DevComplete(str).</summary>
        private void CompleteQuests()
        {
            Quest[] quests = questAsset.Quests;

            for (int i = 0; i < quests.Length; i++)
            {
                Quest quest = quests[i];
                quest.DevComplete(stopAtTask);
            }
        }
    }
}