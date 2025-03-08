using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProductionButton : MonoBehaviour
{
    public GameObject productionPanel;
    private Button bttn;
    public Transform darkBackground;

    private void Start()
    {
        bttn = GetComponent<Button>();

        bttn.onClick.RemoveAllListeners();
        bttn.onClick.AddListener(() => onProductionButtonClick());

        UIAnimator.Instance.darkBackground = darkBackground;
    }

    private void onProductionButtonClick()
    {
        
        UIAnimator.Instance.ShowUI(productionPanel, 0.3f);
        UIAnimator.Instance.ShowBackground();
        productionPanel.GetComponent<ProductionPanel>().UpdateItemsPanel();
        ProductionPanel.Instance.UpdateItemsPanel();
    }
}
