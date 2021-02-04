using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.Interaction.Supermarket
{
    /// <summary>Loads the Shopping Trolley items </summary>
    public class ShoppingTrolley : MonoBehaviour
    {
        [Header("References")]
        [SerializeField]
        private GroceryProperty groceryProperty = null;

        [Header("Items")]
        [SerializeField]
        private GameObject bread = null;

        [SerializeField]
        private GameObject dairy = null;

        [SerializeField]
        private GameObject fruit = null;

        // Start is called before the first frame update
        void Start() {
            FillShoppingTrolley();
        }

        public void FillShoppingTrolley() {
            foreach (var groceryType in groceryProperty.Groceries) {
                switch (groceryType) {
                    case GroceryType.Bread:
                        bread.SetActive(true);
                        break;
                    case GroceryType.Dairy:
                        dairy.SetActive(true);
                        break;
                    case GroceryType.Fruit:
                        fruit.SetActive(true);
                        break;
                }
            }
        }

    }
}