using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelReqUI : MonoBehaviour
{
    public GameObject ReqUIChildPrefab;
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
            CreateRequirementUI(item.itemName, currentAmount, item.count);
        }

        // Hayvan Gereksinimleri
        foreach (var animal in currentLevelData.winCondition.requiredAnimals)
        {
            int currentAmount = InventoryManager.Instance.GetAnimalCount(animal.animalName);
            CreateRequirementUI(animal.animalName, currentAmount, animal.count);
        }

        // Ayý Yakalama Þartý
        if (currentLevelData.winCondition.catchedBears > 0)
        {
            int currentAmount = InventoryManager.Instance.catchedBears;
            CreateRequirementUI("Bear", currentAmount, currentLevelData.winCondition.catchedBears);
        }

        // Altýn Gereksinimi
        if (currentLevelData.winCondition.requiredGold > 0)
        {
            int currentAmount = MoneyManager.instance.currentMoney;
            CreateRequirementUI("Gold", currentAmount, currentLevelData.winCondition.requiredGold);
        }
    }

    private void CreateRequirementUI(string requirementName, int currentAmount, int requiredAmount)
    {
        GameObject reqItem = Instantiate(ReqUIChildPrefab, transform);
        activeUIElements.Add(reqItem);

        reqItem.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = requirementName;
        reqItem.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = $"{currentAmount}/{requiredAmount}";
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
