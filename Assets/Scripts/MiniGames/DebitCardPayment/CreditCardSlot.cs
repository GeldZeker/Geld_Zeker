using BWolf.Utilities;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.MiniGames.DebitCardPayment
{
    /// <summary>A behaviour representing a card slot on a Pin Device</summary>
    public class CreditCardSlot : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float moveToPositionTime = 0.45f;

        [Header("References")]
        [SerializeField]
        private RectTransform debitCardTransform = null;

        [SerializeField]
        private DebitCardPaySetting setting = null;

        [SerializeField]
        private RectMask2D mask = null;

        private Image image;
        private RectTransform rectTransform;
        private DraggableImage draggableCard;

        private bool isOverlapping;
        private bool isActive;

        private bool isInserting;
        private bool isInserted;

        private Vector2 insertStart;
        private float insertYMax;

        private const float CARD_Z_ANGLE = -90;

        public event Action OnInsertion;

        private void Awake()
        {
            //the credit card slot is only active on hard difficulty
            isActive = setting.Difficulty == MiniGameDifficulty.Hard;
        }

        private void Start()
        {
            if (isActive)
            {
                draggableCard = debitCardTransform.GetComponent<DraggableImage>();
                rectTransform = GetComponent<RectTransform>();

                image = GetComponent<Image>();
                image.enabled = false;

                insertYMax = debitCardTransform.sizeDelta.x * 0.25f;
                insertStart = new Vector2(0, -(debitCardTransform.sizeDelta.x * 0.5f) + rectTransform.sizeDelta.y * 0.5f);
            }
        }

        private void Update()
        {
            if (!isActive)
            {
                return;
            }

            //check card release before touch or mouse press check since release is registered a frame after
            CheckCardRelease();

            if (!PlayerInputSystem.Touched && !PlayerInputSystem.MousePressed)
            {
                return;
            }

            if (isInserting)
            {
                CheckInsertion();
            }
            else
            {
                CheckInteraction();
            }
        }

        /// <summary>Clamps the debitcard to maximum y insert position and Checks whether the card is inserted</summary>
        private void CheckInsertion()
        {
            if (isInserted)
            {
                return;
            }

            Vector3 position = debitCardTransform.anchoredPosition;
            position.y = Mathf.Clamp(position.y, insertStart.y, insertYMax);
            debitCardTransform.anchoredPosition = position;

            if (position.y == insertYMax)
            {
                SetOutline(false);
                SetCardDraggability(false);
                OnInsertion();

                isInserted = true;
            }
        }

        /// <summary>Checks whether the overlapping credit card is released by the player</summary>
        private void CheckCardRelease()
        {
            if (!isInserting && isOverlapping && (PlayerInputSystem.MouseUp || PlayerInputSystem.VingerUp)) //make cross platform (Android)
            {
                SetCardDraggability(false);
                SetOutline(false);
                StartCoroutine(MoveToPositionRoutine());
            }
        }

        /// <summary>Checks whether the credit card is interaction with the credit card slot</summary>
        private void CheckInteraction()
        {
            if (debitCardTransform.Overlaps(rectTransform))
            {
                if (!isOverlapping)
                {
                    SetOutline(true);
                    isOverlapping = true;
                }
            }
            else if (isOverlapping)
            {
                SetOutline(false);
                isOverlapping = false;
            }
        }

        /// <summary>Returns a routine that translates the credit card in insertion position</summary>
        private IEnumerator MoveToPositionRoutine()
        {
            LerpValue<Vector3> move = new LerpValue<Vector3>(debitCardTransform.anchoredPosition, insertStart, moveToPositionTime);
            LerpValue<Vector3> rotate = new LerpValue<Vector3>(Vector3.zero, new Vector3(0, 0, CARD_Z_ANGLE), moveToPositionTime);
            while (move.Continue() && rotate.Continue())
            {
                debitCardTransform.anchoredPosition = Vector3.Lerp(move.start, move.end, move.perc);
                debitCardTransform.eulerAngles = Vector3.Lerp(rotate.start, rotate.end, rotate.perc);
                yield return null;
            }

            SetMask(true);
            SetOutline(true);
            SetCardDraggability(true);
            SetInsertConstrained(true);

            isInserting = true;
        }

        /// <summary>Sets the Rect2D mask enableability</summary>
        private void SetMask(bool value)
        {
            mask.enabled = value;
        }

        /// <summary>Sets the debit cards drag ability</summary>
        private void SetCardDraggability(bool value)
        {
            draggableCard.SetDraggability(value);
        }

        /// <summary>Sets whether the debit card needs to be constrained on the X axis</summary>
        private void SetInsertConstrained(bool value)
        {
            draggableCard.SetConstrained(value ? AxisConstrained.X : AxisConstrained.None);
        }

        /// <summary>Sets the outline enableability</summary>
        private void SetOutline(bool value)
        {
            image.enabled = value;
        }
    }
}