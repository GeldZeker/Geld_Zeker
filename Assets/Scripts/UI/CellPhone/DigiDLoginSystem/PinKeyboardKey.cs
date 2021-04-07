using GameStudio.GeldZeker.Audio;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>Behaviour representing a Pin key</summary>
    [RequireComponent(typeof(Button))]
    public class PinKeyboardKey : MonoBehaviour
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