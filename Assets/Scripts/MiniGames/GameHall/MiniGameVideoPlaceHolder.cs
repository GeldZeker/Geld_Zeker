using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace GameStudio.GeldZeker.MiniGames.GameHall
{
    /// <summary>A behaviour for representing a placeholder for a video that needs to be loaded</summary>
    [RequireComponent(typeof(Image))]
    public class MiniGameVideoPlaceHolder : MonoBehaviour
    {
        [SerializeField]
        private VideoPlayer videoPlayer = null;

        private Image placeholderImage = null;

        private void Awake()
        {
            placeholderImage = GetComponent<Image>();
        }

        private void Start()
        {
            if (videoPlayer.isPrepared)
            {
                print(videoPlayer.isPlaying);
                placeholderImage.enabled = false;
            }
            else
            {
                videoPlayer.started += OnStart;
            }
        }

        private void OnDestroy()
        {
            videoPlayer.started -= OnStart;
        }

        private void OnStart(VideoPlayer source)
        {
            placeholderImage.enabled = false;
        }
    }
}