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

        int sec = Mathf.FloorToInt(elapsedTime % 60);


        if (randomSec == -1 && !waitForMinute)
        {
            randomSec = UnityEngine.Random.Range(20, 55);
        }
        else 
        {
            if(sec == 0)
            {
                waitForMinute = false;
            }
        }

        if(sec == randomSec)
        {
            SpawnMob(mobName);
            randomSec = -1;
            waitForMinute = true;
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
