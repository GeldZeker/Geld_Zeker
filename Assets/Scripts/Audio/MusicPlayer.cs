using BWolf.Behaviours.SingletonBehaviours;
using BWolf.Utilities;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.SceneTransitioning;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.Audio
{
    /// <summary>Singleton class for managing audio played in the game</summary>
    public class MusicPlayer : SingletonBehaviour<MusicPlayer>
    {
        [Header("Settings")]
        [SerializeField, Range(0.0f, 1.0f)]
        private float maxSFXVolume = 0.3f;

        [SerializeField, Range(0.0f, 1.0f)]
        private float maxThemeVolume = 0.5f;

        [SerializeField]
        private float themeTransitionFadeInTime = 1.0f;

        [SerializeField, Range(1.0f, 2.0f)]
        private float transitionVolumeIncrease = 1.25f;

        [Header("References")]
        [SerializeField]
        private MusicSettings settings = null;

        [SerializeField]
        private AudioClip backgroundTheme = null;

        [SerializeField]
        private SFXSoundContainer[] sfxSounds = null;

        private Dictionary<SFXSound, SFXSoundContainer> sfxContainers = new Dictionary<SFXSound, SFXSoundContainer>();
        private List<AudioSource> sources = new List<AudioSource>();

        private const string SETTINGS_PATH = "Music/Settings";

        /// <summary>The current percentage SFX volume used ingame</summary>
        public float SFXVolumePercentage
        {
            get { return settings.SFXVolume / maxSFXVolume; }
        }

        /// <summary>The current percentage Theme volume used ingame</summary>
        public float ThemeVolumePercentage
        {
            get
            {
                return settings.ThemeVolume / maxThemeVolume;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            for (int i = 0; i < sfxSounds.Length; i++)
            {
                sfxContainers.Add(sfxSounds[i].Sound, sfxSounds[i]);
            }

            //load music settings in to update volume accordingly
            LoadSettings();

            //start playing background theme
            PlaySound(GetSource(), backgroundTheme, true, settings.ThemeVolume);
        }

        private void Start()
        {
            SceneTransitionSystem.Instance.TransitionStarted += OnTransitionStarted;
            SceneTransitionSystem.Instance.TransitionCompleted += OnTransitionEnded;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (SceneTransitionSystem.Instance != null)
            {
                SceneTransitionSystem.Instance.TransitionStarted -= OnTransitionStarted;
                SceneTransitionSystem.Instance.TransitionCompleted -= OnTransitionEnded;
            }
        }

        /// <summary>Called when a scene transition has ended it fades the theme volume back to normal</summary>
        private void OnTransitionEnded(Scene scene, LoadSceneMode mode)
        {
            StartCoroutine(ChangeSourceVolumeOverTime(GetClipSource(backgroundTheme), settings.ThemeVolume));
        }

        /// <summary>Called when a scene transition starts it increases the theme volume based on transition volume increase </summary>
        private void OnTransitionStarted(string sceneName, LoadSceneMode mode)
        {
            StartCoroutine(ChangeSourceVolumeOverTime(GetClipSource(backgroundTheme), settings.ThemeVolume * transitionVolumeIncrease));
        }

        /// <summary>Plays SFX sound of given type</summary>
        public void PlaySFXSound(SFXSound sound)
        {
            SFXSoundContainer container = sfxContainers[sound];
            PlaySound(GetSource(), container.Clip, false, container.Volume * SFXVolumePercentage);
        }

        /// <summary>Sets the percentage of volume of the SFX sound</summary>
        public void SetSFXVolumePercentage(float perc)
        {
            UpdateSFXVolume(maxSFXVolume * Mathf.Clamp01(perc));
        }

        /// <summary>Sets the percentage of volume of the theme sound</summary>
        public void SetThemeVolumePercentage(float perc)
        {
            UpdateThemeVolume(maxThemeVolume * Mathf.Clamp01(perc));
        }

        /// <summary>Sets the new SFX volume. Set fromFile to true to make sure the changed is not saved again</summary>
        private void UpdateSFXVolume(float newVolume, bool fromFile = false)
        {
            if (newVolume != settings.SFXVolume)
            {
                settings.SFXVolume = newVolume;

                foreach (SFXSoundContainer container in sfxContainers.Values)
                {
                    container.Volume = newVolume;
                }

                if (!fromFile)
                {
                    SaveSettings();
                }
            }
        }

        /// <summary>Sets the new theme volume. Set fromFile to true to make sure the changed is not saved again</summary>
        private void UpdateThemeVolume(float newVolume, bool fromFile = false)
        {
            if (newVolume != settings.ThemeVolume)
            {
                settings.ThemeVolume = newVolume;
                GetClipSource(backgroundTheme).volume = newVolume;

                if (!fromFile)
                {
                    SaveSettings();
                }
            }
        }

        /// <summary>Saves stored settings to local storage</summary>
        private void SaveSettings()
        {
            GameFileSystem.SaveAsJsonToFile(SETTINGS_PATH, settings);
        }

        /// <summary>Tries loading settings from local storage. Uses max theme and sfx volume as fallback on fail</summary>
        private void LoadSettings()
        {
            if (!GameFileSystem.LoadAsJsonFromFile(SETTINGS_PATH, ref settings))
            {
                settings.ThemeVolume = maxThemeVolume;
                settings.SFXVolume = maxSFXVolume;
                SaveSettings();
            }
        }

        /// <summary>Plays sound using given source, clip and loop and volume settings</summary>
        private void PlaySound(AudioSource source, AudioClip clip, bool loop, float volume)
        {
            source.clip = clip;
            source.loop = loop;
            source.volume = volume;
            source.Play();
        }

        /// <summary>Returns an audio source using pooled non-playing sources before creating a new one</summary>
        private AudioSource GetSource()
        {
            for (int i = 0; i < sources.Count; i++)
            {
                if (!sources[i].isPlaying)
                {
                    return sources[i];
                }
            }

            AudioSource source = gameObject.AddComponent<AudioSource>();
            sources.Add(source);
            return source;
        }

        /// <summary>Returns an enumerator that changes the given source's volume of time towards targetVolume</summary>
        private IEnumerator ChangeSourceVolumeOverTime(AudioSource source, float targetVolume)
        {
            LerpValue<float> change = new LerpValue<float>(source.volume, targetVolume, themeTransitionFadeInTime);
            while (change.Continue())
            {
                source.volume = Mathf.Lerp(change.start, change.end, change.perc);
                yield return null;
            }
        }

        /// <summary>Finds the Audio source this clip is currently attached to. Returns null if none is found</summary>
        private AudioSource GetClipSource(AudioClip clip)
        {
            for (int i = 0; i < sources.Count; i++)
            {
                if (sources[i].clip == clip)
                {
                    return sources[i];
                }
            }

            return null;
        }

        [System.Serializable]
        private class SFXSoundContainer
        {
            public SFXSound Sound = SFXSound.DefaultButtonClick;
            public AudioClip Clip = null;

            [Range(0.0f, 1.0f)]
            public float Volume = 0.15f;
        }
    }

    /// <summary>The SFX sounds in this game</summary>
    public enum SFXSound
    {
        DefaultButtonClick,
        StartButtonClick,
        ConfirmChoice,
        PinTransactionStart,
        PinTransitionEnd,
        TurningPage,
        MinigameCorrect,
        MinigameFailed,
        DoorClosing,
        MoodSad,
        MoodNeutral,
        MoodHappy,
        GroceryGrab,
        Eat
    }
}