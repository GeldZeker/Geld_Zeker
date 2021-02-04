using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>A simple button behaviour for managing the active state of a gameobject</summary>
    public class ToggleActiveStateButton : AudableButton
    {
        [Header("Toggle Settings")]
        [SerializeField]
        private GameObject toggleObject = null;

        [SerializeField]
        private bool useCanvasGroup = false;

        private CanvasGroup group = null;

        protected bool groupToggled;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(SetActiveState);

            if (useCanvasGroup)
            {
                group = toggleObject.GetComponent<CanvasGroup>();
                if (group == null)
                {
                    Debug.LogError($"tried attaching active state toggle to {toggleObject} its canvas group but it didn't exist");
                }
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(SetActiveState);
        }

        /// <summary>Called when the button has been clicked to toggle the active state of the referenced game object</summary>
        private void SetActiveState()
        {
            if (!SceneTransitioning.SceneTransitionSystem.Instance.IsTransitioning)
            {
                if (useCanvasGroup)
                {
                    ToggleCanvasGroup();
                }
                else
                {
                    toggleObject.SetActive(!toggleObject.activeInHierarchy);
                }
            }
        }

        private void ToggleCanvasGroup()
        {
            groupToggled = group.alpha != 0;
            groupToggled = !groupToggled;

            group.alpha = groupToggled ? 1.0f : 0.0f;
            group.interactable = groupToggled;
            group.blocksRaycasts = groupToggled;
        }
    }
}