using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemLoader : MonoBehaviour
{
    public static ItemLoader Instance;

    public List<ItemData> allItems = new List<ItemData>();

    private const string ITEM_JSON_PATH = "Json/Items";
    private const string PREFAB_PATH = "Prefabs/";
    private const string ICON_PATH = "Icons/";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);


        InitItems();
    }

    public void InitItems()
    {
        if (allItems.Count == 0)
            LoadItems();
    }

    private void LoadItems()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(ITEM_JSON_PATH);

        if (jsonFile != null)
        {
            ItemList itemList = JsonConvert.DeserializeObject<ItemList>(jsonFile.text);

            foreach(var  item in itemList.items)
            {
                item.itemPrefab = LoadResource<GameObject>(PREFAB_PATH, item.itemPrefabName);
                item.itemIcon = LoadResource<Sprite>(ICON_PATH, item.itemIconName);

                allItems.Add(item);
            }

        }
    
    }

    private T LoadResource<T>(string path, string name) where T : UnityEngine.Object
    {
        T resource = Resources.Load<T>(path + name);
        if (resource == null)
            Debug.LogError(typeof(T).Name + " not found: " + name);
        return resource;
    }

}
