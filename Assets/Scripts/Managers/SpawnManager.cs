using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SpawnManager : MonoBehaviour
{
    public static SpawnManager instance;

    public PolygonCollider2D spawnArea;
    public GameObject gamePanel;
    
    private Bounds bounds;
    private Vector3 spawnPosition;
    private Timer timer;
    private int randomSec = -1;
    private bool waitForMinute= false;
    float nextSpawnTime = 0f;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        timer = GameObject.FindObjectOfType<Timer>();
        
        spawnArea = GameObject.FindObjectOfType<PolygonCollider2D>();
        bounds = spawnArea.bounds;
    }

    private void Update()
    {
        CheckTimeForRandomSpawn("Bear");
    }

    private void CheckTimeForRandomSpawn(string mobName)
    {
        float elapsedTime = timer.GetTime();

        if (elapsedTime >= nextSpawnTime)
        {
            if (nextSpawnTime != 0f)
            {
                SpawnMob(mobName);
            }
            // Sonraki spawn zamaný, þu anki zamandan 40-115 sn sonra olacak.
            float randomDelay = UnityEngine.Random.Range(40f, 115f);
            nextSpawnTime = elapsedTime + randomDelay;

        }
    }



    public void SpawnMob(String animalName)
    {

        AnimalData animal = AnimalManager.Instance.GetAnimalByName(animalName);


        spawnPosition.x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x);
        spawnPosition.y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y);

        GameObject prefab = animal.animalPrefab;
        GameObject newAnimal = Instantiate(prefab, spawnPosition, Quaternion.identity);
        newAnimal.transform.parent = gamePanel.transform;


        if(newAnimal.GetComponent<Mob>() != null)
        {
            InventoryManager.Instance.AddAnimalToAnimals(newAnimal.GetComponent<Mob>().animalName = animalName);
        }
        else
        {
            return;
        }

    }

}
