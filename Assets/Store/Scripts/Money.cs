using UnityEngine;

public class Money
{
    public int money; 
    public Money()
    {
        if (PlayerPrefs.HasKey("Money"))
        {
            money = PlayerPrefs.GetInt("Money");
        }
        else
        {
            money = 0;
        }
    }

    public void AddMoney(int amount)
    {
        money += amount;
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

    public void RemoveMoney(int amount)
    {
        money -= amount;
        PlayerPrefs.SetInt("Money", money);
        PlayerPrefs.Save();
    }

}