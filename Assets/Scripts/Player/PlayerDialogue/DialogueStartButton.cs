using BWolf.Utilities.CharacterDialogue;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.PlayerDialogue
{
    public class DialogueStartButton : AudableButton
    {
        [Header("Dialogue Settings")]
        [SerializeField]
        private Dialogue dialogue = null;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(StartDialogue);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(StartDialogue);
        }

        private void StartDialogue()
        {
            MainCanvasManager.Instance.StartDialogue(dialogue, OnDialogueFinish);
        }

        protected virtual void OnDialogueFinish()
        {
        }
    }
}