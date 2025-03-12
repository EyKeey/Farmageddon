using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MergeManager : MonoBehaviour
{
    public static MergeManager instance;

    public GameObject mergeAnimationScene;
    public bool isAnimationFinished;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public IEnumerator TryMerge(List<ItemData> itemDatas)
    {
        ProductionPanel.Instance.isResultSlotAvailable = false;

        List<string> inputItems = new List<string>();
        foreach (var itemData in itemDatas)
        {
            inputItems.Add(itemData.itemName);
        }

        MergeRecipes matchingRecipes = RecipeLoader.Instance.GetMatchingRecipe(inputItems);

        if (matchingRecipes != null)
        {
            string resultItem = matchingRecipes.result;

            if (InventoryManager.Instance.IsStorageFull(resultItem)) {
                
                Debug.Log("Inventory is Full");
                yield break;
            }

            string rarity = GetRandomRarity(matchingRecipes.rarityChances);
            string newItemName = ItemManager.Instance.NewRegisterWithRarity(resultItem, rarity);
            float productionTime = matchingRecipes.productionTime;

            isAnimationFinished = false;
            
            Canvas canvas = FindAnyObjectByType<Canvas>();
            if (canvas != null)
            {
                GameObject mergeAnimation = Instantiate(mergeAnimationScene, canvas.transform);
                mergeAnimation.GetComponent<MergeAnimationScene>().PlayMergeAnimation(inputItems, newItemName);
            }

            yield return new WaitUntil(() => isAnimationFinished);

            ProductionPanel.Instance.UpdateResultPanel(newItemName, productionTime);
        }
        else
        {
            Debug.Log("No matching Recipes,");
        }
    }

    private string GetRandomRarity(Dictionary<string, float> rarityChances)
    {
        float roll = Random.value;
        float cumulative = 0;

        foreach (var rarity in rarityChances)
        {
            cumulative += rarity.Value;
            if (roll <= cumulative)
                return rarity.Key;
        }

        return "Common";

    }
}
