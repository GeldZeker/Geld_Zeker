using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace GameStudio.GeldZeker.Player
{
    /// <summary>Editor class for restoring progression assets before starting to build</summary>
    public class ProgressionBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder
        {
            get { return 0; }
        }

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerProperties playerPropertiesAsset = Resources.Load<PlayerProperties>(PlayerProperties.ASSET_NAME);
            if (playerPropertiesAsset.RestoreOnBuild)
            {
                playerPropertiesAsset.Restore();
            }

            QuestAsset questAsset = Resources.Load<QuestAsset>(QuestAsset.ASSET_NAME);
            if (questAsset.RestoreOnBuild)
            {
                questAsset.Restore();
            }

            IntroductionAsset introAsset = Resources.Load<IntroductionAsset>(IntroductionAsset.ASSET_NAME);
            if (introAsset.RestoreOnBuild)
            {
                introAsset.Restore();
            }

            MiniGameSettingAsset minigameAsset = Resources.Load<MiniGameSettingAsset>(MiniGameSettingAsset.ASSET_NAME);
            if (minigameAsset.RestoreOnBuild)
            {
                minigameAsset.Restore();
            }

            Debug.Log($"Tried restoring scriptable object assets");
        }
    }
}