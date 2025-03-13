using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

public class MergeAnimationScene : MonoBehaviour
{
    public Transform mergeCenter; 
    public List<Transform> mergingItems; 
    public Transform resultItem; 
    public TextMeshProUGUI resultText; 
    public TextMeshProUGUI skipAnimationText;
    public List<TextMeshProUGUI> itemInfoTexts;
    public float animationDuration = 9f;
    private Button bttn;


    private void Start()
    {
        
        DOTween.Init();
        DOTween.PlayAll();

        bttn = GetComponent<Button>();
        bttn.onClick.RemoveAllListeners();
        bttn.onClick.AddListener(()=>SkipAnimation());


        resultItem.localScale = Vector3.zero; 
        resultText.alpha = 0f;  
        skipAnimationText.alpha = 0f;
        foreach (var item in itemInfoTexts)
        {
            item.alpha = 0f;
        }

        StartCoroutine(MergeSequence());
    }

    public void PlayMergeAnimation(List<string> inputItems, string resultItem)
    {
        SetComponents(inputItems, resultItem);
        StartCoroutine(MergeSequence());
    }

    private void SetComponents(List<string> inputItems, string mergedItem)
    {
        
        ItemData item = ItemManager.Instance.GetItemDataByName(mergedItem);
        Debug.Log(item.rarity);

        int i = 0;
        foreach (var entry in mergingItems)
        {
            if (i < inputItems.Count && inputItems[i] != null)
            {
                ItemData inputItem = ItemManager.Instance.GetItemDataByName(inputItems[i]);
                entry.GetComponent<Image>().sprite = inputItem.itemIcon;
            }
            else
            {
                entry.gameObject.SetActive(false);
            }
            i++;
        }


        resultItem.GetComponent<Image>().sprite = item.itemIcon;

        itemInfoTexts[0].text = item.baseName;
        itemInfoTexts[1].text = item.rarity;
        itemInfoTexts[2].text = item.itemDescription;
        

    }

    private IEnumerator MergeSequence()
    {
       
        skipAnimationText.DOFade(1f, 0.5f); 
        skipAnimationText.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBack) 
            .OnComplete(() => skipAnimationText.transform.DOScale(Vector3.one, 0.2f));

        foreach (var item in mergingItems)
        {
            item.DOShakePosition(3.5f, 200f, 0, 180, false, true);
        }

        yield return new WaitForSecondsRealtime(1.5f); 

        foreach (var item in mergingItems)
        {
            item.DOMoveY(item.position.y + 1.5f, 0.5f).SetEase(Ease.OutQuad);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        foreach (var item in mergingItems)
        {
            item.DOMove(mergeCenter.position, 0.5f).SetEase(Ease.InQuad);
            item.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad);
        }

        yield return new WaitForSecondsRealtime(1.5f);

        resultItem.position = mergeCenter.position;
        resultItem.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSecondsRealtime(0.5f);

        foreach (var item in mergingItems)
        {
            Destroy(item.gameObject);
        }

        mergingItems.Clear();

        StartTextEffect(resultText);
        skipAnimationText.DOFade(0f, 1f);

        yield return new WaitForSecondsRealtime(0.3f);

        foreach(var item in itemInfoTexts)
        {
            StartTextEffect(item);
        }
        
        yield return new WaitForSecondsRealtime(0.5f);


        WaitForInput();
    }

    private void StartTextEffect(TextMeshProUGUI text)
    {
        
        text.DOFade(1f, 0.5f); 
        text.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBack)
            .OnComplete(() => text.transform.DOScale(Vector3.one, 0.2f)); 
    
    }

    public void SkipAnimation()
    {
        skipAnimationText.DOFade(0f, 1f);
        StopAllCoroutines(); 
        resultItem.DOScale(Vector3.one, 0f); 
        resultText.DOFade(1f, 0f); 
        resultText.transform.DOScale(Vector3.one, 0f); 
        foreach(var item in itemInfoTexts)
        {
            item.DOFade(1f, 0f); 
            item.transform.DOScale(Vector3.one, 0f); 
        }
        

        foreach (var item in mergingItems)
        {
            Destroy(item.gameObject);
        }

        mergingItems.Clear();

        WaitForInput();
    }

    private void WaitForInput()
    {
        bttn.onClick.RemoveAllListeners();
        bttn.onClick.AddListener(() => CloseScreen());
    }

    private void CloseScreen()
    {
        MergeManager.instance.isAnimationFinished = true;   
        Destroy(gameObject);
    }
}

