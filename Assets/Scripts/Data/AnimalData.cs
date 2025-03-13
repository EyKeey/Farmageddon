using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalData
{
    public string animalName;
    public string animalDescription;
    public int animalBuyPrice;
    public int animalSellPrice;

    public string animalIconName;
    public string animalPrefabName;
    
    public Sprite animalIcon;
    public GameObject animalPrefab;

}
[System.Serializable]
public class AnimalList
{
    public List<AnimalData> animals;

}
