using BWolf.Utilities;
using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Interaction.Supermarket
{
    public class PinDeviceButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float animationTime = 2f;

        [Header("Scene References")]
        [SerializeField]
        private Button minigameButton = null;

        [Space]
        [SerializeField]
        private Transform tfAnimationEnd = null;

        [SerializeField]
        private Transform tfGroceries = null;

        [Header("Project References")]
        [SerializeField]
        private GroceryProperty groceryProperty = null;

        private bool canShowGroceries;

        private bool checkReplay = true;

        private void Start()
        {
            canShowGroceries = groceryProperty.Groceries.Count != 0;
            //set active state after introduction has finished if it is active, or set it immidiately
            if (IntroductionManager.Instance.IsActive)
            {
                IntroductionManager.Instance.IntroFinished += OnIntroFinish;
            }
            else
            {
                SetActiveStateOfMiniGameButton();
                TryStartConveyerBeltAnimation();
            }
            StartCoroutine(CheckReplayDialogue());
        }

        private void OnDestroy()
        {
            if (IntroductionManager.Instance != null)
            {
                IntroductionManager.Instance.IntroFinished -= OnIntroFinish;
            }
            checkReplay = false;
        }

        private void TryStartConveyerBeltAnimation()
        {
            if (canShowGroceries)
            {
                StartCoroutine(ConveyerBeltAnimation());
            }
        }

        private IEnumerator ConveyerBeltAnimation()
        {
            SetEnabledStateOfGroceries(false);

            LerpValue<Vector3> drag = new LerpValue<Vector3>(tfGroceries.position, tfAnimationEnd.position, animationTime);
            while (drag.Continue())
            {
                tfGroceries.position = Vector3.Lerp(drag.start, drag.end, drag.perc);
                yield return null;
            }

            MusicPlayer.Instance.PlaySFXSound(SFXSound.PinTransactionStart);
            SetEnabledStateOfGroceries(true);
        }

        private void SetEnabledStateOfGroceries(bool value)
        {
            minigameButton.interactable = value;
        }

        /// <summary>Sets the active state of the mini game button based on ammount of groceries in the grocery property</summary>
        private void SetActiveStateOfMiniGameButton()
        {
            minigameButton.gameObject.SetActive(canShowGroceries);
        }

        private void OnIntroFinish(Introduction introduction)
        {
            SetActiveStateOfMiniGameButton();
            TryStartConveyerBeltAnimation();
            IntroductionManager.Instance.IntroFinished -= OnIntroFinish;
        }
        private IEnumerator CheckReplayDialogue()
        {
            while (checkReplay)
            {
                if (IntroductionManager.Instance.IsActive)
                {
                    minigameButton.gameObject.SetActive(true);
                }
                else
                {
                    SetActiveStateOfMiniGameButton();
                }
                yield return null;
            }
        }
    }
}