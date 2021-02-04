using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    public abstract class PlayerProperty : ScriptableObject
    {
        public const string FOLDER_NAME = "ProgressSaves/PlayerProperties";

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        [ContextMenu("Restore")]
        public abstract void Restore();

        /// <summary>Saves value to local storage</summary>
        [ContextMenu("Save")]
        protected abstract void SaveToFile();

        /// <summary>Loads value from local storage</summary>
        [ContextMenu("Load")]
        public abstract void LoadFromFile();
    }
}