using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();
    public Dictionary<AnimalData, int> animals = new Dictionary<AnimalData, int>();

    public int catchedBears = 0;
    public int maxCapacity = 20;
    public int currentFullness = 0;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }


    private void Update()
    {
        
    }

    private void CalculateFullness()
    {
        currentFullness = 0;
        for (int i = 0; i < inventory.Count; i++)
        {
            int cost = inventory.ElementAt(i).Key.storageCost;
            int quantity = inventory.ElementAt(i).Value;

            currentFullness += cost * quantity; 
        }
    }

    public bool IsStorageFull(string itemName)
    {
        
        ItemData item = ItemManager.Instance.GetItemDataByName(itemName);

        int i_cost = item.storageCost;

        if (currentFullness + i_cost >= maxCapacity)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

    #region Items
    public void ReceiveItem(string itemName)
    {
        ItemData item = ItemManager.Instance.GetItemDataByName(itemName);

        AddItemToInventory(item.itemName);
    }

    public void AddItemToInventory(string itemName)
    {
        ItemData item = ItemManager.Instance.GetItemDataByName(itemName);

        if (inventory.ContainsKey(item))
        {
            inventory[item]++;
        }
        else
        {
            inventory.Add(item, 1);
        }

        CalculateFullness();


        VictoryManager.instance.CheckVictory();
    }

    public void RemoveItemFromInventory(String itemName)
    {
        ItemData item = ItemManager.Instance.GetItemDataByName(itemName);


        inventory[item]--;
        if (inventory[item] == 0)
        {
            inventory.Remove(item);
        }

        CalculateFullness();

        VictoryManager.instance.CheckVictory();
    }

    public int GetItemCount(string itemName)
    {
        foreach (var item in inventory)
        {
            if (item.Key.itemName == itemName)
            {
                return item.Value;
            }
        }

        return 0;
    }

    #endregion

    #region Animals
    public void AddAnimalToAnimals(String animalName)
    {
        AnimalData animal = AnimalManager.Instance.GetAnimalByName(animalName);

        if (animals.ContainsKey(animal))
        {
            animals[animal]++;
        }
        else
        {
            animals.Add(animal, 1);
        }

        VictoryManager.instance.CheckVictory();

    }

    public void RemoveAnimalFromAnimals(string animalName)
    {
        AnimalData animal = AnimalManager.Instance.GetAnimalByName(animalName);

        if (animals.ContainsKey(animal)) // Eðer hayvan listede varsa
        {
            animals[animal]--;

            if (animals[animal] <= 0) // Eðer sayýsý 0 veya daha az olduysa
            {
                animals.Remove(animal);
            }

            VictoryManager.instance.CheckVictory();
        }
        else
        {
            Debug.LogWarning($"Hayvan {animalName} zaten listede yok!");
        }
    }


    public int GetAnimalCount(string animalName)
    {

        foreach (var animal in animals)
        {
            if (animal.Key.animalName == animalName)
            {
                return animal.Value;
            }
        }

        return 0;
    }

    #endregion
}
