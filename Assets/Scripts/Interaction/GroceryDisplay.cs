using GameStudio.GeldZeker.Player.Introductions;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

namespace GameStudio.GeldZeker.Interaction
{
    /// <summary>A class representing groceries to display.</summary>
    public class GroceryDisplay : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField]
        private bool fillOnIntroduction = false;

        [Header("Scene References")]
        [SerializeField]
        private GameObject bread = null;

        [SerializeField]
        private GameObject dairy = null;

        [SerializeField]
        private GameObject fruit = null;

        [Header("Project References")]
        [SerializeField]
        private GroceryProperty groceryProperty = null;

        private void Start()
        {
            if (fillOnIntroduction && IntroductionManager.Instance.IsActive)
            {
                groceryProperty.Groceries.Clear();
                groceryProperty.AddGrocery(GroceryType.Bread);
                groceryProperty.AddGrocery(GroceryType.Dairy);
                groceryProperty.AddGrocery(GroceryType.Fruit);
            }
            ShowGroceries();
        }

        private void ShowGroceries()
        {
            bread.SetActive(groceryProperty.HasGrocery(GroceryType.Bread));
            dairy.SetActive(groceryProperty.HasGrocery(GroceryType.Dairy));
            fruit.SetActive(groceryProperty.HasGrocery(GroceryType.Fruit));
        }
    }
}