using BWolf.Utilities.CharacterDialogue;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>A behaviour for simulating a Pin Login process using a Pin Code.</summary>
    public class Pin : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private PinKeyboard keyboard = null;

        private void Update()
        {
            if (!PlayerInputSystem.Touched && !PlayerInputSystem.MousePressed) return;
            CheckPin();
        }

        /// <summary>A routine that starts when a Pin key is pressed.</summary>
        private IEnumerator PinRoutine()
        {
            yield return keyboard.WaitForPinRoutine();
        }

        /// <summary>Function that calls a routine when checking the input on a keypress.</summary>
        private void CheckPin()
        {
            if (keyboard.IsActive) { return; }
            StartCoroutine(PinRoutine());
        }
    }
}