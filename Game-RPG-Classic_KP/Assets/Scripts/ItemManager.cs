using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI potionText;

    public int coinCount =0;
    public int potionCount =0;

    public int CoinCount => coinCount; // Properti untuk mengambil nilai koin
    public int PotionCount => potionCount; 

    void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItems(Collectibles.ItemType itemType)
    {
        switch (itemType)
        {
            case Collectibles.ItemType.Coin:
                int coinsToAdd = Random.Range(1, 16); // Tambah 1-15 koin
                coinCount += coinsToAdd;
                coinText.text = coinCount.ToString(); // Update UI
                break;
            case Collectibles.ItemType.Potion:
                potionCount ++; // Tambah 1 potion
                potionText.text = potionCount.ToString(); // Update UI
                break;
        }
    }

    public void ResetItems()
    {
        coinCount = 0;
        potionCount = 0;
        coinText.text = coinCount.ToString();
        potionText.text = potionCount.ToString();
    }

    public void AddCoins(int amount)
    {
        coinCount += amount;
        coinText.text = coinCount.ToString();
    }

    public void AddPotions(int amount)
    {
        potionCount += amount;
        potionText.text = potionCount.ToString();
    }


}
