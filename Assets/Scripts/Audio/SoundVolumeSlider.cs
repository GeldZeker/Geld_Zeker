using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Audio
{
    /// <summary>Utilitie script for working with a sound slider in audio settings</summary>
    [RequireComponent(typeof(Slider))]
    public class SoundVolumeSlider : MonoBehaviour
    {
        [SerializeField]
        private Sound sound = Sound.SFX;

        private Slider slider;

        private const float VALUE_CHANGE_INTERVAL_WAIT = 0.15f;

        private bool waitingForChangeToStop;

        private void Awake()
        {
            slider = GetComponent<Slider>();
        }

        private void OnEnable()
        {
            slider.onValueChanged.AddListener(OnValueChanged);
        }

        private void Start()
        {
            switch (sound)
            {
                case Sound.Theme:
                    slider.SetValueWithoutNotify(MusicPlayer.Instance.ThemeVolumePercentage);
                    break;

                case Sound.SFX:
                    slider.SetValueWithoutNotify(MusicPlayer.Instance.SFXVolumePercentage);
                    break;
            }
        }

        private void OnDisable()
        {
            slider.onValueChanged.RemoveListener(OnValueChanged);
        }

        private void OnValueChanged(float delta)
        {
            if (!waitingForChangeToStop)
            {
                //if we are not waiting for change to stop, start waiting the slider to stop being changd
                StartCoroutine(WaitForValueChange());
            }
        }

        /// <summary>Returns an enumerator that waits for the slider's value to stop being changed to update the volume afterwards</summary>
        private IEnumerator WaitForValueChange()
        {
            waitingForChangeToStop = true;

            float change = 0.0f;
            while (change != slider.value)
            {
                change = slider.value;
                yield return new WaitForSeconds(VALUE_CHANGE_INTERVAL_WAIT);
            }

            //after the change has stopped, set the new volume percentage based on sound
            switch (sound)
            {
                case Sound.Theme:
                    MusicPlayer.Instance.SetThemeVolumePercentage(slider.value);
                    break;

                case Sound.SFX:
                    MusicPlayer.Instance.SetSFXVolumePercentage(slider.value);
                    MusicPlayer.Instance.PlaySFXSound(SFXSound.DefaultButtonClick);
                    break;
            }

            waitingForChangeToStop = false;
        }

        private enum Sound
        {
            Theme,
            SFX
        }
    }
}