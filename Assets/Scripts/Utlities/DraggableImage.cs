using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Utilities
{
    /// <summary>A utility script to be added to a draggable image</summary>
    [RequireComponent(typeof(Image), typeof(Outline))]
    public class DraggableImage : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        [SerializeField]
        private float speed = 50.0f;

        [SerializeField]
        private AxisConstrained constrained = AxisConstrained.None;

        private Outline outline;

        private bool isDragging;

        public bool IsDraggable { get; private set; } = true;

        private void Awake()
        {
            outline = GetComponent<Outline>();
            SetOutline(false);
        }

        /// <summary>Sets whether this image is draggable or not</summary>
        public void SetDraggability(bool value)
        {
            IsDraggable = value;
            if (isDragging && !value)
            {
                SetOutline(false);
            }
        }

        /// <summary>Sets given axis constrained to this draggable image</summary>
        public void SetConstrained(AxisConstrained constrained)
        {
            this.constrained = constrained;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            if (IsDraggable)
            {
                isDragging = true;
                SetOutline(true);
            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsDraggable)
            {
                Vector3 point = eventData.position;
                ConstrainPosition(ref point);
                transform.position = Vector3.Lerp(transform.position, point, Time.deltaTime * speed);
            }
        }

        /// <summary>Constrains the original position based on current axis constrained</summary>
        private void ConstrainPosition(ref Vector3 original)
        {
            switch (constrained)
            {
                case AxisConstrained.X:
                    original.x = transform.position.x;
                    break;

                case AxisConstrained.Y:
                    original.y = transform.position.y;
                    break;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (IsDraggable)
            {
                isDragging = false;
                SetOutline(false);
            }
        }

        /// <summary>Sets the outline enableability of this draggable image</summary>
        private void SetOutline(bool enabled)
        {
            outline.enabled = enabled;
        }
    }

    public enum AxisConstrained
    {
        None,
        X,
        Y
    }
}