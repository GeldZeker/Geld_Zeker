using GameStudio.GeldZeker.MiniGames.Settings;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Text;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>A behaviour representing a keyboard on a Pin Device</summary>
    public class PinKeyboard : MonoBehaviour
    {
        private PinKeyboardKey[] keys;

        private List<string> input = new List<string>(LoginCodeSetting.PIN_LENGTH);

        private bool canContinue;
        public bool IsActive { get; private set; }
        private WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

        private void Awake()
        {
            keys = GetComponentsInChildren<PinKeyboardKey>();

            foreach (PinKeyboardKey key in keys)
            {
                key.OnClick += OnKeyClicked;
            }
        }

        private void OnDestroy()
        {
            foreach (PinKeyboardKey key in keys)
            {
                key.OnClick -= OnKeyClicked;
            }
        }

        /// <summary>Returns a routine that waits for pincode input and updates given screen with key board input</summary>
        public IEnumerator WaitForPinRoutine()
        {
            /*IsActive = true;

            StringBuilder builder = new StringBuilder("Pin ");
            int startLength = builder.Length;
            while (!canContinue)
            {
                foreach (string pin in input)
                {
                    builder.Append('*');
                }
                screen.SetText(builder.ToString());
                builder.Remove(startLength, builder.Length - startLength);
                yield return waitFixed;
            }

            IsActive = false;
            canContinue = false;
            screen.SetText(string.Empty);*/
            yield return waitFixed;
        }

        private void OnKeyClicked(string key)
        {
            if (!IsActive)
            {
                return;
            }

            switch (key)
            {
                case "back":
                    Trim();
                    break;

                case "stop":
                    break;

                case "continue":
                    canContinue = CheckForContinue();
                    break;

                default:
                    AddKey(key);
                    break;
            }
        }

        /// <summary>checks the current pincode input with helper pincode and returns whether it is equal</summary>
        private bool CheckForContinue()
        {
            string[] pincode = PinHelper.Pin;

            if (pincode.Length != input.Count)
            {
                return false;
            }

            for (int i = 0; i < input.Count; i++)
            {
                if (pincode[i] != input[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>Trims the last input from the current input pincode if possible</summary>
        private void Trim()
        {
            if (input.Count != 0)
            {
                input.RemoveAt(input.Count - 1);
            }
        }

        /// <summary>Tries adding a key to the pincode input</summary>
        private void AddKey(string key)
        {
            if (input.Count != LoginCodeSetting.PIN_LENGTH)
            {
                input.Add(key);
            }
        }
    }
}