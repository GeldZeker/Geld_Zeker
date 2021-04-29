using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


namespace Assets.Scripts.Player.Properties
{
    [CreateAssetMenu(menuName = "Player/Properties/Reward")]
    public class PlayerRewardProperty : PlayerProperty
    {
        [SerializeField]
        private List<Tuple<string, RewardType>> rewardCollection = null;

        public void PrintRewardCollection()
        {
            string result = "--------------- Begin RewardCollection ---------------" + "\n ";
            foreach (Tuple<string, RewardType> T in rewardCollection)
            {
                result += T.Item1 + ": "; 
                result += T.Item2 + "\n";
            }
            result += "--------------- End RewardCollection ---------------";
            Debug.Log(result);

        }

        public void AddRewardThroughDifficulty(string name, MiniGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Bronze));
                    Debug.Log($"Added {RewardType.Bronze} reward for minigame {name}");
                    break;
                case MiniGameDifficulty.Medium:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Silver));
                    Debug.Log($"Added {RewardType.Silver} reward for minigame {name}");
                    break;
                case MiniGameDifficulty.Hard:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Gold));
                    Debug.Log($"Added {RewardType.Gold} reward for minigame {name}");
                    break;
            }
            SaveToFile();
        }

        public void AddReward(string name, RewardType type)
        {
            rewardCollection.Add(new Tuple<string, RewardType>(name, type));
            Debug.Log($"Added {type} reward for normal gamemode {name}");
            SaveToFile();
        }

        public bool HasReward(string name)
        {
            foreach (Tuple<string, RewardType> T in rewardCollection)
            {
                if(T.Item1 == name) return true;
                
            }
            return false;
        }

        public RewardType GetHighestReward(string name)
        {
            int result = 0;

            foreach (Tuple<string, RewardType> T in rewardCollection)
            {
                if (T.Item1 == name) {
                    switch (T.Item2)
                    {
                        case RewardType.Bronze:
                            if (result < 1) result = 1;
                            break;
                        case RewardType.Silver:
                            if (result < 2) result = 2;
                            break;
                        case RewardType.Gold:
                            if (result < 3) result = 3;
                            break;
                    }
                }
            }

            switch (result)
            {
                case 1:
                    return RewardType.Bronze;
                case 2:
                    return RewardType.Silver;
                case 3:
                    return RewardType.Gold;
                default:
                    return RewardType.None;
            }
        }


        //abstract classes for propertyManager below
        //======================================================================================
        public void UpdateReward(List<Tuple<string, RewardType>> rewardList, bool fromSaveFile = false)
        {
            this.rewardCollection = rewardList;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name}";
            if (GameFileSystem.LoadFromFile(path, out List<Tuple<string, RewardType>> outValue))
            {
                UpdateReward(outValue, true);
            }
        }

        public override void Restore()
        {
            rewardCollection = new List<Tuple<string, RewardType>>();
            SaveToFile();
        }

        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name}";
            GameFileSystem.SaveToFile(path, rewardCollection);
        }
    }
}

public enum RewardType
{
    None,
    Bronze,
    Silver,
    Gold
}