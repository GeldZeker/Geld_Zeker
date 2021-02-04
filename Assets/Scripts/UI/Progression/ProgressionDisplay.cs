using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.Progression
{
    public class ProgressionDisplay : MonoBehaviour
    {
        [SerializeField]
        private Text txtDescription = null;

        [SerializeField]
        private Toggle toggleCompleted = null;

        public string description
        {
            set { txtDescription.text = value; }
        }

        public bool completed
        {
            set { toggleCompleted.isOn = value; }
        }

        public void SetActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}