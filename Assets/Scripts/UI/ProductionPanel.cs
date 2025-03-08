using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProductionPanel : MonoBehaviour
{
    public static ProductionPanel Instance;

    public Transform itemsPanel;
    public Transform mergeSlots;
    public Transform mergeResultSlot;
    public GameObject mergeButtonPanel;
    public GameObject mergeProgressUI;

    public List<ItemData> addedItems = new List<ItemData>();

    public GameObject itemCardPrefab;
    private ItemData resultItem;
    public bool isResultSlotAvailable;

    private float startTime;
    private float endTime;


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    
        isResultSlotAvailable = true;
    }

    private void Update()
    {
        mergeButtonPanel.SetActive(addedItems.Count > 1);

        if (!isResultSlotAvailable)
        {
            float remainingTime = endTime - Time.time;
            if (remainingTime <= 0)
            {
                FinishCountdown();
            }
            else
            {
                UpdateTimerText(remainingTime);
            }
        }
    }

    private void FinishCountdown()
    {
        isResultSlotAvailable = true;
        mergeResultSlot.GetChild(1).gameObject.SetActive(false);
        mergeResultSlot.GetComponent<Button>().enabled = true;
    }

    private void UpdateTimerText(float remainingTime)
    {
        int minutes = Mathf.FloorToInt(remainingTime / 60);
        int seconds = Mathf.FloorToInt(remainingTime % 60);
        mergeResultSlot.GetChild(1).GetComponentInChildren<TextMeshProUGUI>().text = $"{minutes}:{seconds:D2}";
    }

    public void UpdateItemsPanel()
    {
        // Önce paneli temizle
        foreach (Transform child in itemsPanel)
        {
            Destroy(child.gameObject);
        }

        InventoryManager inventoryManager = InventoryManager.Instance;
        if (inventoryManager == null)
        {
            Debug.LogError("Inventory Manager not found.");
            return;
        }

        foreach (var item in inventoryManager.inventory)
        {
            if (item.Key == null || itemCardPrefab == null) continue; // Hatalarý önlemek için null kontrolü

            GameObject card = Instantiate(itemCardPrefab, itemsPanel);
            Image icon = card.transform.GetChild(0).GetComponent<Image>();
            TextMeshProUGUI countText = card.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            Button cardButton = card.GetComponent<Button>();

            if (icon != null) icon.sprite = item.Key.itemIcon;
            if (countText != null) countText.text = item.Value.ToString();
            if (cardButton != null)
            {
                cardButton.onClick.RemoveAllListeners();
                cardButton.onClick.AddListener(() => AddItemToSlot(item.Key));
            }
        }
    }

    private void UpdateSlots()
    {
        // Mevcut slotlarý temizle
        foreach (Transform slot in mergeSlots)
        {
            slot.GetChild(0).gameObject.SetActive(false);
            slot.GetComponent<Button>().onClick.RemoveAllListeners();
        }

        // Eklenen öðeleri slotlara yerleþtir
        for (int k = 0; k < addedItems.Count && k < mergeSlots.childCount; k++)
        {
            ItemData item = addedItems[k];
            Transform slot = mergeSlots.GetChild(k);

            slot.transform.GetChild(0).gameObject.SetActive(true);
            slot.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;

            Button slotButton = slot.GetComponent<Button>();
            if (slotButton != null)
            {
                slotButton.onClick.RemoveAllListeners();
                slotButton.onClick.AddListener(() => RemoveItemFromSlot(item));
            }
        }
    }

    public void UpdateResultPanel(string itemName, float productionTime)
    {
        addedItems.Clear();
        ItemData item = ItemManager.Instance.GetItemDataByName(itemName);
        resultItem = item;

        startTime = Time.time;
        endTime = Time.time + productionTime;
        isResultSlotAvailable = false;

        mergeResultSlot.GetChild(0).gameObject.SetActive(true);
        mergeResultSlot.GetChild(0).GetComponent<Image>().sprite = item.itemIcon;
        
        mergeResultSlot.GetComponent<Button>().onClick.RemoveAllListeners();
        mergeResultSlot.GetComponent<Button>().onClick.AddListener(()=> OnResultButtonClicked(item));

        mergeResultSlot.GetComponent<Button>().enabled = false;
        mergeResultSlot.GetChild(1).gameObject.SetActive(true);

        UpdateItemsPanel();
        UpdateSlots();
    }    

    public void AddItemToSlot(ItemData item)
    {
        if (addedItems.Count >= mergeSlots.childCount)
        {
            Debug.Log("Merging slots are full!");
            return;
        }

        addedItems.Add(item);
        InventoryManager.Instance?.RemoveItemFromInventory(item.itemName);

        UpdateItemsPanel();
        UpdateSlots();
    }

    public void RemoveItemFromSlot(ItemData item)
    {
        if (addedItems.Contains(item))
        {
            addedItems.Remove(item);
            InventoryManager.Instance?.AddItemToInventory(item.itemName);
        }

        UpdateItemsPanel();
        UpdateSlots();
    }

    public void OnMergeButtonClicked()
    {

        if (isResultSlotAvailable)
        {
            StartCoroutine(MergeManager.instance.TryMerge(addedItems));
        }
        else
        {
            Debug.Log("Üretim haznesi dolu.");
        }
    }

    public void OnCloseButtonClicked()
    {
        UIAnimator.Instance?.HideUI(gameObject, 0.3f);
        UIAnimator.Instance.HideBackground();
    }

    public void OnResultButtonClicked(ItemData item)
    {
        InventoryManager.Instance.AddItemToInventory(item.itemName);
        mergeResultSlot.GetChild(0).gameObject.SetActive(false);

        UpdateItemsPanel();
    }

    
}
