using UnityEngine;

namespace GameStudio.GeldZeker.Audio
{
    /// <summary>ScriptableObject storing music settings used by the player</summary>
    [CreateAssetMenu(menuName = "Player/AudioSettings")]
    public class MusicSettings : ScriptableObject
    {
        [Range(0.0f, 1.0f)]
        public float ThemeVolume = 0.0f;

        [Range(0.0f, 1.0f)]
        public float SFXVolume = 0.0f;
    }
}