using Assets.Scripts.Player.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.UI.StudyRoom
{

    [RequireComponent(typeof(CanvasGroup), typeof(Image))]
    public class RewardImageLoader : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField]
        private PlayerRewardProperty rewardCollection = null;

        [SerializeField]
        private string rewardName = null;

        [Header("Reward images")]
        [SerializeField]
        private Sprite shadowReward = null;

        [SerializeField]
        private Sprite bronzeReward = null;

        [SerializeField]
        private Sprite SilverReward = null;

        [SerializeField]
        private Sprite goldReward = null;


        protected Image originalImage;

        public void Awake()
        {
            originalImage = GetComponent<Image>();
            ChangeImage();
        }

        private void ChangeImage()
        {
            RewardType rewardToSet = rewardCollection.GetHighestReward(rewardName);

            switch (rewardCollection.GetHighestReward(rewardName))
            {
                case RewardType.Bronze:
                    originalImage.sprite = bronzeReward;
                    break;
                case RewardType.Silver:
                    originalImage.sprite = SilverReward;
                    break;
                case RewardType.Gold:
                    originalImage.sprite = goldReward;
                    break;
                default:
                    originalImage.sprite = shadowReward;
                    break;
            }

            var tempColor = originalImage.color;
            tempColor.a = 1f;
            originalImage.color = tempColor;
        }
    }
}