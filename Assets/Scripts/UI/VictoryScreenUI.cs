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
        Debug.Log(stars);
        int starScore = stars;

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
