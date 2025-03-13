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
    public float rechargeDuration = 300f; 

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

    public bool UseEnergyForLevel()
    {
        if (currentEnergy >= energyPerLevel)
        {
            currentEnergy -= energyPerLevel;
            SaveEnergy();

            if (currentEnergy < maxEnergy && lastEnergyGivenTime == DateTime.MinValue)
                lastEnergyGivenTime = DateTime.Now;

            UpdateEnergyUI();
            return true;
        }
        else
        {
            Debug.Log("Yeterli enerji yok!");
            return false; 
        }
    }


    private void CheckOfflineEnergy()
    {
        if (PlayerPrefs.HasKey("LastQuitTime"))
        {
            string lastTimeString = PlayerPrefs.GetString("LastQuitTime");
            DateTime lastTime = DateTime.Parse(lastTimeString);
            TimeSpan timePassed = DateTime.Now - lastTime;

            int energyToAdd = Mathf.FloorToInt((float)timePassed.TotalSeconds / rechargeDuration);
            currentEnergy = Mathf.Min(currentEnergy + energyToAdd, maxEnergy);

            if (currentEnergy < maxEnergy)
            {
                float remainingSeconds = (float)timePassed.TotalSeconds % rechargeDuration;
                lastEnergyGivenTime = DateTime.Now.AddSeconds(-remainingSeconds);
            }
            else
            {
                lastEnergyGivenTime = DateTime.MinValue; 
            }

            SaveEnergy();
        }
    }

    public void UpdateEnergyUI()
    {
        if (energyText != null)
        {
            energyText.text = currentEnergy.ToString() + "/" + maxEnergy.ToString();
        }
    }

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

            remainingSeconds = rechargeDuration;
        }

        TimeSpan t = TimeSpan.FromSeconds(remainingSeconds);
        timerText.text = $"{t.Minutes:D2}:{t.Seconds:D2}";
    }


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
            currentEnergy = maxEnergy;
            PlayerPrefs.SetInt("Energy", currentEnergy);
            PlayerPrefs.Save();
        }

        if (PlayerPrefs.HasKey("LastEnergyGivenTime"))
            lastEnergyGivenTime = DateTime.Parse(PlayerPrefs.GetString("LastEnergyGivenTime"));
        else
            lastEnergyGivenTime = DateTime.MinValue;
    }

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
