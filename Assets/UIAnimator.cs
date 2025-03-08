using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIAnimator : MonoBehaviour
{
    public static UIAnimator Instance;

    public Transform darkBackground;

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
    }

    public void ShowUI(GameObject panel, float duration) 
    { 
            panel.SetActive(true);
            panel.transform.localScale = Vector3.zero;
            panel.transform.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }

    public void HideUI(GameObject panel, float duration)
    {
        panel.transform.DOScale(Vector3.zero, duration)
        .SetEase(Ease.InBack)
                .OnComplete(() => panel.SetActive(false));
    }

    public void ShowBackground()
    {
        if (darkBackground != null)
        {
            darkBackground.gameObject.SetActive(true);
            Image image = darkBackground.GetComponent<Image>();
            image.DOFade(0, 0);
            image.DOFade(0.7f, 0.3f);
        }
    }

    public void HideBackground()
    {
    
        if (darkBackground != null)
        {
            Image image = darkBackground.GetComponent<Image>();
            image.DOFade(0, 0.3f)
                .OnComplete(() => darkBackground.gameObject.SetActive(false));
        }
    }
}
