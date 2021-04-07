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
    /// <summary>A behaviour for simulating a contactless payment process using a Debit Card</summary>
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

        private IEnumerator PinRoutine()
        {
            yield return keyboard.WaitForPinRoutine();
        }

        private void CheckPin()
        {
            if (keyboard.IsActive)
            {
                return;
            }
            StartCoroutine(PinRoutine());
        }
    }
}