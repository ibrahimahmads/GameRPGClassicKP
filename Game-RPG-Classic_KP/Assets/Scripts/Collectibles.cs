using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectibles : MonoBehaviour
{
    public enum ItemType{Coin,Potion};

    public ItemType itemType;

    private ItemManager itemManager;
    
    void Awake()
    {
        itemManager = FindObjectOfType<ItemManager>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            switch (itemType)
            {
                case ItemType.Coin:
                    itemManager.AddItems(ItemType.Coin);
                    break;
                case ItemType.Potion:
                    itemManager.AddItems(ItemType.Potion);
                    break;
            }
            Destroy(gameObject);
        }
    }
}
