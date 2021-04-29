using Assets.Scripts.Player.Properties;
using BWolf.Utilities;
using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace GameStudio.GeldZeker.MiniGames.MailOrdering
{
    public class PaperPlacer : MonoBehaviour
    {
        [Header("Paper Settings")]
        [SerializeField]
        private float placeTime = 1.0f;

        [SerializeField]
        private Vector3 popupScale = new Vector3(1.125f, 1.125f, 1.0f);

        [Header("Feedback Settings")]
        [SerializeField]
        private string succesHead = "Goedzo!";

        [SerializeField]
        private string failHead = "Helaas";

        [SerializeField]
        private int happinessOnSucces = 10;

        [Header("Scene References")]
        [SerializeField]
        private PaperPlacement placement = null;

        [SerializeField]
        private GameObject feedbackObject = null;

        [SerializeField]
        private Text txtHeader = null;

        [SerializeField]
        private Text txtMessage = null;

        private FolderAnimator animator = null;

        [SerializeField]
        private DraggableSprite paperToPlace = null;

        [Header("Player Properties")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        [SerializeField]
        private PlayerMailProperty mailProperty = null;

        [SerializeField]
        private PlayerRewardProperty rewardCollection = null;

        [SerializeField]
        private MailOrderSetting setting = null;

        [Header("Quests")]
        [SerializeField]
        private Quest createbankAccountQuest = null;

        public MailType MailPlacing { get; private set; }

        private bool touch;
        private bool canPlace;

        private string rewardName = "MailOrdering";

        private void Awake()
        {
            animator = GetComponent<FolderAnimator>();

            //imports PlayerRewardObject
            rewardCollection = PlayerPropertyManager.Instance.GetProperty<PlayerRewardProperty>("Reward");
        }

        private void Start()
        {
            if (mailProperty.HasMailToOrden)
            {
                MailPlacing = FetchFirstMailToOrden();
            }
            else
            {
                if (animator.Setting.MinigameMode)
                {
                    MailPlacing = (MailType)Random.Range(0, System.Enum.GetValues(typeof(MailType)).Length);
                }
                else
                {
                    Debug.LogError("Player is ordering Mail without any mail in his mail property :: this is not intended behaviour!");
                }
            }

            animator.OnFinish += OnAnimatorFinish;

            placement.OnPaperTrigger += SetPlaceability;
            placement.SetActive(false);

            Sprite mailSprite = mailProperty.GetMailSprite(MailPlacing);
            placement.SetSprite(mailSprite);
            paperToPlace.SetSprite(mailSprite);
        }

        private MailType FetchFirstMailToOrden()
        {
            foreach (var mail in mailProperty.MailList)
            {
                if (mail.PickedUp && mail.SortedOut && !mail.Ordened)
                {
                    return mail.Type;
                }
            }

            Debug.LogError("Player is ordering Mail without any ordened mail in his mail property :: this is not intended behaviour!");
            return default;
        }

        private void OnAnimatorFinish(PlacementResult result)
        {
            StartCoroutine(MoveTowardsPlacementWithResult(result));
        }

        private void OnDestroy()
        {
            placement.OnPaperTrigger -= SetPlaceability;
        }

        private void Update()
        {
#if UNITY_EDITOR
            if (canPlace && Input.GetMouseButtonUp(0))
            {
                OnRelease();
            }
#endif

#if UNITY_ANDROID
            if (canPlace && (touch && Input.touchCount == 0))
            {
                OnRelease();
            }

            touch = Input.touchCount != 0;
        }

#endif

        private void OnRelease()
        {
            SetPlaceability(false);
            paperToPlace.SetDraggability(false);

            float timeTillFinish = animator.Finish();
            StartCoroutine(PopupPaper(timeTillFinish));
        }

        private IEnumerator PopupPaper(float time)
        {
            if (time == 0.0f)
            {
                yield break;
            }

            LerpValue<Vector3> popup = new LerpValue<Vector3>(Vector3.one, popupScale, time);
            while (popup.Continue())
            {
                paperToPlace.transform.localScale = Vector3.Lerp(popup.start, popup.end, popup.perc);
                yield return null;
            }
        }

        private IEnumerator MoveTowardsPlacementWithResult(PlacementResult result)
        {
            LerpValue<Vector3> move = new LerpValue<Vector3>(paperToPlace.transform.position, placement.transform.position, placeTime, LerpSettings.Cosine);
            LerpValue<Vector3> popdown = new LerpValue<Vector3>(paperToPlace.transform.localScale, Vector3.one, placeTime);
            while (move.Continue() && popdown.Continue())
            {
                paperToPlace.transform.position = Vector3.Lerp(move.start, move.end, move.perc);
                paperToPlace.transform.localScale = Vector3.Lerp(popdown.start, popdown.end, popdown.perc);
                yield return null;
            }

            OnPlacement(result);
        }

        private void OnPlacement(PlacementResult result)
        {
            feedbackObject.SetActive(true);
            txtHeader.text = result.succesfull ? succesHead : failHead;
            txtMessage.text = result.message;

            MusicPlayer.Instance.PlaySFXSound(result.succesfull ? SFXSound.MinigameCorrect : SFXSound.MinigameFailed);

            if (animator.Setting.MinigameMode)
            {
                //set difficulty played as completed
                animator.Setting.SetCurrentDifficultyCompleted();

                //add reward according to minigame difficulty if the player succeded the game
                if (result.succesfull) rewardCollection.AddRewardThroughDifficulty(rewardName, setting.Difficulty);
            }
            else
            {
                //if not in minigame mode update properties
                mailProperty.OrdenMail(MailPlacing);

                //Add bronze reward since in normal mode
                rewardCollection.AddReward(rewardName, RewardType.Bronze);

                if (result.succesfull)
                {
                    happiness.AddValue(happinessOnSucces);
                }

                //mark game as available in minigame mode
                animator.Setting.SetIsCompletedInStoryMode();

                // and update quests
                if (createbankAccountQuest.IsUpdatable)
                {
                    DoOnceTask orderMailTask = createbankAccountQuest.GetTask<DoOnceTask>("BankPostOrdering");
                    orderMailTask.SetDoneOnce();
                    PlayerPropertyManager.Instance.GetProperty<DigiDProperty>("DigiD").UpdateAccountValue(true, false);
                }
            }

            //printing rewardcollection for testing purposes
            rewardCollection.PrintRewardCollection();
        }

        private void SetPlaceability(bool enter)
        {
            placement.SetActive(enter);
            canPlace = enter;
        }
    }
}