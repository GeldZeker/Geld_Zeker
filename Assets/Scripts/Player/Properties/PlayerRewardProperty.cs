using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;


namespace Assets.Scripts.Player.Properties
{
    [CreateAssetMenu(menuName = "Player/Properties/Reward")]
    public class PlayerRewardProperty : PlayerProperty
    {
        [SerializeField]
        private List<Tuple<string, RewardType>> rewardCollection = null;

        //method used for adding rewards to the rewardCollection list by the difficulty set for a minigame, converted to the enum RewardType before adding to the list
        //easy = bronze
        //medium = silver
        //hard = gold
        public void AddRewardThroughDifficulty(string name, MiniGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Bronze));
                    break;
                case MiniGameDifficulty.Medium:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Silver));
                    break;
                case MiniGameDifficulty.Hard:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Gold));
                    break;
            }
            SaveToFile();
        }

        //method for adding rewards to the rewardCollection List by name and enum rewardType
        public void AddReward(string name, RewardType type)
        {
            rewardCollection.Add(new Tuple<string, RewardType>(name, type));
            SaveToFile();
        }

        //method that returns bool based on the fact that the rewardCollection list has reward(s) with a certain name
        //has reward => return true
        //doesn't have the reward => returns false
        public bool HasReward(string name)
        {
            foreach (Tuple<string, RewardType> T in rewardCollection)
            {
                if(T.Item1 == name) return true;
                
            }
            return false;
        }

        //method to get the highest reward from the rewardCollection based in the name of the reward, reward value is as following:
        // 1. (highest) Gold
        // 2. Silver
        // 3. Bronze
        // 4. None
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






        //abstract class constructors for propertyManager below
        //======================================================================================

        //Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file
        public void UpdateReward(List<Tuple<string, RewardType>> rewardList, bool fromSaveFile = false)
        {
            this.rewardCollection = rewardList;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        //Loads value from local storage
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name}";
            if (GameFileSystem.LoadFromFile(path, out List<Tuple<string, RewardType>> outValue))
            {
                UpdateReward(outValue, true);
            }
        }
        
        //Resets the value of this property to an empty List
        public override void Restore()
        {
            rewardCollection = new List<Tuple<string, RewardType>>();
            SaveToFile();
        }

        //Saves value to local storage
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