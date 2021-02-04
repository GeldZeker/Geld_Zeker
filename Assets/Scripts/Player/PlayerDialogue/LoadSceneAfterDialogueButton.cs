using GameStudio.GeldZeker.SceneTransitioning;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.Player.PlayerDialogue
{
    /// <summary>A player dialogue utility script for loading a scene after dialogue has finished</summary>
    public class LoadSceneAfterDialogueButton : DialogueStartButton
    {
        [Header("Load Settings")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        [SerializeField]
        private string nameOfTransition = "Fade";

        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

        protected override void OnDialogueFinish()
        {
            if (!string.IsNullOrEmpty(nameOfScene))
            {
                button.interactable = false;
                SceneTransitionSystem.Instance.Transition(nameOfTransition, nameOfScene, loadSceneMode);
            }
        }
    }
}