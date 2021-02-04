using BWolf.Behaviours.SingletonBehaviours;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.Player
{
    public class PlayerInputSystem : SingletonBehaviour<PlayerInputSystem>
    {
        [SerializeField]
        private LayerMask mask;

        private Camera mainCam;
        private DraggableSprite spriteDragging;

        /// <summary>Returns whether the mouse has been pressed in the editor</summary>
        public static bool MousePressed
        {
            get { return Application.platform == RuntimePlatform.WindowsEditor && Input.GetMouseButton(0); }
        }

        /// <summary>Returns whether a finger has touched the screen on a mobile android device </summary>
        public static bool Touched
        {
            get { return Application.platform == RuntimePlatform.Android && Input.touchCount == 1; }
        }

        /// <summary>Returns whether a finger has released the screen on a mobile android device </summary>
        public static bool VingerUp
        {
            get { return Application.platform == RuntimePlatform.Android && vingerUp; }
        }

        /// <summary>Returns whether the mouse has been released in the editor</summary>
        public static bool MouseUp
        {
            get { return Application.platform == RuntimePlatform.WindowsEditor && Input.GetMouseButtonUp(0); }
        }

        private static bool vingerhold;
        private static bool vingerUp;

        protected override void Awake()
        {
            base.Awake();

            if (isDuplicate)
            {
                return;
            }

            mainCam = Camera.main;
        }

        private void Start()
        {
            SceneTransitionSystem.Instance.TransitionCompleted += OnSceneTransitioned;
        }

        private void OnSceneTransitioned(Scene scene, LoadSceneMode mode)
        {
            mainCam = Camera.main;
        }

        private void Update()
        {
            if (SceneTransitionSystem.Instance.IsTransitioning)
            {
                return;
            }

            CheckTouchInput();
            CheckSpriteDrag();
        }

        private void CheckTouchInput()
        {
            vingerUp = vingerhold && Input.touchCount == 0;
            vingerhold = Input.touchCount == 1;
        }

        private void CheckSpriteDrag()
        {
            if (MousePressed || Touched)
            {
                Vector3 screenposition = GetScreenPoint();
                RaycastHit2D hit = Physics2D.Raycast(screenposition, Vector2.zero, mask.value);
                if (hit.collider != null)
                {
                    if (spriteDragging != null)
                    {
                        spriteDragging.OnDrag(hit.point);
                    }
                    else
                    {
                        spriteDragging = hit.collider.GetComponent<DraggableSprite>();
                        spriteDragging?.OnBeginDrag();
                    }
                }
                else
                {
                    if (spriteDragging != null)
                    {
                        ResetSpriteDragging();
                    }
                }
            }
            else
            {
                if (spriteDragging != null)
                {
                    ResetSpriteDragging();
                }
            }
        }

        private void ResetSpriteDragging()
        {
            spriteDragging.OnEndDrag();
            spriteDragging = null;
        }

        private Vector3 GetScreenPoint()
        {
            if (Application.platform == RuntimePlatform.WindowsEditor)
            {
                return mainCam.ScreenToWorldPoint(Input.mousePosition);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                return mainCam.ScreenToWorldPoint(Input.GetTouch(0).position);
            }
            else
            {
                return Vector3.zero;
            }
        }
    }
}