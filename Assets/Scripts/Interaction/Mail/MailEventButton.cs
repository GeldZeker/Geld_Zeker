using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Interaction.Mail
{
    /// <summary> Activates behaviour whenever the player receives mail. </summary>
    public class MailEventButton : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private CanvasGroup mailman = null;

        [SerializeField]
        private CanvasGroup letters = null;

        [SerializeField]
        private PlayerMailProperty mailProperty = null;

        [SerializeField]
        private Introduction introduction = null;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnButtonClick);
        }

        // Start is called before the first frame update
        private void Start()
        {
            SetActive(mailProperty.HasMailToSortOut);
        }

        private void SetActive(bool value)
        {
            if (value)
            {
                mailman.alpha = 1;
                letters.alpha = 1;
                letters.interactable = true;
                letters.blocksRaycasts = true;
                gameObject.SetActive(true);
                TryStartIntroduction();
            }
            else
            {
                mailman.alpha = 0;
                letters.alpha = 0;
                letters.interactable = false;
                letters.blocksRaycasts = false;
                gameObject.SetActive(false);
            }
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnButtonClick);
        }

        public void OnButtonClick()
        {
            //for now orden all mail on click instead of minigame
            SetActive(false);
        }

        private void TryStartIntroduction()
        {
            if (!introduction.Finished)
            {
                introduction.Start();
            }
        }
    }
}