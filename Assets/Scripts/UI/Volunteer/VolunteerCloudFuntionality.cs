using Assets.Scripts.Player.Properties;
using GameStudio.GeldZeker.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.UI.Volunteer
{
    public class VolunteerCloudFuntionality : MonoBehaviour
    {
        [SerializeField]
        private VolunteerType name = VolunteerType.None;

        [SerializeField]
        private PlayerVolunteerProperty volunteerP = null;

        public void SetVolunteerType(VolunteerType type)
        {
            volunteerP.LoadFromFile();
            volunteerP.SetVolunteerName(type);
            Debug.Log("type set!!!");
        }
    }
}
