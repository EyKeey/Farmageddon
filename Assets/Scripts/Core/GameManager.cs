using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
            Destroy(gameObject);
        }

        CheckFirstLaunch();
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
            
            if (levelInfo.currentStars < stars)
            {
                levelInfo.currentStars = stars;
                LevelLoader.instance.SaveLevelData();
            }
        }

        
    }

    public IEnumerator SetLevel(int money, string[] animals)
    {
        yield return new WaitUntil(() => MoneyManager.instance != null && SpawnManager.instance != null);

        //set money
        MoneyManager.instance.currentMoney = money;
        //spawn animals
        foreach (string animal in animals)
        {
            SpawnManager.instance.SpawnMob(animal);
        }

        ContinueGame();
    }
    
    public void PauseGame()
    {
        if (Time.timeScale == 0)
            return;
        else
            Time.timeScale = 0;
        
    }

    public void ContinueGame()
    {
        if (Time.timeScale == 1)
            return;
        else
            Time.timeScale = 1;

    }

    private void CheckFirstLaunch()
    {
        if (!PlayerPrefs.HasKey("FirstLaunch"))
        {
            PlayerPrefs.SetInt("FirstLaunch", 1);
            PlayerPrefs.SetInt("CurrentLevel", 1);
            PlayerPrefs.Save();
        }
    }

    public void ResetGameProgress()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("FirstLaunch", 1);
        PlayerPrefs.SetInt("CurrentLevel", 1);
        PlayerPrefs.Save();
    }

    public void OnMenuClick()
    {
        LevelLoader.instance.LoadMainScene();
    }
}
