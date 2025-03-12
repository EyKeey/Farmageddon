using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenUI : MonoBehaviour
{
    public Transform Stars;

    private void Awake()
    {
    }

    public void UpdateStars(int starScore)
    {
        
        for (int i = 0; i < Stars.childCount; i++)
        {
           
            Stars.GetChild(i).gameObject.GetComponent<Image>().color = Color.black;
        }


        for (int i = 0; i < starScore; i++)
        {
            Stars.GetChild(i).gameObject.GetComponent<Image>().color = Color.white;
        }
    
    }

    public void OnClicked()
    {
        LevelLoader.instance.LoadMainScene();
    }

}
