using GameStudio.GeldZeker.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.MiniGames.DebitCardPayment
{
    /// <summary>Behaviour representing a keyboard key on the Pin Device</summary>
    [RequireComponent(typeof(Button))]
    public class PinDeviceKeyboardKey : MonoBehaviour
    {
        [SerializeField]
        private string key = string.Empty;

        public event Action<string> OnClick;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClicked);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClicked);
        }

        public void OnClicked()
        {
            MusicPlayer.Instance.PlaySFXSound(SFXSound.DefaultButtonClick);
            OnClick(key);
        }
    }
}