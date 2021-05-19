using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Properties
{
    [CreateAssetMenu(menuName = "Player/Properties/Volunteer")]
    class PlayerVolunteerProperty : PlayerProperty
    {
        [SerializeField]
        private int timesWorked;

        [SerializeField]
        private VolunteerType nameOfWork;

        [SerializeField]
        private int PaymentInterval;


        public void PlusWorkedShift()
        {
            timesWorked++;
        }

        public bool TimeForPayment()
        {
            if (timesWorked % PaymentInterval == 0)
                return true;
            return false;
        }

        public void SetVolunteerName(VolunteerType type)
        {
            nameOfWork = type;
        }

        public VolunteerType GetVolunteerName()
        {
            return nameOfWork;
        }


        //abstract class constructors for propertyManager below
        //======================================================================================

        //Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file
        public void UpdateReward(int tWorked, VolunteerType nWork, bool fromSaveFile = false)
        {
            this.timesWorked = tWorked;
            this.nameOfWork = nWork;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        //Loads value from local storage
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name}";
            string path2 = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name + "nameOfWork"}";

            if (GameFileSystem.LoadFromFile(path, out int outValue) &&
                GameFileSystem.LoadFromFile(path2, out VolunteerType outValue2))
            {
                UpdateReward(outValue, outValue2, true);
            }
        }

        //Resets the value of this property to an empty List
        public override void Restore()
        {
            timesWorked = 0;
            nameOfWork = VolunteerType.None;
            SaveToFile();
        }

        //Saves value to local storage
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name}";
            GameFileSystem.SaveToFile(path, timesWorked);

            string path2 = $"{FOLDER_NAME}/{nameof(PlayerRewardProperty)}/{name + "nameOfWork"}";
            GameFileSystem.SaveToFile(path2, nameOfWork);
        }
    }
}

public enum VolunteerType
{
    None,
    Sport,
    Coffee,
    Nature
}