using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Properties;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Player.Properties
{
    [CreateAssetMenu(menuName = "Player/Properties/Volunteer")]
    public class PlayerVolunteerProperty : PlayerProperty
    {
        [SerializeField]
        public int timesWorked;

        [SerializeField]
        private VolunteerType nameOfWork;

        public void Awake()
        {
            LoadFromFile();
        }

        /// <summary> Add 1 hour to timesWorked and check if player has worked more then 8 times. </summary>
        public void PlusWorkedShift()
        {
            timesWorked++;
            CheckHours(timesWorked);
            SaveToFile();
        }

        /// <summary> Method to check if player has worked more then 8 times. </summary>
        public bool CheckHours(int hoursToCheck)
        {
            if (hoursToCheck >= 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary> Method to set the type of volunteer work player has chosen. </summary>
        public void SetVolunteerName(VolunteerType type)
        {
            nameOfWork = type;
            SaveToFile();
        }

        /// <summary> Method to get the type of volunteer work player has chosen. </summary>
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
            string path = $"{FOLDER_NAME}/{nameof(PlayerVolunteerProperty)}/{name}";
            string path2 = $"{FOLDER_NAME}/{nameof(PlayerVolunteerProperty)}/{name + "nameOfWork"}";

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
            string path = $"{FOLDER_NAME}/{nameof(PlayerVolunteerProperty)}/{name}";
            GameFileSystem.SaveToFile(path, timesWorked);

            string path2 = $"{FOLDER_NAME}/{nameof(PlayerVolunteerProperty)}/{name + "nameOfWork"}";
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