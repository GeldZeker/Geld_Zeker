﻿using BWolf.Utilities;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.UI.CellPhone.DigiDLoginSystem;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    public class CellPhoneDisplaySystem : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float toggleTime = 0.125f;

        [SerializeField]
        private CellPhoneDisplay[] displays = null;

        private Dictionary<CellPhoneScreen, CellPhoneDisplay> displaypairs = new Dictionary<CellPhoneScreen, CellPhoneDisplay>();

        private CellPhoneScreen phoneScreenOpen;

        private RectTransform rectTransform;
        private bool isToggling;
        private bool hasFocus;
        private bool digidIntroActive = false;

        [Header("DigiD App")]
        [SerializeField]
        private DigiDProperty hasDigiDAccountProperty = null;

        [SerializeField]
        private GameObject pinHelper = null;

        [SerializeField]
        private PinKeyboard keyboard = null;

        [SerializeField]
        private GameObject digidAppObject = null;

        [SerializeField]
        private Introduction DigiDIntro = null;

        [SerializeField]
        private Quest digidBekijkenQuest = null;

        [SerializeField]
        private Quest payInvoiceQuest = null;

        [SerializeField]
        private GameObject payInvoiceButton = null;

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            foreach (CellPhoneDisplay display in displays)
            {
                displaypairs.Add(display.PhoneScreen, display);
            }

            OpenScreen(CellPhoneScreen.Home);
        }

        private void Update()
        {
            if (DigiDIntro.Finished) digidIntroActive = false;
            if (hasFocus && (RectTransformExtensions.PressOutsideTransform(rectTransform) || RectTransformExtensions.PressOutsideTransform(rectTransform)) && !digidIntroActive)
            {
                ToggleCellPhone();
            }
        }

        /// <summary>Toggles the display state of the cellphone if only one touch is recorded</summary>
        public void OnCellPhoneButtonClick()
        {
            hasDigiDAccountProperty = PlayerPropertyManager.Instance.GetProperty<DigiDProperty>("DigiD");

            if (Application.platform == RuntimePlatform.Android && Input.touchCount != 1)
            {
                return;
            }

            ToggleCellPhone();
        }

        /// <summary>Toggles the display state of the cellphone</summary>
        public void ToggleCellPhone()
        {
            hasDigiDAccountProperty = PlayerPropertyManager.Instance.GetProperty<DigiDProperty>("DigiD");

            if (!isToggling)
            {
                // Set App Icons active to false if not available yet.
                if (!hasDigiDAccountProperty.Value) digidAppObject.SetActive(false);
                else
                {
                    digidAppObject.SetActive(true);
                    payInvoiceButton.SetActive(true);
                    if (!digidBekijkenQuest.IsCompleted && !hasFocus)
                    {
                        digidIntroActive = true;
                        DigiDIntro.Restore();
                        DigiDIntro.Start();
                    }
                    if (digidBekijkenQuest.IsCompleted && payInvoiceQuest.IsActive) payInvoiceButton.SetActive(true);
                    if (payInvoiceQuest.IsCompleted) payInvoiceButton.GetComponent<Button>().interactable = false;
                }

                OpenScreen(CellPhoneScreen.Home);
                StartCoroutine(ToggleEnumerator());
            }
        }

        public void GoHome()
        {
            if (phoneScreenOpen != CellPhoneScreen.Home)
            {
                OpenScreen(CellPhoneScreen.Home);
            }
            ResetPinLoginPin();
        }

        public void GoBack()
        {
            if (phoneScreenOpen != CellPhoneScreen.Home && !displaypairs[phoneScreenOpen].GoBack())
            {
                OpenScreen(CellPhoneScreen.Home);
            }
            ResetPinLoginPin();
        }

        /// <summary>Returns an enumerator that either shows or hides the cellphone based on its current state</summary>
        private IEnumerator ToggleEnumerator()
        {
            isToggling = true;

            Vector3 position = rectTransform.anchoredPosition;
            float newYPosition = hasFocus ? -rectTransform.sizeDelta.y : 0;
            LerpValue<float> moveYAxis = new LerpValue<float>(position.y, newYPosition, toggleTime);

            hasFocus = !hasFocus;

            if (!hasFocus) ResetPinLoginPin();

            while (moveYAxis.Continue())
            {
                Vector2 newPosition = new Vector2(position.x, Mathf.Lerp(moveYAxis.start, moveYAxis.end, moveYAxis.perc));
                rectTransform.anchoredPosition = newPosition;
                yield return null;
            }

            isToggling = false;
        }

        public void OpenScreen(CellPhoneScreen phoneScreen)
        {
            if (hasFocus && !isToggling && !digidIntroActive)
            {
                displaypairs[phoneScreenOpen].SetActive(false);

                CellPhoneDisplay newDisplay = displaypairs[phoneScreen];
                newDisplay.SetActive(true);

                phoneScreenOpen = phoneScreen;
            }
        }
        /// <summary>Function to reset the Pin Login saved data when closing. Causes to fill the Pin again on reopening the DigiD App.</summary>
        public void ResetPinLoginPin()
        {
            if (pinHelper.activeSelf) pinHelper.SetActive(false);
            keyboard.ResetInput();
        }
    }
}