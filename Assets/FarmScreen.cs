using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FarmScreen : MonoBehaviour
{
    public Transform inventoryPanel;
    public Transform farmSlot;
    public Transform farmResultSlot;

    public GameObject seedCardPrefab;
    public SeedType addedSeed;

    private void Awake()
    {
        farmResultSlot.gameObject.SetActive(false);
    }

    public void UpdateInventoryPanel()
    {
        foreach (var seed in SeedManager.instance.currentSeeds)
        {
            GameObject newSeed = GameObject.Instantiate(seedCardPrefab, inventoryPanel);
            newSeed.GetComponentInChildren<Image>().sprite = seed.seedIcon;
            newSeed.GetComponentInChildren<TextMeshProUGUI>().text = seed.seedName;

            Button bttn = newSeed.GetComponent<Button>();
            bttn.onClick.RemoveAllListeners();
            bttn.onClick.AddListener(() => AddSeedToFarmSlot(seed));
        }
    }

    public void UpdateFarmSlot()
    {
        if(addedSeed != null)
        {
            farmSlot.GetComponentInChildren<Image>().sprite = addedSeed.seedIcon;
        }
    }

    #region Buttons

    private void AddSeedToFarmSlot(SeedType seed)
    {
        addedSeed = seed;    
    }

    private void RemoveSeedFromFarmSlot()
    {
        addedSeed = null;
    }

    public void StartFarm()
    {
           
    }
    #endregion
}
