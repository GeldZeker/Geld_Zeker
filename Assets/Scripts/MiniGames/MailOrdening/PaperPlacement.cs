using UnityEngine;

namespace GameStudio.GeldZeker.MiniGames.MailOrdering
{
    public class PaperPlacement : MonoBehaviour
    {
        public event PaperTriggerEvent OnPaperTrigger;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetActive(bool value)
        {
            spriteRenderer.enabled = value;
        }

        public void SetSprite(Sprite sprite)
        {
            spriteRenderer.sprite = sprite;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            OnPaperTrigger(true);
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            OnPaperTrigger(false);
        }

        public delegate void PaperTriggerEvent(bool enter);
    }
}