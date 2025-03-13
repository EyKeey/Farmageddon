using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Worm : MonoBehaviour, IInteractable
{
    public GameObject seedPrefab;
    
    
    public void Interact()
    {
        OnWormCatched();
    }

    public void OnWormCatched()
    {
        SeedType randomSeed = SeedLoader.Instance.GetRandomSeed();
        Debug.Log(randomSeed.seedName);

        GameObject droppedSeed = Instantiate(seedPrefab, transform.position, Quaternion.identity);
        droppedSeed.GetComponent<Seed>().SeedType = randomSeed;

        Destroy(gameObject);
    }
}
