using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    public int levelIndex;

    private TextMeshProUGUI levelText;
    private Button button;
    public GameObject levelInfoPanelPrefab;
    private bool isCompleted;

    public Sprite completedLevel;
    public Sprite currentLevel;
    public Sprite lockedLevel;
    public Sprite bossLevel;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> OnButtonClick());
        
        levelText = GetComponentInChildren<TextMeshProUGUI>();
        levelText.text = levelIndex.ToString();

        SetUI();
    }

    private void SetUI()
    {
        LevelData levelInfo = LevelLoader.instance.GetSpesificLevelData(levelIndex);
        Image image = GetComponent<Image>();
        if(levelInfo.isLevelCompleted)
        {
            //eðer tamamlanmýþsa
            isCompleted = true;
            image.sprite = completedLevel;
        }
        else
        {
            if (PlayerPrefs.GetInt("CurrentLevel") == levelInfo.level)
            {
                //eðer henüz tamamlanmamýþsa ve þu anki levelse
                image.sprite = currentLevel;
            }
            else
            {
                //eðer henüz tamamlanmamýþ ve þuanki level deðilse
                isCompleted = false;
                image.sprite = lockedLevel;
            }

        }

    }

    private void OnButtonClick()
    {
        if (!isCompleted)
        {
            Canvas canvas = FindAnyObjectByType<Canvas>();
            GameObject levelInfoPanel = Instantiate(levelInfoPanelPrefab, canvas.transform);
            levelInfoPanel.GetComponent<LevelInfoPanel>().levelIndex = levelIndex;
            UIAnimator.Instance.ShowUI(levelInfoPanel, 0.3f);
        }

    }

}
