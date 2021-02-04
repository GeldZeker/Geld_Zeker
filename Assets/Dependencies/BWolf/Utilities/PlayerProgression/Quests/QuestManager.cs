using BWolf.Behaviours.SingletonBehaviours;
using GameStudio.GeldZeker.Player.GameNotifications;
using GameStudio.GeldZeker.Player.Progression;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace BWolf.Utilities.PlayerProgression.Quests
{
    /// <summary>Singleton Manager class providing options to manage all stored quests in the game</summary>
    public class QuestManager : SingletonBehaviour<QuestManager>
    {
        [Header("Settings")]
        [SerializeField]
        private string questCompletedMessage = "Een doel is af";

        [Header("References")]
        [SerializeField]
        private QuestAsset questAsset = null;

        [SerializeField]
        private Sprite questIcon = null;

        public List<Quest> ActiveQuests { get; } = new List<Quest>();

        public Quest[] Quests
        {
            get { return questAsset.Quests; }
        }

        public static Sprite QuestIcon { get; private set; }

        public event Action<Quest> QuestCompleted;

        private QuestProgressor progressor;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            QuestIcon = questIcon;
            progressor = GetComponent<QuestProgressor>();
        }

        private void Start()
        {
            foreach (Quest quest in questAsset.Quests)
            {
                quest.LoadActiveStateFromFile();
                quest.LoadTasksFromFile();

                quest.ActiveStateChanged += OnActiveStateChanged;
                quest.Completed += OnQuestCompleted;
            }

#if UNITY_EDITOR
            //quest state is saved in editor so add active quests manually if in editor
            foreach (Quest quest in questAsset.Quests)
            {
                if (quest.IsActive && !ActiveQuests.Contains(quest))
                {
                    ActiveQuests.Add(quest);
                }
            }
#endif
        }

        protected override void OnDestroy()
        {
            base.Awake();

            foreach (Quest quest in questAsset.Quests)
            {
                quest.ActiveStateChanged -= OnActiveStateChanged;
                quest.Completed -= OnQuestCompleted;
            }
        }

        private void Update()
        {
            for (int i = 0; i < ActiveQuests.Count; i++)
            {
                ActiveQuests[i].Update();
            }
        }

        public List<Quest> GetQuestsOfLevel(ProgressionLevel level)
        {
            return progressor.GetQuestsOfLevel(level);
        }

        public Quest GetQuest(string nameOfQuest)
        {
            return questAsset.GetQuest(nameOfQuest);
        }

        [ContextMenu("ResetProgress")]
        public void ResetProgress()
        {
            questAsset.Restore();
        }

        /// <summary>Called when a quest's active state has changed to add it to or remove it from the activeQuests list</summary>
        private void OnActiveStateChanged(Quest quest, bool value)
        {
            if (value)
            {
                ActiveQuests.Add(quest);
                MainCanvasManager.Instance.AddNotification(GameNotificationType.Quest);
            }
            else
            {
                ActiveQuests.Remove(quest);
            }
        }

        /// <summary>Called when a quest has been completed to notify listeners of this event</summary>
        private void OnQuestCompleted(Quest quest)
        {
            NotificationUtility.Instance.Notify($"{questCompletedMessage}: {quest.Description}", NotificationStayTime.Average, questIcon);

            QuestCompleted?.Invoke(quest);
        }
    }
}