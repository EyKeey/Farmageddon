using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    public static LevelLoader instance;

    
    [SerializeField] private GameObject loadingScreen;

    private string savePath;
    
    public List<LevelData> levels { get; private set; } = new List<LevelData>();
    [HideInInspector] public int currentLevel = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        savePath = Path.Combine(Application.persistentDataPath, "levels.json");
        
        //BUILD EDERKEN SILINECEK --ilk defa açýlýyormuþ gibi davranmasý için var
        //File.Delete(savePath);
        
        
        
        LoadLevelData();
    }

    #region LevelLoader

    public void LoadLevelData()
    {
        
        if (File.Exists(savePath))
        {
            // If there is JSON saved, load it
            LoadFromSavedFile();
        }
        else
        {
            //If there is no JSON saved, read from Resources and save
            LoadFromResourcesAndSave();
        }
    }

    private void LoadFromResourcesAndSave()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("Json/levels");
        if (jsonFile != null)
        {
            LevelList levelList = JsonConvert.DeserializeObject<LevelList>(jsonFile.text);
            foreach (var level in levelList.levels)
            {
                levels.Add(level);
            }
            SaveLevelData();
            Debug.Log("Varsayýlan JSON Yüklendi ve Kaydedildi.");
        }
        else
        {
            Debug.LogError("JSON dosyasý bulunamadý!");
            levels = new List<LevelData>();
        }
    }

    private void LoadFromSavedFile()
    {
        string json = File.ReadAllText(savePath);
        Debug.Log(json);
        LevelList levelList = JsonConvert.DeserializeObject<LevelList>(json);
        foreach (var level in levelList.levels)
        {
            levels.Add(level);
        }
        Debug.Log("Kayýtlý JSON Yüklendi.");
    }

    #endregion

    #region LevelDataManager

    public void SaveLevelData()
    {
        LevelList wrapper = new LevelList();
        wrapper.levels = levels;
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("Level verisi kaydedildi: " + savePath);
    }

    public LevelData GetSpesificLevelData(int levelIndex)
    {

        LevelData spesificLevelData = levels.Find(level => level.level == levelIndex);

        if (spesificLevelData == null)
        {
            Debug.LogError("Current level data not found for level: " + currentLevel);
        }

        return spesificLevelData;
    }

    public LevelData GetCurrentLevelData()
    {

        LevelData currentLevelData = levels.Find(level => level.level == currentLevel);

        if (currentLevelData == null)
        {
            Debug.LogError("Current level data not found for level: " + currentLevel);
        }

        return currentLevelData;
    }

    public void MarkLevelAsCompleted(int levelIndex, int stars)
    {
        LevelData level = levels.Find(l => l.level == levelIndex);

        if (level != null)
        {
            level.isLevelCompleted = true; // Level tamamlandý olarak iþaretleniyor
            level.currentStars = stars;
            SaveLevelData(); //JSON'a kaydet
            Debug.Log("Level " + levelIndex + " tamamlandý!");
        }
        else
        {
            Debug.LogError("Tamamlanacak level bulunamadý!");
        }
    }

    #endregion

    #region SceneManager
    public void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        StartCoroutine(LoadLevelAsync("Level"));

        LevelData levelInfo = GetCurrentLevelData();

        if (levelInfo != null && GameManager.Instance != null)
        {
            StartCoroutine(GameManager.Instance.SetLevel(levelInfo.gold, levelInfo.animals));
        }
        else
        {
            Debug.LogError("GameManager.Instance or levelInfo null!");
        }
    }


    public void LoadMainScene()
    {
        StartCoroutine(LoadLevelAsync("Main"));
    }

    public IEnumerator LoadLevelAsync(string sceneName)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);
        else
            Debug.LogWarning("Loading screen UI object not assigned!");


        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; 
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            if (loadingScreen != null)
            {
                Slider loadingBar = loadingScreen.GetComponentInChildren<Slider>();
                if (loadingBar != null)
                    loadingBar.value = progress;

                TextMeshProUGUI loadingText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
                if (loadingText != null)
                    loadingText.text = "Loading... " + (progress * 100f).ToString("F0") + "%";
            }

            yield return null;
        }

        operation.allowSceneActivation = true;

        yield return new WaitUntil(() => operation.isDone);

        if (loadingScreen != null)
            loadingScreen.SetActive(false);
    }

    #endregion
}
