using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.MailOrdering
{
    [CreateAssetMenu(menuName = "Minigames/MailOrdening/FolderTab")]
    public class FolderTab : ScriptableObject
    {
        [SerializeField]
        private MailType mailType = default;

        [SerializeField]
        private PaperSprite tab = default;

        [Space]
        [SerializeField]
        private PaperSprite[] paper = null;

        public MailType MailType
        {
            get { return mailType; }
        }

        public PaperSprite Tab
        {
            get { return tab; }
        }

        public PaperSprite[] Papers
        {
            get { return paper; }
        }
    }
}