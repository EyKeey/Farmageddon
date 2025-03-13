using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MergeRecipes
{
    public List<string> inputItems;
    public string result;
    public float productionTime;
    public Dictionary<string, float> rarityChances;
}

[System.Serializable] 
public class MergeRecipeList
{
    public List<MergeRecipes> recipes;
}