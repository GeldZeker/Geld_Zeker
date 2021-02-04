using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    /// <summary>Behaviour for displaying a cell phone screen</summary>
    [RequireComponent(typeof(CanvasGroup), typeof(Image))]
    public class CellPhoneDisplay : MonoBehaviour
    {
        [Header("Display Settings")]
        [SerializeField]
        private bool activeOnAwake = false;

        [SerializeField]
        private CellPhoneScreen phoneScreen = CellPhoneScreen.Home;

        protected Image display;
        protected Sprite startSprite;

        private CanvasGroup group;

        protected virtual void Awake()
        {
            display = GetComponent<Image>();
            group = GetComponent<CanvasGroup>();

            startSprite = display.sprite;

            if (activeOnAwake)
            {
                SetActive(true);
            }
        }

        /// <summary>The type of phone screen this display is showing</summary>
        public CellPhoneScreen PhoneScreen
        {
            get { return phoneScreen; }
        }

        /// <summary>Sets the active state of this display</summary>
        public virtual void SetActive(bool value)
        {
            group.alpha = value ? 1.0f : 0.0f;
            group.interactable = value;
            group.blocksRaycasts = value;
        }

        /// <summary>Tries going back to the previous screen, returns if it did succesfully</summary>
        public virtual bool GoBack()
        {
            return false;
        }
    }
}