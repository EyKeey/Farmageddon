using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int level;
    public int gold;
    public string[] animals;
    public bool isLevelCompleted;
    public int currentStars;

    public WinCondition winCondition;
    public StarTimes starTimes;

}

[System.Serializable]
public class LevelList
{
    public LevelData[] levels;
}

[System.Serializable]
public class WinCondition
{
    public List<ItemRequirement> requiredItems;
    public List<AnimalRequirement> requiredAnimals;
    public int catchedBears;
    public int requiredGold;
}

[System.Serializable]
public class ItemRequirement
{
    public string itemName;
    public int count;
}

[System.Serializable]
public class AnimalRequirement
{
    public string animalName;
    public int count;
}

[System.Serializable]
public class StarTimes
{
    public float threeStars;
    public float twoStars;
    public float oneStar;  
}