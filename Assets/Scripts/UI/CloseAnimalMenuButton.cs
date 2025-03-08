using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseAnimalMenuButton : MonoBehaviour
{
    public Transform animalMenu;
    private Button closeMenuButton;
    public Transform animalMenuOpenButton;

    void Start()
    {
        closeMenuButton = GetComponent<Button>();

        closeMenuButton.onClick.RemoveAllListeners();
        closeMenuButton.onClick.AddListener(() => OnCloseMenuButtonClick());
    }

    private void OnCloseMenuButtonClick()
    {
        animalMenuOpenButton.gameObject.SetActive(true);
        animalMenu.gameObject.SetActive(false);
    }
}
