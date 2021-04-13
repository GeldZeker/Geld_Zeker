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

        /// <summary>Adds the event listener to the Key when the objects are initialized.</summary>
        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClicked);
        }

        /// <summary>Removes the OnClick Listener when the Keys are destroyed.</summary>
        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClicked);
        }

        /// <summary>OnClick event for the input key on the Pin keypad.</summary>
        public void OnClicked()
        {
            MusicPlayer.Instance.PlaySFXSound(SFXSound.DefaultButtonClick);
            OnClick(key);
        }
    }
}