using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;

    [SerializeField] public List<AnimalData> allAnimals;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

    }

    private void Start()
    {
        GetAnimalList();
    }

    private void GetAnimalList()
    {
        List<AnimalData> list = AnimalLoader.Instance.allAnimals;

        foreach (AnimalData data in list)
        {
            allAnimals.Add(data);
        }
    }

    public AnimalData GetAnimalByName(string animalName)
    {
       
        foreach (AnimalData data in allAnimals)
        {
            if(data.animalName == animalName)
            {
                return data;
            }
        }
        
        return null;
    }
    
}
