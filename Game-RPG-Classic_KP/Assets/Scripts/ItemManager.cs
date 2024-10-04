using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemManager : MonoBehaviour
{
    public TextMeshProUGUI coinText;
    public TextMeshProUGUI potionText;

    private int coinCount =0;
    private int potionCount =0;

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

}
