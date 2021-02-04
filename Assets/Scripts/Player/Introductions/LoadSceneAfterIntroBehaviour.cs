using GameStudio.GeldZeker.SceneTransitioning;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.Player.Introductions
{
    public class LoadSceneAfterIntroBehaviour : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string nameOfScene = string.Empty;

        [SerializeField]
        private string nameOfTransition = "Fade";

        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

        [Header("Introduction")]
        [SerializeField]
        private Introduction introduction = null;

        private void Start()
        {
            if (IntroductionManager.Instance.IsActive)
            {
                introduction.OnFinish += OnIntroFinished;
            }
        }

        private void OnDestroy()
        {
            introduction.OnFinish -= OnIntroFinished;
        }

        private void OnIntroFinished(Introduction introduction)
        {
            if (!string.IsNullOrEmpty(nameOfScene))
            {
                SceneTransitionSystem.Instance.Transition(nameOfTransition, nameOfScene, loadSceneMode);
            }
        }
    }
}