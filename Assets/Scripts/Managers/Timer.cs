using UnityEngine;
using TMPro;
using System.Collections;
using System;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public static Timer Instance;

    private TextMeshProUGUI m_TextMeshPro;
    public float elapsedTime = 0f;
    private bool isRunning = true;

    [SerializeField] private bool includeMilliseconds = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        m_TextMeshPro = GetComponent<TextMeshProUGUI>();
        if (m_TextMeshPro == null)
        {
            Debug.LogError("TextMeshProUGUI component bulunamadý!");
        }
    }

    void Update()
    {
        if (isRunning)
        {
            elapsedTime += Time.deltaTime;
            UpdateTimerDisplay();
        }
    }

    private void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(elapsedTime / 60);
        int seconds = Mathf.FloorToInt(elapsedTime % 60);

        if (includeMilliseconds)
        {
            int milliseconds = Mathf.FloorToInt((elapsedTime * 100) % 100);
            m_TextMeshPro.text = string.Format("{0:00}:{1:00}.{2:00}", minutes, seconds, milliseconds);
        }
        else
        {
            m_TextMeshPro.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }

    // Timer kontrolü için public metodlar
    public void StartTimer()
    {
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        elapsedTime = 0f;
        UpdateTimerDisplay();
    }

    public float GetTime()
    {
        return elapsedTime;
    }
}