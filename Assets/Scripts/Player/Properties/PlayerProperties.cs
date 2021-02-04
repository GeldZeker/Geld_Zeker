using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>Scriptable object serving as container for the player's properties</summary>
    [CreateAssetMenu(fileName = ASSET_NAME, menuName = "Player/Properties/Asset")]
    public class PlayerProperties : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField, Tooltip("Are Quests restored to their original default state when building the application")]
        private bool restoreOnBuild = true;

        [SerializeField]
        private PlayerProperty[] properties = null;

        public const string ASSET_NAME = "PlayerPropertiesAsset";

        /// <summary>Are Quests restored to their original default state when building the application</summary>
        public bool RestoreOnBuild
        {
            get { return restoreOnBuild; }
        }

        public void LoadFromFile()
        {
            for (int i = 0; i < properties.Length; i++)
            {
                properties[i].LoadFromFile();
            }
        }

        public T GetProperty<T>(string name) where T : PlayerProperty
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].name == name)
                {
                    return (T)properties[i];
                }
            }

            return null;
        }

        public PlayerProperty GetProperty(string name)
        {
            for (int i = 0; i < properties.Length; i++)
            {
                if (properties[i].name == name)
                {
                    return properties[i];
                }
            }

            return null;
        }

        public void Restore()
        {
            foreach (PlayerProperty property in properties)
            {
                property.Restore();
            }
        }
    }
}