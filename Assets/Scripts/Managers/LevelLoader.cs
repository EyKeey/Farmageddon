using Newtonsoft.Json;
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

    private string savePath;
    public GameObject loadingScreen;
    public List<LevelData> levels = new List<LevelData>(); // Listeyi baþlatýyoruz.
    public int currentLevel = 1;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        savePath = Path.Combine(Application.persistentDataPath, "levels.json");
        
        //BUILD EDERKEN SILINECEK --ilk defa açýlýyormuþ gibi davranmasý için var
        //File.Delete(savePath);
        
        
        
        LoadLevelData();
    }

    private void Start()
    {
        
    }

    public void LoadLevelData()
    {
        
        if (File.Exists(savePath))
        {
            // Eðer kayýtlý JSON varsa, onu yükle
            string json = File.ReadAllText(savePath);
            Debug.Log(json);
            LevelList levelList = JsonConvert.DeserializeObject<LevelList>(json);
            foreach(var level in levelList.levels)
            {
                levels.Add(level);
            }
            Debug.Log("Kayýtlý JSON Yüklendi.");
        }
        else
        {
            //Eðer kayýtlý JSON yoksa, Resources'tan oku ve kaydet
            TextAsset jsonFile = Resources.Load<TextAsset>("Json/levels");
            if (jsonFile != null)
            {
                LevelList levelList = JsonConvert.DeserializeObject<LevelList>(jsonFile.text);
                foreach (var level in levelList.levels)
                {
                    levels.Add(level);
                }
                SaveLevelData(); // Ýlk kez çalýþýyorsa JSON'u kaydet
                Debug.Log("Varsayýlan JSON Yüklendi ve Kaydedildi.");
            }
            else
            {
                Debug.LogError("JSON dosyasý bulunamadý!");
                levels = new List<LevelData>();
            }
        }
    }

    public void SaveLevelData()
    {
        LevelList wrapper = new LevelList();
        wrapper.levels = levels;
        string json = JsonConvert.SerializeObject(wrapper, Formatting.Indented);
        File.WriteAllText(savePath, json);
        Debug.Log("Level verisi kaydedildi: " + savePath);
    }


    public void LoadLevel(int levelIndex)
    {
        currentLevel = levelIndex;

        //SceneManager.LoadScene("Level");
        StartCoroutine(LoadLevelAsync("Level"));

        LevelData levelInfo = GetCurrentLevelData();

        if (levelInfo != null && GameManager.Instance != null)
        {
            StartCoroutine(GameManager.Instance.SetLevel(levelInfo.gold, levelInfo.animals));
        }
        else
        {
            Debug.LogError("GameManager.Instance veya levelInfo null! SetLevel çaðýrýlamadý.");
        }
    }



    public void LoadMainScene()
    {
        StartCoroutine(LoadLevelAsync("Main"));
    }

    public LevelData GetCurrentLevelData()
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("Level listesi boþ! JSON dosyasý yüklenmiþ mi?");
            return null;
        }

        LevelData currentLevelData = levels.Find(level => level.level == currentLevel);

        if (currentLevelData == null)
        {
            Debug.LogError("Current level data not found for level: " + currentLevel);
        }

        return currentLevelData;
    }

    public LevelData GetSpesificLevelData(int levelIndex)
    {
        if (levels == null || levels.Count == 0)
        {
            Debug.LogError("Level listesi boþ! JSON dosyasý yüklenmiþ mi?");
            return null;
        }

        LevelData spesificLevelData = levels.Find(level => level.level == levelIndex);

        if (spesificLevelData == null)
        {
            Debug.LogError("Current level data not found for level: " + currentLevel);
        }

        return spesificLevelData;
    }

    public IEnumerator LoadLevelAsync(string sceneName)
    {
        if (loadingScreen != null)
            loadingScreen.SetActive(true);
        else
            Debug.LogWarning("Loading screen UI nesnesi atanmadý!");


        Debug.Log("Yükleme baþladý...");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false; 
        while (operation.progress < 0.9f)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            Debug.Log("Yükleme ilerlemesi: " + (progress * 100f).ToString("F0") + "%");

            if (loadingScreen != null)
            {
                Slider loadingBar = loadingScreen.GetComponentInChildren<Slider>();
                if (loadingBar != null)
                    loadingBar.value = progress;

                TextMeshProUGUI loadingText = loadingScreen.GetComponentInChildren<TextMeshProUGUI>();
                if (loadingText != null)
                    loadingText.text = "Yükleniyor... " + (progress * 100f).ToString("F0") + "%";
            }

            yield return null;
        }

        Debug.Log("Yükleme tamamlandý, sahne aktifleþtiriliyor...");
        operation.allowSceneActivation = true; // Sahneye geçiþ yap.

        yield return new WaitUntil(() => operation.isDone); // Sahne tamamen yüklenene kadar bekle.

        Debug.Log("Sahne aktifleþtirildi.");
        if (loadingScreen != null)
            loadingScreen.SetActive(false);
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
}
