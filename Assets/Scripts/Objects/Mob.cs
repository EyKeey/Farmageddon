using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : MonoBehaviour
{
    public string animalName;

    public void Die()
    {
        InventoryManager.Instance.RemoveAnimalFromAnimals(animalName);

        Destroy(gameObject);
    }
}
