using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryScreenUI : MonoBehaviour
{
    public Transform Stars;


    public void UpdateStars(int stars)
    {
        int starScore = 2;

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
