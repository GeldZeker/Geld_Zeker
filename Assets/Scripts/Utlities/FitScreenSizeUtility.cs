using UnityEditor;
using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    public static class FitScreenSizeUtility
    {
        static FitScreenSizeUtility()
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlaying) return;
#endif
            ScreenWorldScale = CalculateScreenWorldScale();
        }

        /// <summary>
        /// Use this property to get the ScreenWorldScale since this is a cached value
        /// </summary>
        public static Vector3 ScreenWorldScale { get; private set; }

        /// <summary>Calculates the screen world scale. Use only in editor outside of play mode</summary>
        public static Vector3 CalculateScreenWorldScale()
        {
            Vector3 topRightCorner = new Vector2(1, 1);
            Vector3 edgeVector = Camera.main.ViewportToWorldPoint(topRightCorner);
            return edgeVector * 2.0f;
        }
    }
}