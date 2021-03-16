using GameStudio.GeldZeker.Player;
using GameStudio.GeldZeker.Player.Properties;
using UnityEngine;

[CreateAssetMenu(menuName = "Player/Properties/Money")]
public class PlayerMoneyProperty : PlayerProperty
{
    [SerializeField]
    public static PlayerMoneyProperty instance;

    private double money;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    private void Start()
    {
        LoadFromFile();
    }

    public double Money
    {
        get
        {
            return money;
        }
    }

    /// <summary> Method to add money to the players bank account. </summary>
    public void AddMoney(double moneyToAdd)
    {
        money += moneyToAdd;
        SaveToFile();
    }

    /// <summary> Method to remove money from the players bank account. </summary>
    public void RemoveMoney(double moneyToRemove)
    {
        if (money - moneyToRemove < 0)
        {
            //Debug.Log("Te weinig geld!");
        }
        else
        {
            money -= moneyToRemove;
            SaveToFile();
        }
    }

    /// <summary> Method used to restore the players money to standard begin value of 100 euro. </summary>
    public override void Restore()
    {
        money = 100.00;
        SaveToFile();
        LoadFromFile();
    }

    /// <summary> Method used to save player money to a local storage. </summary>
    protected override void SaveToFile()
    {
        string path = $"{FOLDER_NAME}/Money/{name}";
        GameFileSystem.SaveToFile(path, money);
    }

    /// <summary> Method used to load player money from local storage. </summary>
    public override void LoadFromFile()
    {
        string path = $"{FOLDER_NAME}/Money/{name}";

        if (GameFileSystem.LoadFromFile(path, out double savedMoney))
        {
            money = savedMoney;
        }
        else
        {
            Restore();
        }
    }
}
