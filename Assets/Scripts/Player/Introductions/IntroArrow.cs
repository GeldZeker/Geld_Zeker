using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Introductions
{
    public class IntroArrow : MonoBehaviour
    {
        private FloatableCanvasElement floatable;

        private void Awake()
        {
            floatable = GetComponent<FloatableCanvasElement>();
        }

        public void PlaceRelative(RectTransform relativeTransform, RelativePosition position, float spacing)
        {
            RectTransform rectTransform = (RectTransform)transform;
            Vector2 size = relativeTransform.sizeDelta;
            switch (position)
            {
                case RelativePosition.Top:
                    rectTransform.anchoredPosition = new Vector2(0, size.y * 0.5f + spacing);
                    transform.localEulerAngles = new Vector3(0, 0, 180);
                    break;

                case RelativePosition.Bottom:
                    rectTransform.anchoredPosition = new Vector2(0, -(size.y * 0.5f + spacing));
                    break;

                case RelativePosition.Left:
                    rectTransform.anchoredPosition = new Vector2(-(size.x * 0.5f + spacing), 0);
                    transform.localEulerAngles = new Vector3(0, 0, -90);
                    break;

                case RelativePosition.Right:
                    rectTransform.anchoredPosition = new Vector2(size.x * 0.5f + spacing, 0);
                    transform.localEulerAngles = new Vector3(0, 0, 90);
                    break;
            }

            floatable.ReStart();
        }

        public enum RelativePosition
        {
            Top,
            Bottom,
            Left,
            Right
        }
    }
}