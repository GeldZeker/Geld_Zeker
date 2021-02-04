using BWolf.Behaviours.SingletonBehaviours;
using GameStudio.GeldZeker.SceneTransitioning;
using GameStudio.GeldZeker.UI;
using GameStudio.GeldZeker.UI.Navigation;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameStudio.GeldZeker.Player.Properties
{
    /// <summary>Singleton class for managing the player's properties</summary>
    public class PlayerPropertyManager : SingletonBehaviour<PlayerPropertyManager>
    {
        [Header("Supermarket Reset")]
        [SerializeField]
        private string nameOfSupermarketParent = "FruitDepartment";

        [SerializeField]
        private string nameOfPaymentScene = "DebitCardPayGame";

        [SerializeField]
        private string nameOfGroceryProperty = "SupermarketGroceries";

        [Header("Properties")]
        [SerializeField]
        private PlayerProperties propertiesAsset = null;

        protected override void Awake()
        {
            base.Awake();

            if (!isDuplicate)
            {
                propertiesAsset.LoadFromFile();
            }
        }

        private void Start()
        {
            SceneTransitionSystem.Instance.TransitionStarted += OnTransitionStarted;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (SceneTransitionSystem.Instance != null)
            {
                SceneTransitionSystem.Instance.TransitionStarted -= OnTransitionStarted;
            }
        }

        private void OnTransitionStarted(string nameOfSceneLoading, LoadSceneMode mode)
        {
            string nameOfActiveScene = SceneManager.GetActiveScene().name;
            ParentScene supermarketScene = MainCanvasManager.Instance.GetParentScene(nameOfSupermarketParent);

            bool isNotLoadingSupermarket = supermarketScene.name != nameOfSceneLoading && !supermarketScene.ContainsChild(nameOfSceneLoading) && nameOfSceneLoading != nameOfPaymentScene;
            bool isInSupermarket = supermarketScene.name == nameOfActiveScene || supermarketScene.ContainsChild(nameOfActiveScene) || nameOfActiveScene == nameOfPaymentScene;
            if (isNotLoadingSupermarket && isInSupermarket)
            {
                propertiesAsset.GetProperty<GroceryProperty>(nameOfGroceryProperty).Restore();
            }
        }

        public T GetProperty<T>(string name) where T : PlayerProperty
        {
            return propertiesAsset.GetProperty<T>(name);
        }

        public PlayerProperty GetProperty(string name)
        {
            return propertiesAsset.GetProperty(name);
        }

        [ContextMenu("ResetProgression")]
        public void ResetProgression()
        {
            propertiesAsset.Restore();
        }
    }
}