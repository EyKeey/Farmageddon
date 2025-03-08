using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenAnimalMenuButton : MonoBehaviour
{
    public Transform animalMenu;
    private Button openMenuButton;
    private bool isShopOpened = false;

    void Start()
    {
        openMenuButton = GetComponent<Button>();
    
        openMenuButton.onClick.RemoveAllListeners();
        openMenuButton.onClick.AddListener(() => OnShopButtonClick());
    }

    private void OnShopButtonClick()
    {
        isShopOpened = !isShopOpened;

        if (isShopOpened)
        {
            UIAnimator.Instance.ShowUI(animalMenu.gameObject, 0.2f);
        }
        else
        {
            UIAnimator.Instance.HideUI(animalMenu.gameObject, 0.2f);
        }
    }

    
}
