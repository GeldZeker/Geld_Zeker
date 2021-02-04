using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>A boolean value based player property</summary>
    [CreateAssetMenu(menuName = "Player/Properties/Boolean")]
    public class BooleanProperty : PlayerProperty
    {
        [SerializeField]
        private bool booleanValue = false;

        public bool Value
        {
            get { return booleanValue; }
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateValue(bool newBooleanValue, bool fromSaveFile = false)
        {
            if (booleanValue == newBooleanValue)
            {
                return;
            }

            booleanValue = newBooleanValue;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            UpdateValue(false);

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanProperty)}/{name}";
            GameFileSystem.SaveToFile(path, booleanValue);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(BooleanProperty)}/{name}";

            if (GameFileSystem.LoadFromFile(path, out bool outValue))
            {
                UpdateValue(outValue, true);
            }
        }
    }
}