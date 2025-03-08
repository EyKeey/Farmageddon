using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    public static ItemLoader Instance;

    public List<ItemData> allItems = new List<ItemData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        LoadItems();
    }

    private void LoadItems()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/items");

        if (jsonFile != null)
        {
            ItemList itemList = JsonConvert.DeserializeObject<ItemList>(jsonFile.text);

            foreach(var  item in itemList.items)
            {
                item.itemPrefab = LoadItemPrefab(item.itemPrefabName);
                item.itemIcon = LoadItemIcon(item.itemIconName);
            
                allItems.Add(item);
            }

        }
    
    }

    private Sprite LoadItemIcon(string itemIconName)
    {
        Sprite icon = Resources.Load<Sprite>("Icons/" + itemIconName);

        if (icon != null)
        {
            return icon;
        }
        else
        {
            Debug.LogError("Item Icon not found:" + itemIconName);
            return null;
        }
    }

    private GameObject LoadItemPrefab(string itemPrefabName)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" +  itemPrefabName);
        if (prefab != null)
        {
            return prefab;
        }
        else
        {
            Debug.LogError("Item Prefab not found:" + itemPrefabName );
            return null;
        }
    }
}
