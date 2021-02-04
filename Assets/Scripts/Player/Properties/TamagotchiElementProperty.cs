using System;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>An int value respresenting the player's TamagotchiElement from 0-100% </summary>
    [CreateAssetMenu(menuName = "Player/Properties/TamagotchiElement")]
    public class TamagotchiElementProperty : PlayerProperty
    {
        [SerializeField]
        [Range(MIN_PERCENTAGE, MAX_PERCENTAGE)]
        private int percentage = MAX_PERCENTAGE;

        [SerializeField, Tooltip("This value is subtracted from the Tamagotchi Element every 5s")]
        private int depletion = 1;

        public int Percentage
        {
            get
            {
                return percentage;
            }
        }

        public int Depletion
        {
            get
            {
                return depletion;
            }
        }

        private const int MAX_PERCENTAGE = 100;
        private const int MIN_PERCENTAGE = 0;

        public event Action<int> OnPercentageChange;

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(int newPercentageValue, bool fromSaveFile = false)
        {
            if (percentage == newPercentageValue)
            {
                return;
            }
            if (newPercentageValue > MAX_PERCENTAGE)
            {
                newPercentageValue = MAX_PERCENTAGE;
            }
            if (newPercentageValue < MIN_PERCENTAGE)
            {
                newPercentageValue = MIN_PERCENTAGE;
            }
            percentage = newPercentageValue;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
            OnPercentageChange?.Invoke(percentage);
        }

        /// <summary>Adds the value to the TamagotchiElement</summary>
        public void AddValue(int amount)
        {
            UpdateValue(percentage + amount);
        }

        public void RemoveValue(int amount)
        {
            UpdateValue(percentage - amount);
        }

        public void Deplete()
        {
            UpdateValue(percentage - depletion);
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
#if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                OnPercentageChange = null;
#endif
            UpdateValue(MAX_PERCENTAGE / 2);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(TamagotchiElementProperty)}/{name}";
            GameFileSystem.SaveToFile(path, percentage);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(TamagotchiElementProperty)}/{name}";

            if (GameFileSystem.LoadFromFile(path, out int outValue))
            {
                UpdateValue(outValue, true);
            }
        }
    }
}