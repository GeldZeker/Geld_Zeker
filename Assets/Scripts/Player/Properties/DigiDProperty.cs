using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>Stores all the DigiD items needed as a player property</summary>
    [CreateAssetMenu(menuName = "Player/Properties/DigiD")]
    public class DigiDProperty : PlayerProperty
    {
        [SerializeField]
        private bool accountValue = false;

        public bool Value
        {
            get { return accountValue; }
        }

        /// <summary>Updates the account value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateAccountValue(bool newBooleanValue, bool fromSaveFile = false)
        {
            if (accountValue == newBooleanValue)
            {
                return;
            }

            accountValue = newBooleanValue;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateAccountValue(false);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(DigiDProperty)}/{name}";
            GameFileSystem.SaveToFile(path, accountValue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(DigiDProperty)}/{name}";

            if (GameFileSystem.LoadFromFile(path, out bool outValue))
            {
                UpdateAccountValue(outValue, true);
            }
        }
    }
}