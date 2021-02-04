using GameStudio.GeldZeker.MiniGames.Settings;
using GameStudio.GeldZeker.Player.Introductions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameStudio.GeldZeker.UI.GameHall
{
    /// <summary>A behaviour representing a mini game start display</summary>
    public class GameStartDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private Display[] displays = null;

        [Header("References")]
        [SerializeField]
        private VideoPlayer videoPlayer = null;

        [SerializeField]
        private RawImage renderImage = null;

        [SerializeField]
        private Text txtHeader = null;

        [SerializeField]
        private Text txtSubHeader = null;

        [Header("Buttons")]
        [SerializeField]
        private StartIntroButton introButton = null;

        [Space]
        [SerializeField]
        private GameObject[] loadbuttons = null;

        private CanvasGroup group;

        private bool active;

        private void Awake()
        {
            group = GetComponent<CanvasGroup>();

            active = group.alpha != 0;
        }

        public void SetMiniGame(MiniGame miniGame)
        {
            if (GetMinigameDisplay(miniGame, out Display display))
            {
                //set video player settings
                renderImage.texture = display.renderTexture;
                videoPlayer.clip = display.videoClip;
                videoPlayer.targetTexture = display.renderTexture;
                videoPlayer.Play();

                //set text displays
                txtHeader.text = display.Header;
                txtSubHeader.text = display.SubHeader;

                //set intro button settings
                introButton.SetIntroduction(display.Intro);

                //set load button settings
                foreach (GameObject button in loadbuttons)
                {
                    LoadMiniGameButton loader = button.GetComponent<LoadMiniGameButton>();
                    loader.SetSetting(display.Setting, true);
                    loader.NameOfSceneLoading = display.Setting.NameOfScene;

                    MinigameDifficultyButton difficulty = button.GetComponent<MinigameDifficultyButton>();
                    difficulty.SetSetting(display.Setting);
                }
            }
        }

        private bool GetMinigameDisplay(MiniGame minigame, out Display display)
        {
            for (int i = 0; i < displays.Length; i++)
            {
                if (displays[i].minigame == minigame)
                {
                    display = displays[i];
                    return true;
                }
            }

            display = default;
            return false;
        }

        public void OnGameOverViewClosed()
        {
            if (active)
            {
                ToggleActiveState();
            }
        }

        public void ToggleActiveState()
        {
            active = !active;

            group.alpha = active ? 1.0f : 0.0f;
            group.interactable = active;
            group.blocksRaycasts = active;

            if (!active)
            {
                videoPlayer.Stop();

                videoPlayer.clip = null;
                videoPlayer.targetTexture = null;
                renderImage.texture = null;
            }
        }

        [System.Serializable]
        private struct Display
        {
#pragma warning disable 0649
            public MiniGame minigame;
            public VideoClip videoClip;
            public RenderTexture renderTexture;
            public string Header;
            public string SubHeader;
            public MiniGameSetting Setting;
            public Introduction Intro;
#pragma warning restore 0649
        }
    }
}