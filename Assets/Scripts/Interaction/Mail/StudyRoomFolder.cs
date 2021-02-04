using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.Interaction.Mail
{
    public class StudyRoomFolder : MonoBehaviour
    {
        [SerializeField]
        private GameObject btnMiniGame = null;

        [SerializeField]
        private GameObject mailObject = null;

        [SerializeField]
        private PlayerMailProperty mailProperty = null;

        [SerializeField]
        private Introduction introduction = null;

        private void Start()
        {
            if (mailProperty.HasMailToOrden)
            {
                StartFolderIntro();
            }
            else
            {
                btnMiniGame.SetActive(false);
                mailObject.SetActive(false);
            }
        }

        private void StartFolderIntro()
        {
            if (!introduction.Finished)
            {
                introduction.Start();
            }
        }
    }
}