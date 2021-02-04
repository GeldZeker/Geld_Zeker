using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Utilities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Progression
{
    /// <summary>Behaviour class for progressing the questline as quests are completed</summary>
    public class QuestProgressor : MonoBehaviour
    {
        [Header("Project References")]
        [SerializeField]
        private Quest startQuest = null;

        [SerializeField]
        private Introduction introToStartFrom = null;

        private List<Quest>[] quests;

        private void Start()
        {
            quests = new List<Quest>[Enum.GetNames(typeof(ProgressionLevel)).Length];
            for (int i = 0; i < quests.Length; i++)
            {
                quests[i] = new List<Quest>();
            }

            QuestManager.Instance.QuestCompleted += OnQuestCompletion;
            Quest[] allQuests = QuestManager.Instance.Quests;
            for (int i = 0; i < allQuests.Length; i++)
            {
                //add quest to a quest list based on the level it belongs to
                Quest quest = allQuests[i];
                quests[(int)quest.Level].Add(quest);

                foreach (QuestTask task in quest.Tasks)
                {
                    task.Completed += OnTaskCompleted;
                }
            }

            introToStartFrom.OnFinish += OnIntroFinish;
        }

        private void OnDestroy()
        {
            introToStartFrom.OnFinish -= OnIntroFinish;
        }

        /// <summary>Sets the starting quest to be active</summary>
        public void OnIntroFinish(Introduction introduction)
        {
            if (!startQuest.IsActive && !startQuest.IsCompleted)
            {
                startQuest.SetActive(true);
            }
        }

        private void OnTaskCompleted(QuestTask task)
        {
            NotificationUtility.Instance.Notify($"Een deel van een doel is af: {task.TaskDescription}", NotificationStayTime.Long, QuestManager.QuestIcon);
        }

        /// <summary>Returns a list of quests that belong to the given progression level</summary>
        public List<Quest> GetQuestsOfLevel(ProgressionLevel level)
        {
            return quests[(int)level];
        }

        /// <summary>called when a quest has been completed to activate quests that had the completed quest as a requirement</summary>
        private void OnQuestCompletion(Quest completedQuest)
        {
            foreach (Quest quest in QuestManager.Instance.Quests)
            {
                if (quest.RequiredQuest == completedQuest)
                {
                    quest.SetActive(true);
                }
            }
        }
    }
}