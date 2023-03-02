using UnityEngine;

public class Item
{
    public string name;
    public string description;
    public int prize;
    public GameObject item;
    public bool isBought;

    public Item(string name, string description, int prize, GameObject item)
    {
        this.name = name;
        this.prize = prize;
        this.item = item;
        if (!PlayerPrefs.HasKey("Item" + name))
        {
            PlayerPrefs.SetInt("Item" + name, 0);
        }
        else
        {   
            isBought = PlayerPrefs.GetInt("Item" + name) == 1;
        }
    }

    public void BuyItem()
    {
        PlayerPrefs.SetInt("Item" + name, 1);
        PlayerPrefs.Save();
        isBought = true;
    }

    public void ExpiredItem()
    {
        PlayerPrefs.SetInt("Item" + name, 0);
        PlayerPrefs.Save();
        isBought = false;
    }
}