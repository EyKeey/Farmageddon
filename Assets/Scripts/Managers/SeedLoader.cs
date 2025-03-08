using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public class SeedLoader : MonoBehaviour
{
    public static SeedLoader Instance;

    public List<SeedType> allSeeds = new List<SeedType>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadSeeds();
    }

    private void LoadSeeds()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/seeds");

        if (jsonFile != null)
        {
            SeedList seedList = JsonConvert.DeserializeObject<SeedList>(jsonFile.text);

            foreach (var seed in seedList.seeds)
            {
                allSeeds.Add(seed);
            }
        }
    }

    public SeedType GetSeedInfo(string seedName)
    {
        SeedType seedType = allSeeds.Find(seed => seed.seedName == seedName);

        if (seedType != null)
        {
            return seedType;
        }

        return null;

    }

    public SeedType GetRandomSeed()
    {
        Random rand = new Random();
        double totalWeight = 0;

        
        foreach (var seed in allSeeds)
            totalWeight += seed.dropChance;

        if (totalWeight == 0) return null; 

        double randomValue = rand.NextDouble() * totalWeight;
        double cumulative = 0;

        foreach (var seed in allSeeds)
        {
            cumulative += seed.dropChance;
            if (randomValue <= cumulative)
                return seed;
        }

        return null;
    }
}
