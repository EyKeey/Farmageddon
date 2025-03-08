using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MergeResultSlot : MonoBehaviour
{
    public ItemData itemData;
    public Image background;
    public Image icon;
    public Button bttn;
    public Color color;

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
        color = icon.color;
        bttn = GetComponent<Button>();
    }

    public void UpdateSlot()
    {
        SetColor();
        SetIcon();
    }

    private void SetColor()
    {
        if (itemData == null)
        {
            background.color = colors["Uncommon"];
        }
        else
        {
            string rarity = itemData.rarity;

            if (colors.ContainsKey(rarity))
            {
                background.color = colors[rarity];
            }
        }
    }

    private void SetIcon()
    {
        if (itemData == null)
        {
            icon.enabled = false;
        }
        else
        {
            icon.enabled = true;
            icon.sprite = itemData.itemIcon;
        }
    }
}
