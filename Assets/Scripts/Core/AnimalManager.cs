using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance;

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

    public AnimalData GetAnimalByName(string animalName)
    {
        foreach (AnimalData data in AnimalLoader.Instance.allAnimals)
        {
            if (data.animalName == animalName)
            {
                return data;
            }
        }

        Debug.LogWarning($"Animal with name '{animalName}' not found.");
        return null;
    }

    public List<AnimalData> GetAllAnimals()
    {
        return AnimalLoader.Instance.allAnimals;
    }
}
