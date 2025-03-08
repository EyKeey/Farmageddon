using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalLoader : MonoBehaviour
{
    public static AnimalLoader Instance;

    public List<AnimalData> allAnimals = new List<AnimalData>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        LoadAnimals();
    }
    
    private void LoadAnimals()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/animals");

        if (jsonFile != null)
        {
            AnimalList animalList = JsonConvert.DeserializeObject<AnimalList>(jsonFile.text);

            foreach(var animal in animalList.animals)
            {
                animal.animalPrefab = LoadAnimalPrefab(animal.animalPrefabName);
                animal.animalIcon = LoadAnimalIcon(animal.animalIconName);

                allAnimals.Add(animal);
            }

        }
        else 
        {
            Debug.LogError("Json file not found.");
        }
    }

    private Sprite LoadAnimalIcon(string animalIconName)
    {
        Sprite icon = Resources.Load<Sprite>("Icons/" + animalIconName);

        if (icon != null)
        {
            return icon;

        }
        else
        {
            Debug.LogError("Animal Icon not found:" + animalIconName);
            return null;
        }

    }

    private GameObject LoadAnimalPrefab(string animalPrefabName)
    {
        GameObject prefab = Resources.Load<GameObject>("Prefabs/" + animalPrefabName);

        if(prefab != null)
        {
            return prefab;
        }
        else
        {
            Debug.LogError("Animal prefab not found:" + animalPrefabName);
            return null;
        }
            
    
    }
}
