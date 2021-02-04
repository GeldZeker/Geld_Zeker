using UnityEngine;

namespace GameStudio.GeldZeker.Player.Introductions
{
    /// <summary>An introduction used to introduce a scene</summary>
    [CreateAssetMenu(menuName = "Player/Introductions/SceneIntroduction")]
    public class SceneIntroduction : Introduction
    {
        [Header("SceneSettings")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        public string NameOfScene
        {
            get { return nameOfScene; }
        }
    }
}