using GameStudio.GeldZeker.Audio;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>A behaviour used for adding audio to a button click</summary>
    [RequireComponent(typeof(Button))]
    public class AudableButton : MonoBehaviour
    {
        [Header("Audio Settings")]
        [SerializeField]
        private SFXSound sound = SFXSound.DefaultButtonClick;

        protected Button button;

        public Button Button
        {
            get { return button; }
        }

        protected virtual void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(PlayAudio);
        }

        protected virtual void OnDestroy()
        {
            button.onClick.RemoveListener(PlayAudio);
        }

        private void PlayAudio()
        {
            MusicPlayer.Instance.PlaySFXSound(sound);
        }
    }
}