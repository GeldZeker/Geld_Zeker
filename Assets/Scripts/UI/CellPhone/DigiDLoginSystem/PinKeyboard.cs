using GameStudio.GeldZeker.MiniGames.Settings;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Text;
using System;

namespace GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem
{
    /// <summary>A behaviour representing a keyboard on a Pin Device</summary>
    public class PinKeyboard : MonoBehaviour
    {
        private PinKeyboardKey[] keys;

        private List<string> input = new List<string>(LoginCodeSetting.PIN_LENGTH);

        private bool canContinue = false;
        public bool IsActive { get; private set; }
        private WaitForFixedUpdate waitFixed = new WaitForFixedUpdate();

        [Header("References")]
        [SerializeField]
        private GameObject Input1 = null;

        [SerializeField]
        private GameObject Input2 = null;

        [SerializeField]
        private GameObject Input3 = null;

        [SerializeField]
        private GameObject Input4 = null;

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

        public IEnumerator WaitForPinRoutine()
        {
            IsActive = true;

            while (!canContinue)
            {
                foreach (string pin in input)
                {
                    AddStarInInput();
                }
                
                yield return waitFixed;
            }

            IsActive = false;
            canContinue = false;
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

                default:
                    AddKey(key);
                    canContinue = CheckForContinue();
                    break;
            }
        }

        private void AddStarInInput()
        {
            switch (input.Count)
            {
                case 1: Input1.GetComponent<UnityEngine.UI.Text>().text = "*";
                    break;
                case 2: Input2.GetComponent<UnityEngine.UI.Text>().text = "*";
                    break;
                case 3: Input3.GetComponent<UnityEngine.UI.Text>().text = "*";
                    break;
                case 4: Input4.GetComponent<UnityEngine.UI.Text>().text = "*";
                    break;
                default:
                    {
                        Input1.GetComponent<UnityEngine.UI.Text>().text = "";
                        Input2.GetComponent<UnityEngine.UI.Text>().text = "";
                        Input3.GetComponent<UnityEngine.UI.Text>().text = "";
                        Input4.GetComponent<UnityEngine.UI.Text>().text = "";
                    }
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
            CellPhoneDigiDAppNavigation.instance.PinLoginCorrect();
            ResetInput();
            return true;
        }

        /// <summary>Trims the last input from the current input pincode if possible</summary>
        private void Trim()
        {
            GameObject element = null;
            if (input.Count != 0)
            {
                input.RemoveAt(input.Count - 1);
            }

            switch (input.Count + 1)
            {
                case 1: element = Input1;
                    break;
                case 2: element = Input2;
                    break;
                case 3: element = Input3;
                    break;
                case 4: element = Input4;
                    break;
                default: break;
            }
            if (element) element.GetComponent<UnityEngine.UI.Text>().text = "";
        }

        /// <summary>Tries adding a key to the pincode input</summary>
        private void AddKey(string key)
        {
            if (input.Count != LoginCodeSetting.PIN_LENGTH)
            {
                input.Add(key);
            }
        }

        public void ResetInput()
        {
            input.Clear();
            Input1.GetComponent<UnityEngine.UI.Text>().text = "";
            Input2.GetComponent<UnityEngine.UI.Text>().text = "";
            Input3.GetComponent<UnityEngine.UI.Text>().text = "";
            Input4.GetComponent<UnityEngine.UI.Text>().text = "";
        }
    }
}