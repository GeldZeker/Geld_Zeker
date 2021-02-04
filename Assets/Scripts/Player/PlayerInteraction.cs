using GameStudio.GeldZeker.Audio;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Player
{
    /// <summary>Add button that plays the Players mood</summary>
    [RequireComponent(typeof(Button))]
    public class PlayerInteraction : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private TamagotchiElementProperty happiness = null;

        private Button btn;

        // Start is called before the first frame update
        void Start() {
            btn = GetComponent<Button>();
            btn.onClick.AddListener(PlayMoodSound);
        }

        private void PlayMoodSound() {
            int percentage = happiness.Percentage;
            if (percentage > 60) {
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MoodHappy);
            }
            else if (percentage > 40) {
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MoodNeutral);
            }
            else {
                MusicPlayer.Instance.PlaySFXSound(SFXSound.MoodSad);
            }
        }

        private void OnDestroy() {
            btn.onClick.RemoveListener(PlayMoodSound);
        }
    }
}