using BWolf.Utilities.PlayerProgression.Quests;
using GameStudio.GeldZeker.Player.Properties;
using GameStudio.GeldZeker.Utilities;
using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.Interaction.Supermarket
{
    /// <summary>Button for groceries that can be selected/bought.</summary>
    [RequireComponent(typeof(Button))]
    public class AddGroceryButton : AudableButton
    {
        [Header("Grocery Settings")]
        [SerializeField]
        private GroceryType groceryType = GroceryType.Bread;

        [SerializeField]
        private ShoppingTrolley shoppingTrolley = null;

        [SerializeField]
        private GroceryProperty grocerySupermarketProperty = null;

        [SerializeField]
        private Quest boodschappenPakkenQuest = null;

        protected override void Awake()
        {
            base.Awake();

            button.onClick.AddListener(AddGrocery);

            gameObject.SetActive(!grocerySupermarketProperty.HasGrocery(groceryType));
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            button.onClick.RemoveListener(AddGrocery);
        }

        private void AddGrocery()
        {
            /// <summary>Finish grocery fetching task.</summary>
            if (boodschappenPakkenQuest.IsUpdatable && !boodschappenPakkenQuest.IsCompleted)
            {
                DoOnceTask grabGrocery = boodschappenPakkenQuest.GetTask<DoOnceTask>("1KeerBoodschappenPakken");
                grabGrocery.SetDoneOnce();
            }
            grocerySupermarketProperty.AddGrocery(groceryType);
            gameObject.SetActive(false);
            shoppingTrolley.FillShoppingTrolley();
        }
    }
}