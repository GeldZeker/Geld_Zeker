using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.UI;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Player
{
    /// <summary> Loads gender image on gameobject </summary>
    public class LoadPlayer : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField]
        private PlayerGenderProperty gender = null;

        private Image player;
        private Animator animator;

        private void Awake()
        {
            player = GetComponent<Image>();
            animator = GetComponent<Animator>();
        }

        private void Start()
        {
            switch (gender.Value)
            {
                case PlayerGender.Male:
                    SetMale();
                    MainCanvasManager.Instance.SetTamagotchiHappinessIcon(Tamagotchi.HappinesIcon.Male);
                    break;

                case PlayerGender.Female:
                    SetFemale();
                    MainCanvasManager.Instance.SetTamagotchiHappinessIcon(Tamagotchi.HappinesIcon.Female);
                    break;
            }

            if (IntroductionManager.Instance.IsActive)
            {
                IntroductionManager.Instance.IntroFinished += OnIntroFinish;
                DisableVisability();
            }
        }

        private void SetMale()
        {
            animator.SetTrigger("SetMan");
        }

        private void SetFemale()
        {
            animator.SetTrigger("SetWoman");
        }

        private void OnDestroy()
        {
            if (IntroductionManager.Instance != null)
            {
                IntroductionManager.Instance.IntroFinished -= OnIntroFinish;
            }
        }

        private void DisableVisability()
        {
            player.enabled = false;
        }

        private void OnIntroFinish(Introduction introduction)
        {
            player.enabled = true;
        }
    }
}