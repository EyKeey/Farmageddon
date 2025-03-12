using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using System;
using UnityEngine.UI;

public class MergeAnimationScene : MonoBehaviour
{
    public Transform mergeCenter; // Ortada birleşme noktası
    public List<Transform> mergingItems; // Birleşecek itemlerin Transformları
    public Transform resultItem; // Ortaya çıkacak sonuç itemi
    public TextMeshProUGUI resultText; // Sonuç Text'i
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


        resultItem.localScale = Vector3.zero; // Başlangıçta sonucu görünmez yap
        resultText.alpha = 0f;  // Başlangıçta text şeffaf olsun
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
       
        skipAnimationText.DOFade(1f, 0.5f); // Text fade-in efekti
        skipAnimationText.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBack) // Bounce efekti
            .OnComplete(() => skipAnimationText.transform.DOScale(Vector3.one, 0.2f)); // Küçülme

        // 1️⃣ - Rastgele yönlere salınma
        foreach (var item in mergingItems)
        {
            item.DOShakePosition(3.5f, 200f, 0, 180, false, true);
        }

        yield return new WaitForSecondsRealtime(1.5f); // Salınım süresi

        // 2️⃣ - Itemleri yukarı kaldır
        foreach (var item in mergingItems)
        {
            item.DOMoveY(item.position.y + 1.5f, 0.5f).SetEase(Ease.OutQuad);
        }

        yield return new WaitForSecondsRealtime(0.5f);

        // 3️⃣ - Itemleri merkeze doğru çektir ve küçült
        foreach (var item in mergingItems)
        {
            item.DOMove(mergeCenter.position, 0.5f).SetEase(Ease.InQuad);
            item.DOScale(Vector3.zero, 0.5f).SetEase(Ease.InQuad);
        }

        yield return new WaitForSecondsRealtime(1.5f);

        // 4️⃣ - Sonuç itemini büyüt ve göster
        resultItem.position = mergeCenter.position;
        resultItem.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);

        yield return new WaitForSecondsRealtime(0.5f);

        // 5️⃣ - Eski itemleri yok et
        foreach (var item in mergingItems)
        {
            Destroy(item.gameObject);
        }

        mergingItems.Clear();

        // 6️⃣ - Sonuç Text'ini göster (fade-in + bounce effect)
        StartTextEffect(resultText);
        skipAnimationText.DOFade(0f, 1f);

        yield return new WaitForSecondsRealtime(0.3f);

        foreach(var item in itemInfoTexts)
        {
            StartTextEffect(item);
        }
        
        yield return new WaitForSecondsRealtime(0.5f); // Yazının tam görünmesi için bekle


        WaitForInput();
    }

    private void StartTextEffect(TextMeshProUGUI text)
    {
        
        text.DOFade(1f, 0.5f); // Text fade-in efekti
        text.transform.DOScale(Vector3.one * 1.2f, 0.3f).SetEase(Ease.OutBack) // Bounce efekti
            .OnComplete(() => text.transform.DOScale(Vector3.one, 0.2f)); // Küçülme
    
    }

    // Bu fonksiyon animasyonu atlamak için kullanılacak
    public void SkipAnimation()
    {
        skipAnimationText.DOFade(0f, 1f);
        StopAllCoroutines(); // Tüm animasyonları durdur
        resultItem.DOScale(Vector3.one, 0f); // Sonuç iteminin ölçeğini anında 1 yap
        resultText.DOFade(1f, 0f); // Sonuç metnini anında göster
        resultText.transform.DOScale(Vector3.one, 0f); // Sonuç metnini anında orijinal boyutunda göster
        foreach(var item in itemInfoTexts)
        {
            item.DOFade(1f, 0f); // Sonuç metnini anında göster
            item.transform.DOScale(Vector3.one, 0f); // Sonuç metnini anında orijinal boyutunda göster
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

