using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField]
    private double money;
    [SerializeField]
    private Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        money = 100.00;
        moneyText.text = money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Method to add money to the banking account
    public void addMoney()
    {
        money += 10.00;
        moneyText.text = money.ToString();
        Debug.Log("10 euro bijgeschreven");
    }

    // Method to remove money from the banking account
    public void removeMoney()
    {
        // Check if player has enough money
        if (money - 10.00 < 0)
        {
            Debug.Log("Niet genoeg geld op de bank!");
        } else
        {
            money -= 10.00;
            moneyText.text = money.ToString();
            Debug.Log("10 euro afgeschreven");
        }
    }
}
