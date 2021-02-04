using BWolf.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.MailOrdering
{
    public class AnimatablePaper : MonoBehaviour
    {
        private float animateTime;

        private SpriteRenderer spriteRenderer;
        private Sprite frontSprite;
        private Sprite backSprite;

        private Vector3 rotation;

        public bool GoingLeft { get; private set; }
        public bool IsAnimating { get; private set; }
        public bool IsOnBack { get; private set; }
        public bool IsTab { get; private set; }

        public MailType MailShowing { get; private set; }

        public float AnimateTimeLeft
        {
            get { return currentRotation.TimeLeft; }
        }

        private LerpValue<Vector3> currentRotation;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        private void Start()
        {
            rotation = transform.localEulerAngles;
        }

        public void SetIsTab(bool isTab)
        {
            IsTab = isTab;
        }

        public void SetAnimateTime(float time)
        {
            animateTime = time;
        }

        public void SetDisplay(PaperSprite sprite)
        {
            frontSprite = sprite.front;
            backSprite = sprite.back;

            SetIsOnBack(false);
        }

        public void SetMailShowing(MailType mailType)
        {
            MailShowing = mailType;
        }

        public void SetIsAnimating(bool value)
        {
            IsAnimating = value;
        }

        public void SetOrderInLayer(int order)
        {
            spriteRenderer.sortingOrder = order;
        }

        public void SetIsLeft(bool value)
        {
            GoingLeft = value;
        }

        public void SetIsOnBack(bool value)
        {
            IsOnBack = value;
            spriteRenderer.sprite = value ? backSprite : frontSprite;
        }

        public LerpValue<Vector3> RotateToMiddle
        {
            get
            {
                float yStart = GoingLeft ? -180.0f : 0.0f;
                currentRotation = new LerpValue<Vector3>(new Vector3(rotation.x, yStart, rotation.z), new Vector3(rotation.x, -90.0f, rotation.z), animateTime);
                return currentRotation;
            }
        }

        public LerpValue<Vector3> RotateToOutside
        {
            get
            {
                float yEnd = GoingLeft ? 0.0f : -180.0f;
                currentRotation = new LerpValue<Vector3>(new Vector3(rotation.x, -90.0f, rotation.z), new Vector3(rotation.x, yEnd, rotation.z), animateTime);
                return currentRotation;
            }
        }
    }
}