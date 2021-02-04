using System.Collections.Generic;
using UnityEngine;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>A list respresenting the player's grocery items.</summary>
    [CreateAssetMenu(menuName = "Player/Properties/Groceries")]
    public class GroceryProperty : PlayerProperty
    {
        [SerializeField]
        private List<GroceryType> groceries = null;

        [SerializeField]
        private GroceryPrice[] prices = null;

        public List<GroceryType> Groceries
        {
            get { return groceries; }
        }

        /// <summary>Returns whether this property contains a grocery of given type</summary>
        public bool HasGrocery(GroceryType groceryType)
        {
            foreach (var grocery in groceries)
            {
                if (grocery == groceryType)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>Returns the price of given grocery type for this grocery property</summary>
        public double GetPrice(GroceryType groceryType)
        {
            for (int i = 0; i < prices.Length; i++)
            {
                GroceryPrice price = prices[i];
                if (price.Type == groceryType)
                {
                    return price.Value;
                }
            }

            return 0.0d;
        }

        /// <summary>Updates the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void UpdateGroceries(List<GroceryType> groceries, bool fromSaveFile = false)
        {
            this.groceries = groceries;

            if (!fromSaveFile)
            {
                SaveToFile();
            }
        }

        /// <summary>Adds the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void AddGrocery(GroceryType groceryType)
        {
            groceries.Add(groceryType);
            SaveToFile();
        }

        /// <summary>Adds all groceries to groceries property</summary>
        public void AddGroceries(GroceryType[] groceryTypes)
        {
            foreach (GroceryType grocery in groceryTypes)
            {
                groceries.Add(grocery);
            }

            SaveToFile();
        }

        /// <summary>Remove the value of this property, only set fromSaveFile flag to true when called when called after loading it from a file</summary>
        public void RemoveGrocery(GroceryType groceryType)
        {
            for (int i = groceries.Count - 1; i >= 0; i--)
            {
                if (groceries[i] == groceryType)
                {
                    groceries.RemoveAt(i);
                    break;
                }
            }
            SaveToFile();
        }

        /// <summary>Resets the value of this property and the achievements attached to it</summary>
        public override void Restore()
        {
            groceries.Clear();
            SaveToFile();

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        /// <summary>Saves value to local storage</summary>
        protected override void SaveToFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(GroceryProperty)}/{name}";
            GameFileSystem.SaveToFile(path, groceries);
        }

        /// <summary>Loads value from local storage</summary>
        public override void LoadFromFile()
        {
            string path = $"{FOLDER_NAME}/{nameof(GroceryProperty)}/{name}";
            if (GameFileSystem.LoadFromFile(path, out List<GroceryType> outValue))
            {
                UpdateGroceries(outValue, true);
            }
        }

        [System.Serializable]
        private struct GroceryPrice
        {
#pragma warning disable 0649
            public GroceryType Type;
            public double Value;
#pragma warning restore 0649
        }
    }

    public enum GroceryType { Bread, Dairy, Fruit }
}