using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.PlayerDialogue;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI
{
    /// <summary>A simple behaviour attachable to a button to make it load a scene to be defined in the inspector</summary>
    [RequireComponent(typeof(Button))]
    public class StartGameButton : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private string nameOfGameScene = string.Empty;

        [SerializeField]
        private string nameOfCharacterSelectScene = string.Empty;

        [SerializeField]
        private string nameOfTransition = "Fade";

        [SerializeField]
        private LoadSceneMode loadSceneMode = LoadSceneMode.Additive;

        [Header("References")]
        [SerializeField]
        private PlayerAudableCharacter playerCharacter = null;

        private Button button;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        /// <summary>Called when the button has been clicked to transition to a new scene</summary>
        private void OnClick()
        {
            TimeController.instance.BeginTimer();
            if (!string.IsNullOrEmpty(nameOfGameScene))
            {
                MusicPlayer.Instance.PlaySFXSound(SFXSound.StartButtonClick);

                PlayerGenderProperty gender = PlayerPropertyManager.Instance.GetProperty<PlayerGenderProperty>("Gender");

                string nameOfScene;
                if (gender.Value != PlayerGender.NotSelected)
                {
                    //if a gender has already been selected, try loading the last scene used
                    if (GameFileSystem.LoadFromFile(NavigationSystem.SCENE_SAVE_FILE_PATH, out string loadedSceneName))
                    {
                        nameOfScene = loadedSceneName;
                    }
                    else
                    {
                        //if no scene could be loaded, set name of scene to be loaded to name of game scene
                        nameOfScene = nameOfGameScene;
                    }

                    playerCharacter.InitFromLocalStorage();
                }
                else
                {
                    nameOfScene = nameOfCharacterSelectScene;
                }

                SceneTransitionSystem.Instance.Transition(nameOfTransition, nameOfScene, loadSceneMode);
            }
        }
    }
}