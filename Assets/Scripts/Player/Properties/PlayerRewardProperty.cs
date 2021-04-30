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

        // ================================== section for debugging =================================
       
        // bool to switch debug logging on and off, true = print log, false = no log
        private bool printDebug = true;

        //method to log desired values for testing purposes, turned on and off with the bool "printDebug" declared at the top of this class
        private void PrintDebugLogs(string printThis)
        {
            if (printDebug) Debug.Log(printThis);
        }

        //method to print all rewards into the debug console
        public void PrintRewardCollection()
        {
            //stops the mothod from printing if debugging is not enabled
            if (!printDebug) return;

            string result = "--------------- Begin RewardCollection ---------------" + "\n ";
            foreach (Tuple<string, RewardType> T in rewardCollection)
            {
                result += T.Item1 + ": "; 
                result += T.Item2 + "\n";
            }
            result += "--------------- End RewardCollection ---------------";
            Debug.Log(result);
        }
        //====================================== end of debugging section ==========================


        
        public void AddRewardThroughDifficulty(string name, MiniGameDifficulty difficulty)
        {
            switch (difficulty)
            {
                case MiniGameDifficulty.Easy:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Bronze));
                    PrintDebugLogs($"Added {RewardType.Bronze} reward for minigame {name}");
                    break;
                case MiniGameDifficulty.Medium:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Silver));
                    PrintDebugLogs($"Added {RewardType.Silver} reward for minigame {name}");
                    break;
                case MiniGameDifficulty.Hard:
                    rewardCollection.Add(new Tuple<string, RewardType>(name, RewardType.Gold));
                    PrintDebugLogs($"Added {RewardType.Gold} reward for minigame {name}");
                    break;
            }
            SaveToFile();
        }

        public void AddReward(string name, RewardType type)
        {
            rewardCollection.Add(new Tuple<string, RewardType>(name, type));
            PrintDebugLogs($"Added {type} reward for normal gamemode {name}");
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


        //abstract class constructors for propertyManager below
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