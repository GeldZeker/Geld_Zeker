using UnityEngine;
using UnityEngine.UI;

namespace GameStudio.GeldZeker.UI.CellPhone
{
    public class PlayerMoney : MonoBehaviour
    {
        [SerializeField]
        private Text moneyText = null;

        [SerializeField]
        private PlayerMoneyProperty playerMoneyProperty = null;

        // Start is called before the first frame update
        void Start()
        {
            UpdateMoneyText();
        }

        // Update is called once per frame
        void Update()
        {
            UpdateMoneyText();
        }

        /// <summary> Method to update the money text inside the bank application. </summary>
        private void UpdateMoneyText()
        {
            moneyText.text = playerMoneyProperty.Money.ToString();
        }

        /// <summary> Method to deposit money on the players bank account. Currently adding 10 euros each time the button is clicked. Can be changed in the future. </summary>
        public void DepositMoney()
        {
            playerMoneyProperty.AddMoney(10.0);
            //Debug.Log("Er is 10 euro gestort op de rekening, huidig bedrag: " + playerMoneyProperty.Money);
            UpdateMoneyText();
        }

        /// <summary> Method to withdraw money from the players bank account. </summary>
        public void WithdrawMoney()
        {
            playerMoneyProperty.RemoveMoney(10.0);
            //Debug.Log("Er is 10 euro afgeschreven van de rekening, huidig bedrag: " + playerMoneyProperty.Money);
            UpdateMoneyText();
        }

        /// <summary> Method to withdraw money from the players bank account. Used for payment scene in supermarket. </summary>
        public void WithdrawMoney(double moneyToWithdraw)
        {
            playerMoneyProperty.RemoveMoney(moneyToWithdraw);
            //Debug.Log("Er is 10 euro afgeschreven van de rekening, huidig bedrag: " + playerMoneyProperty.Money);
            UpdateMoneyText();
        }
    }
}