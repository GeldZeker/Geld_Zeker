using Assets.Scripts.Player.Properties;
using BWolf.Utilities.CharacterDialogue;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.MiniGames.DebitCardPayment
{
    /// <summary>A behaviour for simulating a contactless payment process using a Debit Card</summary>
    public class PinDevice : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private float confirmationTime = 0.75f;

        [SerializeField]
        private float processTime = 1.0f;

        [SerializeField]
        private int HappinessOnCompletion = 10;

        [SerializeField]
        private string sceneToLoadOnComplete = "Kitchen";

        [SerializeField]
        private DebitCardPaySetting setting = null;

        [Header("References")]
        [SerializeField]
        private RectTransform debitCardTransform = null;

        [SerializeField]
        private RectTransform screenTransform = null;

        [SerializeField]
        private PinDeviceKeyboard keyboard = null;

        [SerializeField]
        private CreditCardSlot cardSlot = null;

        [SerializeField]
        private Introduction gameHallIntroduction = null;

        [Header("Quests")]
        [SerializeField]
        private Quest contactlessPaymentQuest = null;

        [Header("Properties")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private GroceryProperty ownedGroceryProperty = null;

        [SerializeField]
        private GroceryProperty supermarketGroceryProperty = null;

        [SerializeField]
        private PayProcessScreen screen;

        [Space]
        [SerializeField]
        private Dialogue finishDialogue = null;
        
        [Space]
        [SerializeField]
        private PlayerRewardProperty rewardCollection = null;
        private string rewardName = "DebitCardPayment";

        private bool inPaymentProcess;
        private bool hasCompletedPayment;
        private bool isOverlapping;
        private bool isInteractable;

        private void Awake()
        {
            double displayPrice = 0.0d;
            if (setting.MinigameMode)
            {
                displayPrice = Random.Range(1.0f, 10.0f);
            }
            else
            {
                foreach (var grocery in supermarketGroceryProperty.Groceries)
                {
                    displayPrice += supermarketGroceryProperty.GetPrice(grocery);
                }
            }

            screen.SetPrice(displayPrice);
            screen.SetDisplay();
            screen.SetDifficulty(setting.Difficulty);

            if (setting.Difficulty == MiniGameDifficulty.Hard)
            {
                cardSlot.OnInsertion += OnCardInserted;
            }
        }

        private void Update()
        {
            if (!PlayerInputSystem.Touched && !PlayerInputSystem.MousePressed)
            {
                return;
            }

            switch (setting.Difficulty)
            {
                case MiniGameDifficulty.Easy:
                    CheckContactlessPayment();
                    break;

                case MiniGameDifficulty.Medium:
                    CheckPincodePayment();
                    break;

                case MiniGameDifficulty.Hard:
                    CheckForDenial();
                    break;
            }
        }

        private void OnDestroy()
        {
            if (setting.Difficulty == MiniGameDifficulty.Hard)
            {
                cardSlot.OnInsertion -= OnCardInserted;
            }
        }

        private void CheckForDenial()
        {
            if (debitCardTransform.Overlaps(screenTransform))
            {
                if (isOverlapping || hasCompletedPayment)
                {
                    //exit if isOverlapping flag is already true or payment has already been completed
                    return;
                }

                screen.ShowDenied();

                isOverlapping = true;
            }
            else if (isOverlapping)
            {
                isOverlapping = false;
            }
        }

        private void CheckPincodePayment()
        {
            if (keyboard.IsActive)
            {
                return;
            }

            if (debitCardTransform.Overlaps(screenTransform))
            {
                if (isOverlapping || inPaymentProcess || hasCompletedPayment)
                {
                    //exit if isOverlapping flag is already true, a routine is already busy or payment has already been completed
                    return;
                }

                //cache payment process routine and start it as a coroutine
                StartCoroutine(PinCodePaymentRoutine());

                isOverlapping = true;
            }
            else if (isOverlapping)
            {
                inPaymentProcess = false;
                isOverlapping = false;
            }
        }

        private void CheckContactlessPayment()
        {
            if (debitCardTransform.Overlaps(screenTransform))
            {
                if (isOverlapping || inPaymentProcess || hasCompletedPayment)
                {
                    //exit if isOverlapping flag is already true, a routine is already busy or payment has already been completed
                    return;
                }

                //cache payment process routine and start it as a coroutine
                StartCoroutine(ContactlessPaymentRoutine());

                isOverlapping = true;
            }
            else if (isOverlapping)
            {
                if (inPaymentProcess)
                {
                    //if the debit card stopped overlapping and the payment was in process, stop it
                    StopAllCoroutines();

                    screen.SetStart();
                    screen.SetDisplay();
                    inPaymentProcess = false;
                }

                isOverlapping = false;
            }
        }

        private void OnCardInserted()
        {
            StartCoroutine(PinCodePaymentRoutine());
        }

        /// <summary>Returns a routine that starts a transaction using pin code</summary>
        private IEnumerator PinCodePaymentRoutine()
        {
            inPaymentProcess = true;

            MusicPlayer.Instance.PlaySFXSound(SFXSound.PinTransactionStart);

            screen.MoveNext();
            screen.SetDisplay();

            yield return keyboard.WaitForPincodeRoutine(screen);

            inPaymentProcess = false;
            hasCompletedPayment = true;

            StartCoroutine(ProcessRoutine());
        }

        /// <summary>Returns an enumerator that sets of the payment process</summary>
        private IEnumerator ContactlessPaymentRoutine()
        {
            inPaymentProcess = true;

            MusicPlayer.Instance.PlaySFXSound(SFXSound.PinTransactionStart);

            screen.MoveNext();
            screen.SetDisplay();

            float time = 0;
            while (time < confirmationTime)
            {
                time += Time.deltaTime;
                yield return null;
            }

            inPaymentProcess = false;
            hasCompletedPayment = true;

            StartCoroutine(ProcessRoutine());
        }

        /// <summary>Returns an enumerator that verifies the payment process</summary>
        private IEnumerator ProcessRoutine()
        {
            screen.MoveNext();
            screen.SetDisplay();

            float time = 0;
            while (time < processTime)
            {
                time += Time.deltaTime;
                yield return null;
            }

            screen.MoveNext();
            screen.SetDisplay();

            OnProcessCompleted();
        }

        /// <summary>Called when the payment process has been completed to give feedback and start the end process</summary>
        private void OnProcessCompleted()
        {
            MusicPlayer.Instance.PlaySFXSound(SFXSound.PinTransitionEnd);

            if (setting.MinigameMode)
            {
                //set difficulty played as completed
                setting.SetCurrentDifficultyCompleted();

                //add reward according to minigame difficulty 
                rewardCollection.AddRewardThroughDifficulty(rewardName, setting.Difficulty);

                SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, NavigationSystem.NameOfGameHall, UnityEngine.SceneManagement.LoadSceneMode.Additive);
            }
            else
            {
                //if not in minigame mode, add property values
                happiness.AddValue(HappinessOnCompletion);
                ownedGroceryProperty.AddGroceries(supermarketGroceryProperty.Groceries.ToArray());

                //mark game as available in minigame mode
                setting.SetIsCompletedInStoryMode();

                //update quests
                if (contactlessPaymentQuest.IsUpdatable)
                {
                    DoOnceTask payContactlessOnceTask = contactlessPaymentQuest.GetTask<DoOnceTask>("1KeerContactloosBetalen");
                    payContactlessOnceTask.SetDoneOnce();
                }
                
                //Add bronze reward since in normal mode
                rewardCollection.AddReward(rewardName, RewardType.Bronze);

                //and start dialogue with cassiere to transition back home
                MainCanvasManager.Instance.StartDialogue(finishDialogue, () =>
                {
                    SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, sceneToLoadOnComplete, UnityEngine.SceneManagement.LoadSceneMode.Additive);
                    gameHallIntroduction.Start();
                });
            }
        }

        [System.Serializable]
        public struct PayProcessScreen
        {
#pragma warning disable 0649

            [Header("Display")]
            [SerializeField]
            private Image display;

            [SerializeField]
            private Sprite[] processScreens;

            [Header("Price")]
            [SerializeField]
            private Text txtComponent;

            [SerializeField]
            private int indexOfPriceScreen;

            [SerializeField]
            private Sprite pinScreen;

            [SerializeField]
            private Sprite deniedScreen;

#pragma warning restore 0649

            private double price;

            private int indexOfScreen;
            private MiniGameDifficulty difficulty;

            /// <summary>Moves to next display sprite, use SetDisplay to show new display</summary>
            public void MoveNext()
            {
                indexOfScreen++;
                if (indexOfScreen == processScreens.Length)
                {
                    SetStart();
                }

                DisplayPrice();
            }

            public void ShowDenied()
            {
                display.sprite = deniedScreen;
            }

            public void SetDifficulty(MiniGameDifficulty difficulty)
            {
                this.difficulty = difficulty;
                if (difficulty != MiniGameDifficulty.Easy)
                {
                    processScreens[indexOfPriceScreen] = pinScreen;
                }
            }

            /// <summary>Sets the price value to be shown on the display</summary>
            public void SetPrice(double price)
            {
                this.price = price;
            }

            public void SetText(string text)
            {
                txtComponent.text = text;
            }

            /// <summary>Sets the price to be shown on screen depending on index of price screen</summary>
            private void DisplayPrice()
            {
                if (difficulty == MiniGameDifficulty.Easy)
                {
                    SetText(indexOfPriceScreen == indexOfScreen ? string.Format("{0:0.00}", price) : string.Empty);
                }
            }

            /// <summary>Sets the display sprite based on current index of screen</summary>
            public void SetDisplay()
            {
                display.sprite = processScreens[indexOfScreen];
            }

            /// <summary>Resets the index of screen to first</summary>
            public void SetStart()
            {
                indexOfScreen = 0;
                txtComponent.text = string.Empty;
            }
        }
    }
}