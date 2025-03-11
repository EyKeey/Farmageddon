using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnergyManager : MonoBehaviour
{
    public static EnergyManager instance;

    public int maxEnergy = 100;
    public int currentEnergy;
    public int energyPerLevel = 20;
    public float rechargeDuration = 300f; // 5 dakika = 300 saniye

    private DateTime lastQuitTime;
    private DateTime lastEnergyGivenTime;

    public TextMeshProUGUI energyText;
    public TextMeshProUGUI timerText;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);


        LoadEnergy();
        CheckOfflineEnergy();
        UpdateEnergyUI();
    }

    private void Update()
    {
        if (currentEnergy < maxEnergy)
        {
            UpdateTimerUI();
        }
        else
        {
            timerText.text = "Full Energy!";
        }
    }

    // Oyuncu level ba�lat�nca enerji azalt
    public bool UseEnergyForLevel()
    {
        if (currentEnergy >= energyPerLevel)
        {
            currentEnergy -= energyPerLevel;
            SaveEnergy();

            if (currentEnergy < maxEnergy && lastEnergyGivenTime == DateTime.MinValue)
                lastEnergyGivenTime = DateTime.Now;

            UpdateEnergyUI();
            return true; // Level ba�layabilir
        }
        else
        {
            Debug.Log("Yeterli enerji yok!");
            return false; // Yetersiz enerji
        }
    }

    // Offline enerji hesab� (oyun a��l�rken)
    private void CheckOfflineEnergy()
    {
        if (PlayerPrefs.HasKey("LastQuitTime"))
        {
            string lastTimeString = PlayerPrefs.GetString("LastQuitTime");
            DateTime lastTime = DateTime.Parse(lastTimeString);
            TimeSpan timePassed = DateTime.Now - lastTime;

            int energyToAdd = Mathf.FloorToInt((float)timePassed.TotalSeconds / rechargeDuration);
            currentEnergy = Mathf.Min(currentEnergy + energyToAdd, maxEnergy);

            // Son eklenen zaman, e�er hala max de�ilse hesaplan�r
            if (currentEnergy < maxEnergy)
            {
                float remainingSeconds = (float)timePassed.TotalSeconds % rechargeDuration;
                lastEnergyGivenTime = DateTime.Now.AddSeconds(-remainingSeconds);
            }
            else
            {
                lastEnergyGivenTime = DateTime.MinValue; // Tam doluysa s�f�rla
            }

            SaveEnergy();
        }
    }

    // Enerji UI g�ncelle
    private void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = currentEnergy.ToString() + "/" + maxEnergy.ToString();
        }
    }

    // Timer UI g�ncelle (ka� saniye kald�)
    private void UpdateTimerUI()
    {
        if(timerText == null)
        {
            return;
        }

        if (lastEnergyGivenTime == DateTime.MinValue)
        {
            lastEnergyGivenTime = DateTime.Now;
        }

        TimeSpan timeSinceLast = DateTime.Now - lastEnergyGivenTime;
        float remainingSeconds = rechargeDuration - (float)timeSinceLast.TotalSeconds;

        if (remainingSeconds <= 0f)
        {
            currentEnergy++;
            lastEnergyGivenTime = DateTime.Now;
            SaveEnergy();
            UpdateEnergyUI();

            if (currentEnergy >= maxEnergy)
            {
                timerText.text = "Full!";
                lastEnergyGivenTime = DateTime.MinValue;
                return;
            }

            remainingSeconds = rechargeDuration; // Yeniden ba�la
        }

        TimeSpan t = TimeSpan.FromSeconds(remainingSeconds);
        timerText.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
    }

    // Enerji kaydet
    private void SaveEnergy()
    {
        PlayerPrefs.SetInt("Energy", currentEnergy);
        if (lastEnergyGivenTime != DateTime.MinValue)
            PlayerPrefs.SetString("LastEnergyGivenTime", lastEnergyGivenTime.ToString());

        PlayerPrefs.Save();
    }

    private void LoadEnergy()
    {
        if (PlayerPrefs.HasKey("Energy"))
        {
            currentEnergy = PlayerPrefs.GetInt("Energy");
        }
        else
        {
            // �lk kez a��l�yorsa maxEnergy ile ba�la ve kaydet
            currentEnergy = maxEnergy;
            PlayerPrefs.SetInt("Energy", currentEnergy);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("LastEnergyGivenTime"))
            lastEnergyGivenTime = DateTime.Parse(PlayerPrefs.GetString("LastEnergyGivenTime"));
        else
            lastEnergyGivenTime = DateTime.MinValue;
    }


    // Oyundan ��karken zaman� kaydet
    private void OnApplicationQuit()
    {
        PlayerPrefs.SetString("LastQuitTime", DateTime.Now.ToString());
        PlayerPrefs.Save();
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {
            PlayerPrefs.SetString("LastQuitTime", DateTime.Now.ToString());
            PlayerPrefs.Save();
        }
    }
}
