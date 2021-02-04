using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.UI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Player.Tamagotchi
{
    /// <summary>Displays Tamagotchi Element on scene.</summary>
    public class ShowTamagotchiElements : MonoBehaviour
    {
        [Header("HapinessIcons")]
        [SerializeField]
        private Sprite maleIcon = null;

        [SerializeField]
        private Sprite femaleIcon = null;

        [Header("Elements")]
        [SerializeField]
        private Element[] elements = null;

        private Image background;

        private const float ONE_PERCENT = 0.01f;

        private void Awake()
        {
            background = GetComponent<Image>();

            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void Start()
        {
            for (int i = 0; i < elements.Length; i++)
            {
                Element element = elements[i];
                element.OnPercentageChanged(element.property.Percentage);
                element.Subscribe();
            }
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].UnSubscribe();
            }
        }

        public void SetHappinessIcon(HappinesIcon icon)
        {
            Image happinessIcon = GetIcon("Happiness");
            switch (icon)
            {
                case HappinesIcon.Male:
                    happinessIcon.sprite = maleIcon;
                    break;

                case HappinesIcon.Female:
                    happinessIcon.sprite = femaleIcon;
                    break;
            }
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            ParentScene livingRoom = MainCanvasManager.Instance.GetParentScene(NavigationSystem.NameOfLivingRoom);
            bool isAtHome = scene.name == livingRoom.name || livingRoom.ContainsChild(scene.name);

            background.enabled = isAtHome;

            for (int i = 0; i < elements.Length; i++)
            {
                elements[i].gameObject.SetActive(isAtHome);
            }
        }

        private Image GetIcon(string propertyName)
        {
            for (int i = 0; i < elements.Length; i++)
            {
                if (elements[i].property.name == propertyName)
                {
                    return elements[i].icon;
                }
            }

            return null;
        }

        [System.Serializable]
        private struct Element
        {
#pragma warning disable 0649
            public GameObject gameObject;
            public TamagotchiElementProperty property;
            public Image filler;
            public Image icon;
#pragma warning restore 0649

            public void OnPercentageChanged(int percentage)
            {
                filler.fillAmount = (ONE_PERCENT * percentage);
                if (percentage > 60)
                {
                    filler.color = Color.green;
                }
                else if (percentage > 40)
                {
                    filler.color = Color.yellow;
                }
                else
                {
                    filler.color = Color.red;
                }
            }

            public void Subscribe()
            {
                property.OnPercentageChange += OnPercentageChanged;
            }

            public void UnSubscribe()
            {
                property.OnPercentageChange -= OnPercentageChanged;
            }
        }
    }

    public enum HappinesIcon { Male, Female };
}