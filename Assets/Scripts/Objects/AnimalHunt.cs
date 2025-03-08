using System;
using System.Collections;
using UnityEngine;

public class AnimalHunt : MonoBehaviour
{
    private int clickCount;
    private float clickCooldown = 2f;
    private float lastClickTime = 0;
    public bool isCatched = false;
    private float catchTime;
    private float escapeTime = 10;

    private MobMovement mobMovement;
    private CollectableItem collectableItem;

    private void Start()
    {
        // Bileþenleri önceden al ve kontrol et
        mobMovement = GetComponent<MobMovement>();
        if (mobMovement == null)
        {
            Debug.LogError("AnimalHunt: MobMovement bileþeni eksik!");
        }

        collectableItem = GetComponent<CollectableItem>();
        collectableItem.enabled = false;
        if (collectableItem == null)
        {
            Debug.LogError("AnimalHunt: CollectableItem bileþeni eksik!");
        }
    }

    private void Update()
    {
        if (Time.time - lastClickTime > clickCooldown)
        {
            clickCount = 0;
        }

        if (isCatched && Time.time - catchTime > escapeTime)
        {
            StartCoroutine(Escape());
        }
    }

    private IEnumerator Escape()
    {
        Debug.Log("Ayý kaçýyor!");

        GameObject escapeObj = GameObject.Find("EscapePoint");
        if (escapeObj == null)
        {
            Debug.LogError("Escape point bulunamadý!");
            yield break; // Hata varsa coroutine'i bitir
        }

        Vector2 escapePoint = escapeObj.transform.position;

        if (mobMovement != null)
        {
            mobMovement.moveSpeed = 6f;
            mobMovement.boundaryArea = null;
            mobMovement.Move(escapePoint);
        }

        yield return new WaitForSeconds(3);
        Destroy(gameObject);
    }

    public void BearHunt()
    {
        if (Time.time - lastClickTime < clickCooldown)
        {
            clickCount++;
        }

        lastClickTime = Time.time;

        if (clickCount >= 6 && !isCatched)
        {
            if (mobMovement != null)
            {
                mobMovement.Stopped();
                mobMovement.enabled = false;
            }

            isCatched = true;
            catchTime = Time.time;

            if (collectableItem != null)
            {
                collectableItem.enabled = true;
                StartCoroutine(collectableItem.Collect());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Animal") && !isCatched)
        {
            Mob mob = collision.gameObject.GetComponent<Mob>();
            if (mob != null)
            {
                mob.Die();
            }
            else
            {
                Debug.LogWarning("AnimalHunt: Çarpýlan nesnede Mob bileþeni yok!");
            }
        }
    }
}
