using BWolf.Utilities.PlayerProgression.Quests;
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

        [SerializeField]
        private double moneyToRemove = 0;

        [Header("Quest")]
        [SerializeField]
        private Quest payInvoiceQuest = null;

        [Header("Tamagotchi")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private int happinessOnCompletion = 10;

        [Header("References")]
        [SerializeField]
        private GameObject totalAmountDraggable = null;

        [SerializeField]
        private GameObject iBANDraggable = null;

        [SerializeField]
        private GameObject businessNameDraggable = null;

        [SerializeField]
        private GameObject totalAmountBox = null;

        [SerializeField]
        private GameObject iBANBox = null;

        [SerializeField]
        private GameObject businessNameBox = null;

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

        private bool itemsCorrect = false;
        private float gameBusyDuration = 0f;

        // Start is called before the first frame update.
        private void Start()
        {
            StartGameTimer();
        }

        /// <summary> Start the game timer depending on introduction. </summary>
        private void StartGameTimer()
        {
            //start game timer after introduction if it is active
            if (IntroductionManager.Instance.IsActive)
            {
                IntroductionManager.Instance.IntroFinished += OnIntroFinished;
            }
            else
            {
                StartCoroutine(GameTimer(setting.GetAmountOfSeconds()));
            }
        }

        /// <summary> Checks if introduction is finished. </summary>
        private void OnIntroFinished(Introduction introduction)
        {
            IntroductionManager.Instance.IntroFinished -= OnIntroFinished;
            StartCoroutine(GameTimer(setting.GetAmountOfSeconds()));
        }

        ///<summary> This timer depletes the intervaltime from the total game duration. </summary>
        private IEnumerator GameTimer(float waitTime)
        {
            while (gameBusyDuration < setting.GetAmountOfSeconds())
            {
                yield return new WaitForSeconds(1);
                if (!itemsCorrect)
                {
                    gameBusyDuration++;
                    timerSeconds.text = (setting.GetAmountOfSeconds() - gameBusyDuration).ToString();
                }
                else
                {
                    TurnOffDraggability();
                }
            }
            TurnOffDraggability();
            ShowFeedback(false);
        }

        ///<summary> Check if the boxes have the right elements inside of them. </summary>
        public void itemsCorrectPlace()
        {
            if(totalAmountBox.transform.Find("TotalAmount") && iBANBox.transform.Find("IBAN") && businessNameBox.transform.Find("BusinessName"))
            {
                itemsCorrect = true;
                TurnOffDraggability();
                ShowFeedback(true);
            }
        }

        ///<summary> Disable drag for all elements. </summary>
        private void TurnOffDraggability()
        {
            totalAmountDraggable.GetComponent<DragDrop>().SetDraggability(false);
            iBANDraggable.GetComponent<DragDrop>().SetDraggability(false);
            businessNameDraggable.GetComponent<DragDrop>().SetDraggability(false);
        }

        ///<summary> Shows positive or negative feedback depending on the players succes. </summary>
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
                    playerMoneyProperty.RemoveMoney(moneyToRemove);
                }
            }
            else
            {
                feedbackHeader.text = "Helaas...";
                feedbackMessage.text = "Niet goed ingevuld.";
                resultFeedback.SetActive(true);
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MinigameFailed);
            }

            if (setting.MinigameMode)
            {
                // set difficulty played as completed.
                setting.SetCurrentDifficultyCompleted();
            }
            else
            {
                // if the game was not played in the game hall, update properties and mark it as played in store mode.
                setting.SetIsCompletedInStoryMode();
            }

            // sets the quest "FactuurBetalen" to done.
            if (payInvoiceQuest.IsUpdatable)
            {
                DoOnceTask payInvoiceTask = payInvoiceQuest.GetTask<DoOnceTask>("FactuurBetalen");
                payInvoiceTask.SetDoneOnce();
            }
        }
    }
}