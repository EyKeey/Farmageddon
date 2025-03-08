using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ItemData{
    [Header("Data")]
    public string baseName;
    public int itemID;
    public int sellPrice;
    public int storageCost;
    public string rarity;
    public float productionTime;

    [Header("Graphic")]
    public string itemName;
    public string itemDescription;
    public GameObject itemPrefab;
    public Sprite itemIcon;

    public string itemPrefabName;
    public string itemIconName;
}

[System.Serializable]
public class ItemList
{
    public List<ItemData> items;
}
