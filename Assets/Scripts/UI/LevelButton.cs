using DG.Tweening;
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

    [Header("Required Components")]
    public GameObject levelInfoPanelPrefab;
    public Image glowImage;
    public Transform darkBg;
    public Transform starPanel;

    [Header("Button Icon Sprites")]
    public Sprite completedLevel;
    public Sprite currentLevel;
    public Sprite lockedLevel;
    public Sprite bossLevel;

    [Header("Prefabs")]
    public GameObject starPrefab;

    private TextMeshProUGUI levelText;
    private Button button;
    private bool isCompleted;
    private Tween pulseTween;

    private void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(()=> OnButtonClick());
        
        levelText = GetComponentInChildren<TextMeshProUGUI>();
        levelText.text = levelIndex.ToString();
        glowImage.gameObject.SetActive(false);

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
            

            foreach(Transform child in starPanel)
            {
                Destroy(child);
            }

            for (int i = 0; i <= levelInfo.currentStars - 1; i++)
            {
                Instantiate(starPrefab, starPanel);
            }

        }
        else
        {
            if (PlayerPrefs.GetInt("CurrentLevel") == levelInfo.level)
            {
                //eðer henüz tamamlanmamýþsa ve þu anki levelse
                image.sprite = completedLevel;

                glowImage.gameObject.SetActive(true);
                glowImage.color = new Color(1,1,1,0);

                glowImage.DOFade(1f, 0.4f);

                pulseTween = glowImage.rectTransform
                    .DOScale(1.4f, 1f)
                    .SetLoops(-1, LoopType.Yoyo)
                    .SetEase(Ease.InOutSine);
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
            UIAnimator.Instance.darkBackground = darkBg;
            UIAnimator.Instance.ShowBackground();
        }

    }

}
