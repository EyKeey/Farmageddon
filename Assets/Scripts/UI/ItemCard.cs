using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemCard : MonoBehaviour
{
    public ItemData itemData;
    public Image background;
    public Image icon;
    public TextMeshProUGUI quantity;
    public Button bttn;

    public Dictionary<string, Color> colors = new Dictionary<string, Color>
    {
        { "Uncommon", new Color(0.9921f, 0.9215f, 0.8156f) },
        { "Common", new Color(0.3450f, 0.8392f, 0.5529f) },
        { "Rare", new Color(0.2039f, 0.5960f, 0.8588f) },
        { "Epic", new Color(0.5568f, 0.26f, 0.6784f) },
        { "Legendary", new Color(0.9019f, 0.4941f, 0.13f) }
    };

    

    public void Start()
    {
        background = transform.GetChild(0).GetComponent<Image>();
        icon = transform.GetChild(1).GetComponent<Image>();
        quantity = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        bttn = GetComponent<Button>();

        bttn.onClick.RemoveAllListeners();
        bttn.onClick.AddListener(() => ProductionPanel.Instance.AddItemToSlot(itemData));

        SetColor();
        SetIcon();
        SetQuantity();
    }

    private void SetQuantity()
    {
        if (InventoryManager.Instance != null)
        {
            if (InventoryManager.Instance.inventory.ContainsKey(itemData))
            {
                quantity.text = "x" + InventoryManager.Instance.inventory[itemData].ToString();
            }
        }
    }

    private void SetColor()
    {
        string rarity = itemData.rarity;

        background.color = colors[rarity];
        
    }

    private void SetIcon()
    {
        icon.sprite = itemData.itemIcon;
    }


}
