using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.MiniGames.InvoiceDragging
{
    public class InvoiceDraggingBehaviour : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField]
        private InvoiceDraggingSetting setting = null;

        [SerializeField]
        private PlayerMoneyProperty playerMoneyProperty = null;

        [Header("Tamagotchi")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private int happinessOnCompletion = 10;

        [Header("References")]
        [SerializeField]
        private Image totalAmount = null;

        [SerializeField]
        private Image iBAN = null;

        [SerializeField]
        private Image businessName = null;

        [Header("Feedback references")]
        [SerializeField]
        private GameObject resultFeedback = null;

        [SerializeField]
        private Text feedbackHeader = null;

        [SerializeField]
        private Text feedbackMessage = null;

        [SerializeField]
        private GameObject winEffects = null;

        [Header("Timer references")]
        [SerializeField]
        private Text timerSeconds = null;

        [SerializeField]
        private float gameDuration = 5f;

        private bool itemsCorrect = false;
        private float gameBusyDuration = 0f;

        private void Start()
        {
            StartGameTimer();
        }

        private void StartGameTimer()
        {
            //start game timer after introduction if it is active
            if (IntroductionManager.Instance.IsActive)
            {
                IntroductionManager.Instance.IntroFinished += OnIntroFinished;
            }
            else
            {
                StartCoroutine(GameTimer(gameDuration));
            }
        }

        private void OnIntroFinished(Introduction introduction)
        {
            IntroductionManager.Instance.IntroFinished -= OnIntroFinished;
            StartCoroutine(GameTimer(gameDuration));
        }

        ///<summary>This timer depletes the intervaltime from the total game duration.</summary>
        private IEnumerator GameTimer(float waitTime)
        {
            while (gameBusyDuration < gameDuration)
            {
                yield return new WaitForSeconds(1);
                if (!itemsCorrect)
                {
                    gameBusyDuration++;
                    timerSeconds.text = (gameDuration - gameBusyDuration).ToString();
                }
                else
                {
                    TurnOffDraggability();
                }
            }
            TurnOffDraggability();
            ShowFeedback(false);
        }

        private void itemsCorrectPlace()
        {
            //TODO: Check of alle vakjes correct ingevuld zijn met de juiste elementen.
        }

        ///<summary>Disable drag for all elements.</summary>
        private void TurnOffDraggability()
        {
            totalAmount.GetComponent<DraggableImage>().SetDraggability(false);
            iBAN.GetComponent<DraggableImage>().SetDraggability(false);
            businessName.GetComponent<DraggableImage>().SetDraggability(false);
        }

        ///<summary>Shows positive or negative feedback depending on the players succes</summary>
        private void ShowFeedback(bool isInTime)
        {
            if (isInTime)
            {
                resultFeedback.SetActive(true);
                winEffects.SetActive(true);
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MinigameCorrect);

                if (!setting.MinigameMode)
                {
                    happiness.AddValue(happinessOnCompletion);
                    playerMoneyProperty.RemoveMoney(25.65);
                }
            }
            else
            {
                feedbackHeader.text = "Helaas...";
                feedbackMessage.text = "De elementen staan niet op de goede plek.";
                resultFeedback.SetActive(true);
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MinigameFailed);
            }

            if (setting.MinigameMode)
            {
                //set difficulty played as completed
                setting.SetCurrentDifficultyCompleted();
            }
            else
            {
                //if the game was not played in the game hall, update properties and mark it as played in store mode
                setting.SetIsCompletedInStoryMode();
            }
        }
    }
}