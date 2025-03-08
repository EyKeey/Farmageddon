using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Truck : MonoBehaviour
{
    public Sprite disableButtonSprite;
    public Sprite enableButtonSprite;


    public Transform deliveryScreen;
    public bool isTruckWaitingForProduct = true;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnTruckClicked());
    }

    public void OnTruckClicked()
    {
        if (isTruckWaitingForProduct)
        {
            UIAnimator.Instance.ShowUI(deliveryScreen.gameObject, 0.3f);
            UIAnimator.Instance.ShowBackground();
            deliveryScreen.GetComponent<DeliveryPanel>().UpdateInventoryUI();
        }

    }

    public void StartDelivery(int amount)
    {
        StartCoroutine(SendTruck(amount));
    }

    public IEnumerator SendTruck(int amount)
    {
        isTruckWaitingForProduct = false;
        gameObject.GetComponent<Image>().sprite = disableButtonSprite;

        yield return new WaitForSecondsRealtime(8f);
        
        MoneyManager.instance.IncreaseMoney(amount);
        gameObject.GetComponent <Image>().sprite = enableButtonSprite;
        isTruckWaitingForProduct = true;
    }

    
}
