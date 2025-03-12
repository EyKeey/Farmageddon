using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.VFX;
using static Cinemachine.DocumentationSortingAttribute;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(Instance);
        }

        //BUILD EDERKEN SILINECEK -ilk  defa a��l�yormu� gibi davranmas� i�in var
        //PlayerPrefs.DeleteAll();

        if (!PlayerPrefs.HasKey("FirstLaunch")) 
        {
            Debug.Log("Oyun ilk defa a��l�yor!");

            PlayerPrefs.SetInt("FirstLaunch", 1); 
            PlayerPrefs.SetInt("CurrentLevel", 1); 
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Oyun daha �nce a��lm��!");
        }

    }

    private void Start()
    {
        
    }

    private void Update()
    {
        
    }

    public void LevelCompleted()
    {
        PauseGame();
        float elapsedTime = Timer.Instance.elapsedTime;
        LevelData levelInfo = LevelLoader.instance.GetCurrentLevelData();
        StarTimes starTimes =   levelInfo.starTimes;

        int stars = VictoryManager.instance.CalculateStars(elapsedTime, starTimes);
        VictoryManager.instance.Victory(stars);

        if (!levelInfo.isLevelCompleted)
        {
            LevelLoader.instance.MarkLevelAsCompleted(levelInfo.level, stars);
            PlayerPrefs.SetInt("CurrentLevel", levelInfo.level + 1);
        }
        else
        {
            //E�er �ncekinden daha �ok y�ld�z al�nd�ysa y�ld�zlar� g�ncelle
            if (levelInfo.currentStars > stars)
            {
                levelInfo.currentStars = stars;
            }
        }
    }

    public IEnumerator SetLevel(int money, string[] animals)
    {
        yield return new WaitUntil(() => MoneyManager.instance != null);

        
        //set money
        MoneyManager.instance.currentMoney = money;
        //spawn animals
        foreach (string animal in animals)
        {
            SpawnManager.instance.SpawnMob(animal);
        }

        Time.timeScale = 1.0f;
    }
    
    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void ContinueGame()
    {
        Time.timeScale = 1f;
    }



    public void OnMenuClick()
    {
        LevelLoader.instance.LoadMainScene();
    }
}
