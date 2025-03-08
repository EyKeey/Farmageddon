using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AnimalButton : MonoBehaviour
{
    [SerializeField] string animalName;
    private AnimalData animal;
    [SerializeField] Image animalIcon;
    TextMeshProUGUI priceText;
    Button buyButton;

    private void Awake()
    {
        animal = AnimalManager.Instance.GetAnimalByName(animalName);
    }

    void Start()
    {
        priceText = GetComponentInChildren<TextMeshProUGUI>();
        buyButton = GetComponent<Button>();
        
        HandleUI();

        buyButton.onClick.RemoveAllListeners();
        buyButton.onClick.AddListener(() => OnBuyButtonClicked());

        

        
    }

    private void Update()
    {
        if (MoneyManager.instance != null)
        {
            if (MoneyManager.instance.currentMoney < animal.animalBuyPrice)
            {
                animalIcon.color = Color.black;
                GetComponent<Button>().enabled = false;
            }
            else
            {
                animalIcon.color = Color.white;
                GetComponent<Button>().enabled = true;
            }
        }
        else
        {
            Debug.Log("money manager not found");
        }
    }

    private void HandleUI()
    {
        animalIcon.sprite = animal.animalIcon; 
        priceText.text = animal.animalBuyPrice.ToString();
    }

    

    public void OnBuyButtonClicked()
    {
        SpawnManager spawn = GameObject.FindAnyObjectByType<SpawnManager>();

        spawn.SpawnMob(animalName);
        MoneyManager.instance.currentMoney -= animal.animalBuyPrice;

    }
}
