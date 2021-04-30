using Assets.Scripts.Player.Properties;
using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.MiniGames.MailPicking
{
    public class MailPickingGameBehaviour : MonoBehaviour
    {
        [Header("Game Settings")]
        [SerializeField]
        private MailType wantedMailType = MailType.Bank;

        [SerializeField]
        private MailPickingSetting setting = null;

        [SerializeField]
        private PlayerMailProperty mailProperty = null;

        [Header("Animation")]
        [SerializeField]
        private float animationDuration = 1.5f;

        [SerializeField]
        private GameObject basket = null;

        [Header("Tamagotchi")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private int happinessOnCompletion = 10;

        [Header("References")]
        [SerializeField]
        private Canvas wantedLetterCanvas = null;

        [SerializeField]
        private Image wantedLetterImage = null;

        [SerializeField]
        private Image clickableLetter = null;

        [SerializeField]
        private Image[] pileLetters = null;

        [SerializeField]
        private Image[] secondPileLetters = null;

        [SerializeField]
        private GameObject secondLetterLayerObject = null;

        [Header("Letter images")]
        [SerializeField]
        private Sprite fakeBlancoLetter = null;

        [SerializeField]
        private Sprite bankLetter = null;

        [SerializeField]
        private Sprite fakeBankSprite = null;

        [SerializeField]
        private Sprite taxLetter = null;

        [SerializeField]
        private Sprite fakeTaxLetter = null;

        [SerializeField]
        private Sprite healthInsuranceLetter = null;

        [SerializeField]
        private Sprite fakeHealthInsuranceLetter = null;

        [Header("Feedback references")]
        [SerializeField]
        private GameObject resultFeedback = null;

        [SerializeField]
        private Text feedbackHeader = null;

        [SerializeField]
        private Text feedbackMessage = null;

        [SerializeField]
        private GameObject correctLetter = null;

        [SerializeField]
        private GameObject incorrectLetter = null;

        [SerializeField]
        private GameObject winEffects = null;

        [Header("Timer references")]
        [SerializeField]
        private Text timerSeconds = null;

        [SerializeField]
        private float gameDuration = 5f;

        [SerializeField]
        private PlayerRewardProperty rewardCollection = null;
        private string rewardName = "MailPicking";

        private bool isAnimating;
        private bool isFound;
        private float gameBusyDuration = 0f;

        // Start is called before the first frame update
        private void Start()
        {
            //Randomise Wanted MailType if desired
            if (setting.MinigameMode)
            {
                wantedMailType = (MailType)Random.Range(0, System.Enum.GetValues(typeof(MailType)).Length);
            }
            else
            {
                wantedMailType = FetchFirstMailToPickOut();
            }

            //Set letters on all Canvas
            SetWantedImages();
            SetPileLetterLayers();
            //Set listener for the Wanted Letter
            clickableLetter.GetComponent<Button>().onClick.AddListener(LetterFound);
            //Start game timer
            StartGameTimer();

            //imports PlayerRewardObject
            rewardCollection = PlayerPropertyManager.Instance.GetProperty<PlayerRewardProperty>("Reward");
        }

        private void OnDestroy()
        {
            if (clickableLetter != null)
            {
                clickableLetter.GetComponent<Button>().onClick.RemoveListener(LetterFound);
            }
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

        private MailType FetchFirstMailToPickOut()
        {
            foreach (var mail in mailProperty.MailList)
            {
                if (mail.PickedUp && !mail.SortedOut && !mail.Ordened)
                {
                    return (MailType)(mail.Type + 1);
                }
            }

            Debug.LogError("Player is ordering Mail without any ordened mail in his mail property :: this is not intended behaviour!");
            return default;
        }

        ///<summary>This timer depletes the intervaltime from the total game duration.</summary>
        private IEnumerator GameTimer(float waitTime)
        {
            while (gameBusyDuration < gameDuration)
            {
                yield return new WaitForSeconds(1);
                if (!isFound)
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

        ///<summary>Disable drag for all letters.</summary>
        private void TurnOffDraggability()
        {
            clickableLetter.GetComponent<DraggableImage>().SetDraggability(false);
            foreach (var letter in pileLetters)
            {
                letter.GetComponent<DraggableImage>().SetDraggability(false);
            }
            foreach (var letter in secondPileLetters)
            {
                letter.GetComponent<DraggableImage>().SetDraggability(false);
            }
        }

        /// <summary>Adds the letters on top of the Wanted Letter, based on the selected difficulty.</summary>
        private void SetPileLetterLayers()
        {
            SetLettersExceptforType(wantedMailType, pileLetters);
            if (setting.GetAmountOfLayers() > 1)
            {
                secondLetterLayerObject.SetActive(true);
                SetLettersExceptforType(wantedMailType, secondPileLetters);
            }
            if (setting.GetHardEnvelopes())
            {
                secondLetterLayerObject.SetActive(true);
                SetLettersSameEnvelopes();
            }
        }

        /// <summary>Sets all envelopes to lookalikes (HARD MODE ONLY) </summary>
        private void SetLettersSameEnvelopes()
        {
            Sprite fakeImage = null;
            switch (wantedMailType)
            {
                case MailType.Blanco:
                    fakeImage = fakeBlancoLetter;
                    break;

                case MailType.Bank:
                    fakeImage = fakeBankSprite;
                    break;

                case MailType.RijksOverheid:
                    fakeImage = fakeTaxLetter;
                    break;

                case MailType.Zorg:
                    fakeImage = fakeHealthInsuranceLetter;
                    break;
            }
            foreach (var letter in pileLetters)
            {
                letter.sprite = fakeImage;
            }
            foreach (var letter in secondPileLetters)
            {
                letter.sprite = fakeImage;
            }
        }

        /// <summary>Activates when the Wanted Letter is clicked. Puts the Wanted Letter on highest layer and starts an animation.</summary>
        private void LetterFound()
        {
            if (!isAnimating && (gameBusyDuration < gameDuration))
            {
                isFound = true;
                wantedLetterCanvas.sortingOrder = 0;
                StartCoroutine(ShowCorrectLetter());
            }
        }

        ///<summary>Shows positive or negative feedback depending on the players succes</summary>
        private void ShowFeedback(bool isInTime)
        {
            if (isInTime)
            {
                resultFeedback.SetActive(true);
                correctLetter.SetActive(true);
                winEffects.SetActive(true);
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MinigameCorrect);

                if (!setting.MinigameMode)
                {
                    happiness.AddValue(happinessOnCompletion);

                    //Add bronze reward since in normal mode
                    rewardCollection.AddReward(rewardName, RewardType.Bronze);
                } else
                {
                    //add reward according to minigame difficulty 
                    rewardCollection.AddRewardThroughDifficulty(rewardName, setting.Difficulty);
                }
                //print rewardCollection for testing purposes
                rewardCollection.PrintRewardCollection();
            }
            else
            {
                feedbackHeader.text = "Helaas...";
                feedbackMessage.text = "De brief is niet gevonden.";
                resultFeedback.SetActive(true);
                incorrectLetter.SetActive(true);
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
                mailProperty.SortMail((MailOrdering.MailType)(wantedMailType - 1));
                setting.SetIsCompletedInStoryMode();
            }
        }

        /// <summary>Sets the sprites for all letters EXCEPT the wanted letter and its example.</summary>
        private void SetLettersExceptforType(MailType mailWanted, Image[] images)
        {
            int type = 0;
            foreach (var letter in images)
            {
                //Set random rotation
                int randomDegrees = UnityEngine.Random.Range(0, 360);
                letter.transform.localEulerAngles = new Vector3(0, 0, randomDegrees);
                //Set type of letter (to not be the one that we are looking for)
                switch (type)
                {
                    case 0:
                        if (mailWanted != MailType.Bank)
                        {
                            letter.sprite = bankLetter;
                        }
                        type++;
                        break;

                    case 1:
                        if (mailWanted != MailType.RijksOverheid)
                        {
                            letter.sprite = taxLetter;
                        }
                        type++;
                        break;

                    case 2:
                        if (mailWanted != MailType.Zorg)
                        {
                            letter.sprite = healthInsuranceLetter;
                        }
                        type = 0;
                        break;
                }
            }
        }

        /// <summary>Sets the sprites for the wanted letter and its example</summary>
        private void SetWantedImages()
        {
            //Random spawn wanted letter
            clickableLetter.transform.localEulerAngles = new Vector3(0, 0, Random.Range(0, 360));
            int randomHeight = UnityEngine.Random.Range(0, 500);
            int randomWidth = UnityEngine.Random.Range(-250, 250);
            clickableLetter.transform.position += new Vector3(randomWidth, randomHeight, 0);
            //Set the corresponding sprite
            switch (wantedMailType)
            {
                case MailType.Bank:
                    wantedLetterImage.sprite = bankLetter;
                    clickableLetter.sprite = bankLetter;
                    break;

                case MailType.RijksOverheid:
                    wantedLetterImage.sprite = taxLetter;
                    clickableLetter.sprite = taxLetter;
                    break;

                case MailType.Zorg:
                    wantedLetterImage.sprite = healthInsuranceLetter;
                    clickableLetter.sprite = healthInsuranceLetter;
                    break;
            }
        }

        /// <summary>Moves wanted letter to the center of the screen and displays the feedback afterwards.</summary>
        private IEnumerator ShowCorrectLetter()
        {
            isAnimating = true;
            //Set differences in position
            Vector3 startLocationLetter = clickableLetter.transform.position;
            Vector3 centerOfBasket = basket.transform.position;

            //Set differences in rotation
            Vector3 localRotation = clickableLetter.transform.localEulerAngles;
            Vector3 tiltRotation = new Vector3(0, 0, 90);

            //Animation
            float time = 0;
            while (time < animationDuration)
            {
                time += Time.deltaTime;
                if (time > animationDuration)
                {
                    time = animationDuration;
                }
                float percentage = time / animationDuration;
                clickableLetter.transform.position = Vector3.Lerp(startLocationLetter, centerOfBasket, percentage);
                clickableLetter.transform.localEulerAngles = Vector3.Lerp(localRotation, tiltRotation, percentage);
                yield return new WaitForFixedUpdate();
            }
            isAnimating = false;
            ShowFeedback(true);
        }
    }
}