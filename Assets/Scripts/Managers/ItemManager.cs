using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public List<ItemData> allItems = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(Instance);

    }

    private void Start()
    {
        allItems = ItemLoader.Instance.allItems;    
        Debug.Log(allItems.Count);
    }

    public string NewRegisterWithRarity(string itemName, string rarity)
    {
        ItemData itemData = GetItemDataByName(itemName);

        if (itemData == null)
        {
            Debug.LogError($"Item '{itemName}' not found!");
            return null;
        }

        string newItemName = rarity + " " + itemName;


        ItemData existingItem = allItems.Find(item => item.baseName == newItemName);
        if (existingItem != null)
        {
            return newItemName;
        }

        ItemData newItem = new ItemData
        {
            baseName = newItemName,
            itemID = itemData.itemID,
            sellPrice = itemData.sellPrice,
            storageCost = itemData.storageCost,
            rarity = rarity,
            itemName = itemData.itemName,
            itemDescription = itemData.itemDescription,
            itemPrefab = itemData.itemPrefab,
            itemIcon = itemData.itemIcon
        };

        allItems.Add(newItem);

        return newItemName;
    }


    public ItemData GetItemDataByName(string name)
    {
        foreach (ItemData item in allItems)
        {
            if (item.baseName.Equals(name))
            {
                return item;
            }
        }

        return null;
    }
}
