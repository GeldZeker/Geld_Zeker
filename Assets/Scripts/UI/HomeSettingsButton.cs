using GameStudio.GeldZeker.Utilities;

namespace GameStudio.GeldZeker.UI
{
    /// <summary>A behaviour for managing the active state of the settings on the home screen</summary>
    public class HomeSettingsButton : AudableButton
    {
        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(ToggleGameSettings);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(ToggleGameSettings);
        }

        /// <summary>Called when the button has been clicked to toggle the settings menu active state</summary>
        private void ToggleGameSettings()
        {
            MainCanvasManager.Instance.ToggleSettings();
        }
    }
}