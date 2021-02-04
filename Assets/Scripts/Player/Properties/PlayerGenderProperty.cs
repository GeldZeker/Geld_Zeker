using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>A property describing the gender of the character being played</summary>
    [CreateAssetMenu(menuName = "Player/Properties/Gender")]
    public class PlayerGenderProperty : PlayerProperty
    {
        [SerializeField]
        private PlayerGender gender = PlayerGender.NotSelected;

        public PlayerGender Value
        {
            get { return gender; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateGender(PlayerGender newGender, bool fromSaveFile = false)
        {
            if (gender == newGender)
            {
                return;
            }

            gender = newGender;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateGender(PlayerGender.NotSelected);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerGender)}/{name}";
            GameFileSystem.SaveToFile(path, gender);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(PlayerGender)}/{name}";

            if (GameFileSystem.LoadFromFile(path, out PlayerGender outValue))
            {
                UpdateGender(outValue, true);
            }
        }
    }

    public enum PlayerGender
    {
        NotSelected,
        Male,
        Female
    }
}