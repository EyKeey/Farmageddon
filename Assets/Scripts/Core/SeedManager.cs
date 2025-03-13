using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedManager : MonoBehaviour
{
    public static SeedManager instance;
    
    public List<SeedType> currentSeeds;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void AddSeedToInventory(SeedType seed)
    {
        Debug.Log(seed.seedName + "toplandý.");
        currentSeeds.Add(seed);
    }

    public void RemoveSeedFromInventory(SeedType seed)
    {
        if (currentSeeds.Contains(seed))
        {
            currentSeeds.Remove(seed);
        }
    }

}
