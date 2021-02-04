using GameStudio.GeldZeker.Utilities;

namespace GameStudio.GeldZeker.UI.GameHall
{
    /// <summary>A button for toggling the game overview</summary>
    public class ToggleGameOverviewButton : ToggleActiveStateButton
    {
        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(ToggleMinigameModeButtons);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(ToggleMinigameModeButtons);
        }

        private void ToggleMinigameModeButtons()
        {
            MainCanvasManager.Instance.SetMinigameModeButtonState(!groupToggled);
        }
    }
}