using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager instance;

    public int currentMoney;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncreaseMoney(int amount)
    {
        currentMoney += amount;
    }

    public bool DecreaseMoney(int amount)
    {
        if (currentMoney > amount)
        {
            currentMoney -= amount;
            return true;
        }
        else
        {
            return false;
        }
    }
    
}
