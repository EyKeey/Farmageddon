using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class AnimalLoader : MonoBehaviour
{
    public static AnimalLoader Instance;

    public List<AnimalData> allAnimals = new List<AnimalData>();

    private const string ANIMAL_JSON_PATH = "Json/animals";
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

        LoadAnimals();
    }
    
    private void LoadAnimals()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>(ANIMAL_JSON_PATH);

        if (jsonFile != null)
        {
            AnimalList animalList = JsonConvert.DeserializeObject<AnimalList>(jsonFile.text);

            foreach(var animal in animalList.animals)
            {
                animal.animalPrefab = LoadResource<GameObject>(PREFAB_PATH, animal.animalPrefabName);
                animal.animalIcon = LoadResource<Sprite>(ICON_PATH, animal.animalIconName);

                allAnimals.Add(animal);
            }

        }
        else 
        {
            Debug.LogError("Json file not found.");
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
