using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class DraggableSprite : MonoBehaviour
    {
        private Camera mainCam;

        private SpriteRenderer spriteRenderer;

        public bool IsDraggable { get; private set; } = true;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            mainCam = Camera.main;
        }

        public void SetDraggability(bool value)
        {
            IsDraggable = value;
        }

        public void OnBeginDrag()
        {
        }

        public void OnDrag(Vector2 point)
        {
            if (IsDraggable)
            {
                SetPosition(point);
            }
        }

        public void OnEndDrag()
        {
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        private void SetPosition(Vector3 newPosition)
        {
            Sprite sprite = spriteRenderer.sprite;
            Vector3 pivot = sprite.pivot;
            Vector3 halfSize = sprite.textureRect.size * 0.5f;
            Vector3 correction = (pivot - halfSize) / 100.0f;
            transform.position = newPosition + correction;
        }
    }
}