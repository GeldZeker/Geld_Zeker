﻿using GameStudio.GeldZeker.Player;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMoney : MonoBehaviour
{
    [SerializeField]
    public static PlayerMoney Instance;

    [SerializeField]
    private double money;

    [SerializeField]
    private Text moneyText = null;

    [SerializeField]
    private string MONEY_PATH = "Player/Money";

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        LoadFromFile();
        moneyText.text = money.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary> Method to add money to the banking account. </summary>
    private void addMoney()
    {
        money += 10.00;
        moneyText.text = money.ToString();
        Debug.Log("10 euro bijgeschreven");
    }

    /// <summary> Method to remove money from the banking account of the player. </summary>
    private void removeMoney()
    {
        /// <summary> Check if player has enough money. </summary>
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

    /// <summary> Method used to save player money to a local storage. </summary>
    public void SaveToFile()
    {
        GameFileSystem.SaveToFile(MONEY_PATH, money);
    }

    /// <summary> Method used to load player money from local storage. </summary>
    private void LoadFromFile()
    {
        /// <summary> Check if there already exists a saved amount of money. </summary>
        if (GameFileSystem.LoadFromFile(MONEY_PATH, out double savedMoney))
        {
            money = savedMoney;
        }
        else
        {
            money = 100.00;
        }
    }
}