using UnityEngine;

namespace GameStudio.GeldZeker.Utilities
{
    public class FitScreenSize : MonoBehaviour
    {
        [SerializeField, Range(0.1f, 1.0f)]
        private float scalar = 1.0f;

        private void Awake()
        {
            ScaleToFitScreen();
        }

        private void ScaleToFitScreen()
        {
            // get the sprite width in world space units
            Vector2 size = GetComponent<SpriteRenderer>().sprite.bounds.size;
            float worldSpriteWidth = size.x;
            float worldSpriteHeight = size.y;

            // get the screen height & width in world space units
            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float worldScreenWidth = (worldScreenHeight / Screen.height) * Screen.width;

            // initialize new scale to the current scale
            Vector3 newScale = transform.localScale;

            // divide screen width by sprite width, set to X axis scale
            newScale.x = worldScreenWidth / worldSpriteWidth;
            newScale.y = worldScreenHeight / worldSpriteHeight;

            // apply scale change
            transform.localScale = newScale * scalar;
        }

#if UNITY_EDITOR

        [ContextMenu("ScaleToFitScreen")]
        public void ScaleToFitScreenEditor()
        {
            string[] res = UnityEditor.UnityStats.screenRes.Split('x');
            int screenHeight = int.Parse(res[1]);
            int screenWidth = int.Parse(res[0]);

            // get the sprite width in world space units
            Vector2 size = GetComponent<SpriteRenderer>().sprite.bounds.size;
            float worldSpriteWidth = size.x;
            float worldSpriteHeight = size.y;

            // get the screen height & width in world space units
            float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
            float worldScreenWidth = (worldScreenHeight / screenHeight) * screenWidth;

            // initialize new scale to the current scale
            Vector3 newScale = transform.localScale;

            // divide screen width by sprite width, set to X axis scale
            newScale.x = worldScreenWidth / worldSpriteWidth;
            newScale.y = worldScreenHeight / worldSpriteHeight;

            // apply scale change
            transform.localScale = newScale * scalar;
        }

#endif
    }
}