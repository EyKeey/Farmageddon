using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaterManager : MonoBehaviour
{
    public static WaterManager instance;

    [SerializeField] private float fullWaterAmount;
    [SerializeField] private float currentWaterAmount;
    [SerializeField] private GameObject waterButton;
    private Slider waterSlider;

    private int waterCost = 19;

    private void Start()
    {
        fullWaterAmount = 100;        
        currentWaterAmount = fullWaterAmount;
        
        waterSlider = waterButton.GetComponentInChildren<Slider>();
        waterSlider.value = 1;
    }

    public bool UseWater()
    {
        currentWaterAmount -= 20;
        waterSlider.value = currentWaterAmount / fullWaterAmount;
        if (currentWaterAmount < 0)
        {
            WaterEmpty();
            return false;
        }
        else
        {
            return true;
        }
           
        
    }

    private void Update()
    {
        
    }

    private void WaterEmpty()
    {
        //Debug.Log("no water");
    }

    public void FullWater()
    {

        if (MoneyManager.instance.DecreaseMoney(waterCost) && currentWaterAmount != fullWaterAmount)
        {
            currentWaterAmount = fullWaterAmount;
            waterSlider.value = currentWaterAmount / fullWaterAmount;
        }
        else
        {
            UIAnimator.Instance.ShowMessage("Yeterli para yok.");
        }

        
    }

}
