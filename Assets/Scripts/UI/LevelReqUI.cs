using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelReqUI : MonoBehaviour
{
    public GameObject ReqUIChildPrefab;
    public Transform reqUIPanel;
    private List<GameObject> activeUIElements = new List<GameObject>();

    private void Start()
    {
        RefreshUI();
    }

    public void RefreshUI()
    {
        ClearUI(); // Önce eski UI elemanlarýný temizle

        LevelData currentLevelData = LevelLoader.instance.GetCurrentLevelData();
        if (currentLevelData == null) return;

        // Item Gereksinimleri
        foreach (var item in currentLevelData.winCondition.requiredItems)
        {
            int currentAmount = InventoryManager.Instance.GetItemCount(item.itemName);
            CreateRequirementUI(item.itemName, currentAmount, item.count, "Item");
        }

        // Hayvan Gereksinimleri
        foreach (var animal in currentLevelData.winCondition.requiredAnimals)
        {
            int currentAmount = InventoryManager.Instance.GetAnimalCount(animal.animalName);
            CreateRequirementUI(animal.animalName, currentAmount, animal.count, "Animal");
        }

        // Altýn Gereksinimi
        if (currentLevelData.winCondition.requiredGold > 0)
        {
            int currentAmount = MoneyManager.instance.currentMoney;
            CreateRequirementUI("Gold", currentAmount, currentLevelData.winCondition.requiredGold, "Gold");
        }
    }

    private void CreateRequirementUI(string requirementName, int currentAmount, int requiredAmount, string type)
    {
        GameObject reqItem = Instantiate(ReqUIChildPrefab, reqUIPanel);
        activeUIElements.Add(reqItem);

        if(type == "Item")
        {
            reqItem.transform.GetChild(0).GetComponent<Image>().sprite = ItemManager.Instance.GetItemDataByName(requirementName).itemIcon;
        }
        else if (type == "Animal")
        {
            reqItem.transform.GetChild(0).GetComponent<Image>().sprite = AnimalManager.Instance.GetAnimalByName(requirementName).animalIcon;
        }
        else if(type == "Gold")
        {
            reqItem.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/GoldIcon");
        }

        reqItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"{currentAmount}/{requiredAmount}";
        
    }

    private void ClearUI()
    {
        foreach (GameObject obj in activeUIElements)
        {
            Destroy(obj);
        }
        activeUIElements.Clear();
    }
}
