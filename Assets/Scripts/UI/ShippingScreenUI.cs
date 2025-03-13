using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using TMPro.Examples;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryPanel : MonoBehaviour
{
    [SerializeField] private Transform itemPanel;
    [SerializeField] private List<Transform> truckGroup;
    private Truck truck;

    public GameObject itemCardPrefab;
    private Dictionary<ItemData, int> addedItems = new Dictionary<ItemData, int>();
    public Button sendButton;

    private void Start()
    {
        
        truck = FindAnyObjectByType<Truck>();       
        sendButton.onClick.RemoveAllListeners();
        sendButton.onClick.AddListener(() => OnSellButtonPressed());
        
    }

    public void UpdateInventoryUI()
    {
        foreach (var item in InventoryManager.Instance.inventory)
        {
            
            GameObject newCard = Instantiate(itemCardPrefab, itemPanel);
            newCard.transform.GetChild(0).GetComponent<Image>().sprite = item.Key.itemIcon;
            newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = item.Key.itemName;
            newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = item.Value.ToString();
            
            newCard.GetComponent<Button>().onClick.RemoveAllListeners();
            newCard.GetComponent<Button>().onClick.AddListener(() => OnAddButtonPressed(item.Key)); 
            
        }
    
    }
    private void UpdateTruckUI()
    {
        if (addedItems.Count == 0)
        {
            return;
        }

        int itemsToDisplay = Mathf.Min(addedItems.Count, truckGroup.Count);

        for (int i = 0; i < itemsToDisplay; i++)
        {
            var item = addedItems.ElementAt(i);
                if (item.Value > 0)
            {
                foreach (Transform child in truckGroup[i])
                {
                    child.gameObject.SetActive(true);
                }

                truckGroup[i].GetChild(0).gameObject.GetComponent<Image>().sprite = item.Key.itemIcon;
                truckGroup[i].GetChild(1).gameObject.GetComponent<TextMeshProUGUI>().text = "x" + item.Value.ToString();
                Button removeButton = truckGroup[i].GetComponent<Button>();

                ItemData currentItem = item.Key;
                removeButton.onClick.RemoveAllListeners();
                removeButton.onClick.AddListener(() => onRemoveButtonPressed(currentItem));
            }
        }
    }
    private void ResetAllUI()
    {
        foreach (Transform child in itemPanel)
        {
            Destroy(child.gameObject);
        }
        foreach (Transform child in truckGroup)
        {
            foreach (Transform k in child)
            {
                k.gameObject.SetActive(false);
            }
        }
    }



    #region Buttons
    private void onRemoveButtonPressed(ItemData item)
    {
        addedItems[item]--;
        InventoryManager.Instance.AddItemToInventory(item.baseName);
        if (addedItems[item] == 0)
        {
            addedItems.Remove(item);
        }
        
        ResetAllUI();
        UpdateInventoryUI();
        UpdateTruckUI();
    }

    public void OnCloseButtonPressed()
    {
        for(int i = 0;i < addedItems.Count; i++)
        {
            if(addedItems.ElementAt(i).Key != null)
            {
                var item = addedItems.ElementAt((int)i);
                for(int j = 0; j<item.Value; j++)
                {
                    InventoryManager.Instance.AddItemToInventory(item.Key.itemName);
                }
            }
        }

        addedItems.Clear();

        ResetAllUI();
        UIAnimator.Instance.HideUI(gameObject, 0.3f);
        UIAnimator.Instance.HideBackground();
        
    }

    public void OnAddButtonPressed(ItemData item)
    {

        
        if (addedItems.Count >= truckGroup.Count)
        {
            if (!addedItems.ContainsKey(item))
            {
                Debug.Log("Truck slots are full. Cannot add more items.");
                return; 
            }
        }


        if (addedItems.ContainsKey(item))
        {
            if (addedItems[item] < 10)
            {
                addedItems[item]++;
            }
            else
            {
                Debug.Log("Maximum stack size reached for this item.");
                return; 
            }
        }
        else
        {
           
            addedItems.Add(item, 1);
        }

    
        InventoryManager.Instance.RemoveItemFromInventory(item.baseName);

      
        ResetAllUI();
        UpdateInventoryUI();
        UpdateTruckUI();
    }

    public void OnSellButtonPressed()
    {
        
            int amount = 0;
            for (int i = addedItems.Count - 1; i >= 0; i--)
            {
                if (addedItems.ElementAt(i).Value > 0)
                {
                    
                    amount += addedItems.ElementAt(i).Key.sellPrice * addedItems.ElementAt(i).Value;
                    addedItems.Remove(addedItems.ElementAt(i).Key);
                }
            }
        
        UIAnimator.Instance.HideUI(gameObject, 0.3f);
        UIAnimator.Instance.HideBackground();

        UpdateInventoryUI();
        UpdateTruckUI();
        ResetAllUI();
        

        truck.StartDelivery(amount);
    }

    
    #endregion
}