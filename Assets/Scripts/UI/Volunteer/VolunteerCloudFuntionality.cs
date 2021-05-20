using Assets.Scripts.Player.Properties;
using Assets.Scripts.SceneTransitioning;
using GameStudio.GeldZeker.MiniGames;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.UI.CellPhone;
using GameStudio.GeldZeker.UI.Navigation;
using GameStudio.GeldZeker.Utilities;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.UI.Volunteer
{
    public class VolunteerCloudFuntionality : AudableButton
    {
        [Header("Button settings")]
        [SerializeField]
        private VolunteerType volunteerType= VolunteerType.None;

        [Header("Button settings")]
        [SerializeField]
        private PlayerVolunteerProperty volunteerProporty = null;

        private void SetVolunteerType()
        {
            volunteerProporty.LoadFromFile();
            volunteerProporty.SetVolunteerName(volunteerType);

            Debug.Log("Set to: " + volunteerType);
        }

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(SetVolunteerType);
        }
        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(SetVolunteerType);
        }
        public void SetInteractable(bool value)
        {
            button.interactable = value;
        }
    }
}
