using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Interaction
{
    /// <summary>Class representing an image that can be interacted with and eaten.</summary>
    [RequireComponent(typeof(Button))]
    public class EdibleImage : AudableButton
    {
        [Header("Edibility Settings")]
        [SerializeField]
        private GroceryType groceryType = GroceryType.Bread;

        [SerializeField]
        private GroceryProperty groceryProperty = null;

        [SerializeField]
        private TamagotchiElementProperty happy = null;

        [SerializeField]
        private TamagotchiElementProperty hunger = null;

        [SerializeField]
        private int happyGain = 5;

        [SerializeField]
        private int hungerGain = 10;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(Consume);
        }

        protected override void OnDestroy()
        {
            base.Awake();

            button.onClick.RemoveListener(Consume);
        }

        private void Consume()
        {
            groceryProperty.RemoveGrocery(groceryType);
            happy.AddValue(happyGain);
            hunger.AddValue(hungerGain);
            gameObject.SetActive(groceryProperty.HasGrocery(groceryType));
        }
    }
}