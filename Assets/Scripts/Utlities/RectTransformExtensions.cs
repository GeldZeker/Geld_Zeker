using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    public static class RectTransformExtensions
    {
        public static bool Overlaps(this RectTransform a, RectTransform b)
        {
            return a.WorldRect().Overlaps(b.WorldRect());
        }

        public static bool Overlaps(this RectTransform a, RectTransform b, bool allowInverse)
        {
            return a.WorldRect().Overlaps(b.WorldRect(), allowInverse);
        }

        public static Rect WorldRect(this RectTransform rectTransform)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            float rectTransformWidth = sizeDelta.x * rectTransform.lossyScale.x;
            float rectTransformHeight = sizeDelta.y * rectTransform.lossyScale.y;

            Vector3 position = rectTransform.position;

            return new Rect(position.x - rectTransformWidth / 2f, position.y - rectTransformHeight / 2f, rectTransformWidth, rectTransformHeight);
        }

        /// <summary>Returns whether there was a touch outside of the given rectangle transform</summary>
        public static bool TouchOutsideTransform(RectTransform rectTransform)
        {
            return Input.touchCount > 0 && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.GetTouch(0).position);
        }

        /// <summary>Returns whether there was a press outside of the given rectangle transform</summary>
        public static bool PressOutsideTransform(RectTransform rectTransform)
        {
            return Input.anyKeyDown && !RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
        }
    }
}