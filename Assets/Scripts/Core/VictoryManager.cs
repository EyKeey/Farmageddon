using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryManager : MonoBehaviour
{
    public static VictoryManager instance;

    public Transform victoryScreen;
 

    public bool isLevelCompleted = false;

    private void Update()
    {
        
    }


    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void CheckVictory()
    {
        LevelData currentLevelData = LevelLoader.instance.GetCurrentLevelData();
        

        if(CheckAnimalCondition(currentLevelData) &&
            CheckItemCondition(currentLevelData) &&
            CheckGoldCondition(currentLevelData)
            || isLevelCompleted)
        {
            GameManager.Instance.LevelCompleted();
        }

        FindAnyObjectByType<LevelReqUI>().RefreshUI();
    }

    #region Conditions

    
    public bool CheckItemCondition(LevelData currentLevelData)
    {
        
        foreach (var item in currentLevelData.winCondition.requiredItems)
        {
            int animalCountInInventory = InventoryManager.Instance.GetItemCount(item.itemName);

            if (animalCountInInventory < item.count)
            {
                return false;
            }

        }

        return true;
    }

    public bool CheckAnimalCondition(LevelData currentLevelData)
    {
        
        foreach (var animal in currentLevelData.winCondition.requiredAnimals)
        {
            int animalCountInInventory = InventoryManager.Instance.GetAnimalCount(animal.animalName);


            if (animalCountInInventory < animal.count)
            {
                return false;
            }
        
        }

        return true;
        
    }

    public bool CheckGoldCondition(LevelData currentLevelData)
    {
        if(MoneyManager.instance.currentMoney < currentLevelData.winCondition.requiredGold)
        {
            return false;
        }
        else
        {

            return true;
        }

    }

    #endregion

    #region Stars

    public int CalculateStars(float elapsedTime, StarTimes starTimes)
    {
        if (elapsedTime <= starTimes.threeStars)
        {
            return 3;
        }
        else if (elapsedTime <= starTimes.twoStars)
        {
            return 2;
        }
        else
        {
            return 1;
        }
    }

    #endregion


    public void Victory(int stars)
    {
        Canvas canvas = FindAnyObjectByType<Canvas>();
        if (canvas != null)
        {
            victoryScreen.gameObject.GetComponent<VictoryScreenUI>().UpdateStars(stars);
            UIAnimator.Instance.ShowUI(victoryScreen.gameObject, 0.3f);
            UIAnimator.Instance.ShowBackground();
        }

    }
}
