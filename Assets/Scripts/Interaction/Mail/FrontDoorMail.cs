using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Interaction.Mail
{
    [RequireComponent(typeof(Button), typeof(ShakeableObject))]
    public class FrontDoorMail : MonoBehaviour
    {
        [Header("Load Settings")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        [Header("Project References")]
        [SerializeField]
        private PlayerMailProperty mailProperty = null;

        [SerializeField]
        private Introduction introduction = null;

        private Button button;
        private ShakeableObject shakable;

        private void Awake()
        {
            shakable = GetComponent<ShakeableObject>();
            button = GetComponent<Button>();

            if (mailProperty.HasUnPickedUpMail)
            {
                shakable.Initiate();
                TryStartIntroduction();
            }
            else
            {
                gameObject.SetActive(false);
            }

            button.onClick.AddListener(PickupMail);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(PickupMail);
        }

        private void TryStartIntroduction()
        {
            if (!introduction.Finished)
            {
                introduction.Start();
            }
        }

        /// <summary>Sets all mail to be picked up, deactivates this gameobject and loads the living room scene</summary>
        private void PickupMail()
        {
            mailProperty.PickUpMail();
            gameObject.SetActive(false);
            SceneTransitionSystem.Instance.Transition(SceneTransitionSystem.DefaultTransition, nameOfScene, LoadSceneMode.Additive);
        }
    }
}