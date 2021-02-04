using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>A utility component for toggling the interactability of a selectable ui element like a button</summary>
    [RequireComponent(typeof(Selectable))]
    public class InteractableOnEvent : MonoBehaviour
    {
        [SerializeField]
        private bool isInteractablebyDefault = false;

        private Selectable selectable;

        private void Awake()
        {
            selectable = GetComponent<Selectable>();
            selectable.interactable = isInteractablebyDefault;
        }

        public void OnEvent()
        {
            selectable.interactable = !selectable.interactable;
        }
    }
}