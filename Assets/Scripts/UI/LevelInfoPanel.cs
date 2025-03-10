using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelInfoPanel : MonoBehaviour
{
    [Header("UI")]
    public TextMeshProUGUI levelIndexText;
    public Transform objectivesPanel;
    public Transform rewardsPanel;

    [Header("Prefabs")]
    public GameObject objectivesCardPrefab;
    public GameObject rewardsCardPrefab;
    
    public int levelIndex;

    private void Start()
    {
       levelIndexText.text = "Level " + levelIndex;
        UpdateObjectivesContentPanel();
    }



    #region Content

    public void UpdateObjectivesContentPanel()
    {
        LevelData level = LevelLoader.instance.GetSpesificLevelData(levelIndex);

        foreach(var objectives in level.winCondition.requiredItems)
        {
            GameObject newCard = Instantiate(objectivesCardPrefab, objectivesPanel);
            newCard.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + objectives.itemName + "Icon");
            newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = objectives.itemName;
            newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + objectives.count;
            UIAnimator.Instance.ShowUI(newCard, 0.4f);
        }
        foreach (var objectives in level.winCondition.requiredAnimals)
        {
            GameObject newCard = Instantiate(objectivesCardPrefab, objectivesPanel);
            newCard.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/" + objectives.animalName + "Icon");
            newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = objectives.animalName;
            newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + objectives.count;
            UIAnimator.Instance.ShowUI(newCard, 0.4f);
        }

        if (level.winCondition.requiredGold > 0)
        {
            GameObject newCard = Instantiate(objectivesCardPrefab, objectivesPanel);
            newCard.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("Icons/GoldIcon");
            newCard.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Gold";
            newCard.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "x" + level.winCondition.requiredGold;
            UIAnimator.Instance.ShowUI(newCard, 0.4f);
        }
    }

    public void UpdateRewardsContentPanel()
    {

    }

    #endregion



    #region Buttons

    public void OnStartButoonClicked()
    {
       
        if(EnergyManager.instance.UseEnergyForLevel())
        {
            LevelLoader.instance.LoadLevel(levelIndex);
            UIAnimator.Instance.HideBackground();
            Destroy(gameObject);
        }

    }

    public void OnCloseButtonClicked()
    {
        StartCoroutine(DestroyPanel());
    }

    public IEnumerator DestroyPanel()
    {
        UIAnimator.Instance.HideBackground();
        UIAnimator.Instance.HideUI(gameObject, 0.3f);
        yield return new WaitForSeconds(0.3f);
        Destroy(gameObject);
    }

    #endregion

}
