using BWolf.Utilities;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.Navigation
{
    /// <summary>Behaviour for controlling the displaying of a child scene navigation</summary>
    public class ChildSceneDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float toggleTime = 0.125f;

        [Header("References")]
        [SerializeField]
        private Image parentSceneIcon = null;

        [SerializeField]
        private CanvasGroup group = null;

        [Space]
        [SerializeField]
        private ChildSceneNavButton[] navButtons = null;

        private const float EXPAND_PER_BUTTON = 160.0f;

        private RectTransform rectTransform;

        private float defaultWidth;
        private float targetWidth;

        private bool isToggling;
        private bool hasFocus;

        /// <summary>Returns the active nav button count</summary>
        private int activeNavButtonCount
        {
            get
            {
                int count = 0;
                for (int i = 0; i < navButtons.Length; i++)
                {
                    if (navButtons[i].active)
                    {
                        count++;
                    }
                }

                return count;
            }
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();

            defaultWidth = ((RectTransform)transform).sizeDelta.x;
        }

        private void Start()
        {
            for (int i = 0; i < navButtons.Length; i++)
            {
                ChildSceneNavButton navButton = navButtons[i];
                navButton.visible = false;
                navButton.AddOnClickListener(ToggleDisplay);
            }
        }

        private void Update()
        {
            if (hasFocus && (RectTransformExtensions.PressOutsideTransform(rectTransform) || RectTransformExtensions.PressOutsideTransform(rectTransform)))
            {
                //if the navigation has focus and there is a touch outside the navigation, close it
                ToggleDisplay();
            }
        }

        /// <summary>Sets the active state of the display by adjusting the canvas group values</summary>
        public void SetActive(bool value)
        {
            group.alpha = value ? 1.0f : 0.0f;
            group.interactable = value;
            group.blocksRaycasts = value;
        }

        /// <summary>Populates the display with parent scene information</summary>
        public void Populate(string sceneName, ParentScene parentScene)
        {
            if (parentScene.ContainsChild(sceneName))
            {
                PopulateUsingChild(sceneName, parentScene);
            }
            else
            {
                PopulateUsingParent(parentScene);
            }
        }

        /// <summary>Populates the child scene display based on a child scene being entered</summary>
        private void PopulateUsingChild(string nameOfChildScene, ParentScene parentScene)
        {
            parentSceneIcon.sprite = parentScene.GetChild(nameOfChildScene).icon;

            for (int i = 0; i < navButtons.Length; i++)
            {
                navButtons[i].active = false;
            }

            PopulateNavButton(navButtons[0], new ChildScene { name = parentScene.name, icon = parentScene.icon });

            List<ChildScene> childScenes = parentScene.ChildrenWithout(nameOfChildScene);
            for (int i = 0, j = 1; i < childScenes.Count; i++, j++)
            {
                PopulateNavButton(navButtons[j], childScenes[i]);
            }
        }

        /// <summary>Populates the child scene display based on a parent scene being entered</summary>
        private void PopulateUsingParent(ParentScene parentScene)
        {
            parentSceneIcon.sprite = parentScene.icon;

            ChildScene[] childScenes = parentScene.childScenes;
            for (int i = 0; i < navButtons.Length; i++)
            {
                navButtons[i].active = false;
            }

            for (int i = 0; i < childScenes.Length; i++)
            {
                ChildSceneNavButton navbutton = navButtons[i];
                ChildScene childScene = childScenes[i];

                navbutton.active = true;
                navbutton.interactable = true;
                navbutton.nameOfSceneLoading = childScene.name;
                navbutton.icon = childScene.icon;
            }
        }

        private void PopulateNavButton(ChildSceneNavButton navbutton, ChildScene childScene)
        {
            navbutton.active = true;
            navbutton.interactable = true;
            navbutton.nameOfSceneLoading = childScene.name;
            navbutton.icon = childScene.icon;
        }

        /// <summary>Toggles the current display state</summary>
        public void ToggleDisplay()
        {
            if (!isToggling)
            {
                hasFocus = !hasFocus;

                int buttonCount = activeNavButtonCount;
                StartCoroutine(ToggleVisabilityRoutine(buttonCount));
                StartCoroutine(ToggleRoutine(buttonCount));
            }
        }

        /// <summary>Returns an enumerator that toggles the visability of the nav buttons</summary>
        private IEnumerator ToggleVisabilityRoutine(int buttonCount)
        {
            float secondWait = hasFocus ? (toggleTime / (navButtons.Length + 1)) : (1 / 60);
            for (int i = 0; i < buttonCount; i++)
            {
                yield return new WaitForSeconds(secondWait);
                navButtons[i].visible = hasFocus;
            }
        }

        /// <summary>Returns an enumerator that toggles the size of the display</summary>
        private IEnumerator ToggleRoutine(int buttonCount)
        {
            isToggling = true;
            targetWidth = defaultWidth + (EXPAND_PER_BUTTON * buttonCount);

            float startWidth = hasFocus ? defaultWidth : targetWidth;
            float endWidth = hasFocus ? targetWidth : defaultWidth;
            LerpValue<float> changeSize = new LerpValue<float>(startWidth, endWidth, toggleTime);

            while (changeSize.Continue())
            {
                Vector2 sizeDelta = new Vector2(Mathf.Lerp(changeSize.start, changeSize.end, changeSize.perc), rectTransform.sizeDelta.y);
                rectTransform.sizeDelta = sizeDelta;
                yield return null;
            }

            isToggling = false;
        }

        [System.Serializable]
        private struct ChildSceneNavButton
        {
#pragma warning disable 0649

            [SerializeField]
            private LoadSceneButton button;

            [SerializeField]
            private CanvasGroup group;

            [SerializeField]
            public Image image;

#pragma warning restore 0649

            public bool active
            {
                get { return button.gameObject.activeInHierarchy; }
                set { button.gameObject.SetActive(value); }
            }

            public bool visible
            {
                set { group.alpha = value ? 1.0f : 0.0f; }
            }

            public bool interactable
            {
                set { button.SetInteractable(value); }
            }

            public string nameOfSceneLoading
            {
                set { button.NameOfSceneLoading = value; }
            }

            public Sprite icon
            {
                set { image.sprite = value; }
            }

            public void AddOnClickListener(Action action)
            {
                button.Clicked += action;
            }
        }
    }
}