using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeLoader : MonoBehaviour
{
    public static RecipeLoader Instance;
 
    private Dictionary<List<string>, MergeRecipes> recipeDictionary = new Dictionary<List<string>, MergeRecipes>();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        LoadRecipes();
    }

    private void LoadRecipes()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/recipes");
        if (jsonFile != null)
        {

            MergeRecipeList recipeList = JsonConvert.DeserializeObject<MergeRecipeList>(jsonFile.text);
            foreach (var recipe in recipeList.recipes)
            {
 
                recipe.inputItems.Sort();
                recipeDictionary[recipe.inputItems] = recipe;

            }

            Debug.Log("Recipes loaded.");
        }
        else
        {
            Debug.LogError("Recipes json not found.");
        }
    }

    public MergeRecipes GetMatchingRecipe(List<string> inputItems)
    {
        inputItems.Sort();
        
        foreach(var entry in recipeDictionary)
        {
            if(AreListsEqual(entry.Key, inputItems))
            {
                
                return entry.Value;
            }
        }

        return null;
    }

    public bool AreListsEqual(List<string> list1, List<string> list2)
    {
        
        Debug.Log(list2);

        if(list1.Count != list2.Count) return false;

        for (int i = 0; i < list1.Count; i++)
        {
            if( list1[i] != list2[i]) return false;
        }

        return true;
    }

}
