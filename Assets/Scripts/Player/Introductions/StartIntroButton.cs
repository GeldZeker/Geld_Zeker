using GameStudio.GeldZeker.Utilities;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Introductions
{
    public class StartIntroButton : AudableButton
    {
        [Header("Introduction Settings")]
        [SerializeField]
        private Introduction introToStart = null;

        [SerializeField]
        private bool forceStart = true;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(StartIntroduction);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(StartIntroduction);
        }

        public void SetIntroduction(Introduction newIntroduction)
        {
            introToStart = newIntroduction;
        }

        private void StartIntroduction()
        {
            if (forceStart)
            {
                introToStart.Restore();
            }
            introToStart.Start();
        }
    }
}