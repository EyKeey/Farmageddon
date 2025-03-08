using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SeedType
{
    public string seedName;
    public string producedItem;
    public float growthTime;
    public float dropChance;
    public float producitonTime;

    public Sprite seedIcon;
}

[System.Serializable]
public class SeedList
{
    public List<SeedType> seeds;
}