using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIAnimator : MonoBehaviour
{
    public static UIAnimator Instance;

    public Transform darkBackground;
    public GameObject LogPrefab;

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
            image.DOFade(0.85f, 0.3f);
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

    public void ShowMessage(string message)
    {
        float moveDistance = 100f;
        Canvas canvas = FindAnyObjectByType<Canvas>();
        
        GameObject messagePopUp = Instantiate(LogPrefab, canvas.transform);
        Image background = messagePopUp.GetComponent<Image>();
        TextMeshProUGUI messageText = messagePopUp.GetComponentInChildren<TextMeshProUGUI>();

        messageText.text = message;

        Color textColor = messageText.color;
        textColor.a = 0;
        messageText.color = textColor;

        Color bgColor = background.color;
        bgColor.a = 0;
        background.color = bgColor;

        Vector3 startPos = messagePopUp.transform.position;
        messagePopUp.transform.position = new Vector3(startPos.x, startPos.y - moveDistance, startPos.z);

        messagePopUp.transform.DOMoveY(startPos.y, 0.5f).SetEase(Ease.OutQuad);
        messageText.DOFade(1f, 0.5f);
        background.DOFade(1f, 0.5f);

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(1.5f);
        seq.Append(transform.DOMoveY(startPos.y + moveDistance, 0.5f).SetEase(Ease.InQuad));
        seq.Join(messageText.DOFade(0f, 0.5f));
        seq.Join(background.DOFade(0f, 0.5f));
        seq.OnComplete(()=> Destroy(messagePopUp));
    }

}
