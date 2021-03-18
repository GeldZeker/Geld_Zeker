using BWolf.Utilities;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    public class CellPhoneDisplaySystem : MonoBehaviour
    {
        [SerializeField]
        private float toggleTime = 0.125f;

        [SerializeField]
        private CellPhoneDisplay[] displays = null;

        private Dictionary<CellPhoneScreen, CellPhoneDisplay> displaypairs = new Dictionary<CellPhoneScreen, CellPhoneDisplay>();

        private CellPhoneScreen phoneScreenOpen;

        private RectTransform rectTransform;
        private bool isToggling;
        private bool hasFocus;

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
            if (hasFocus && (RectTransformExtensions.PressOutsideTransform(rectTransform) || RectTransformExtensions.PressOutsideTransform(rectTransform)))
            {
                ToggleCellPhone();
            }
        }

        /// <summary>Toggles the display state of the cellphone if only one touch is recorded</summary>
        public void OnCellPhoneButtonClick()
        {
            if (Application.platform == RuntimePlatform.Android && Input.touchCount != 1)
            {
                return;
            }

            ToggleCellPhone();
        }

        /// <summary>Toggles the display state of the cellphone</summary>
        public void ToggleCellPhone()
        {
            if (!isToggling)
            {
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
        }

        public void GoBack()
        {
            if (phoneScreenOpen != CellPhoneScreen.Home && !displaypairs[phoneScreenOpen].GoBack())
            {
                OpenScreen(CellPhoneScreen.Home);
            }
        }

        /// <summary>Returns an enumerator that either shows or hides the cellphone based on its current state</summary>
        private IEnumerator ToggleEnumerator()
        {
            isToggling = true;

            Vector3 position = rectTransform.anchoredPosition;
            float newYPosition = hasFocus ? -rectTransform.sizeDelta.y : 0;
            LerpValue<float> moveYAxis = new LerpValue<float>(position.y, newYPosition, toggleTime);

            hasFocus = !hasFocus;

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
            if (hasFocus && !isToggling)
            {
                displaypairs[phoneScreenOpen].SetActive(false);

                CellPhoneDisplay newDisplay = displaypairs[phoneScreen];
                newDisplay.SetActive(true);

                phoneScreenOpen = phoneScreen;
            }
        }
    }
}