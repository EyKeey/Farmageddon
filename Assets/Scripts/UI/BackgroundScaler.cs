using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public BoxCollider2D spawnAreaCollider; // Tavuklar�n spawn oldu�u collider

    private void Start()
    {
        ScaleBackground();
        AdjustCollider();
    }

    private void ScaleBackground()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr == null)
            return;

        // Kameran�n World Space geni�li�i ve y�ksekli�i
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        // Sprite'�n orijinal boyutu
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Arka plan�n geni�li�i ekran geni�li�inden k���kse geni�li�i b�y�t
        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        // HANG�S� DAHA B�Y�KSE ONU KULLAN (Bo�luk kalmamas� i�in)
        float finalScale = Mathf.Max(scaleX, scaleY);

        // Sprite'� ekrana tam oturt
        transform.localScale = new Vector3(finalScale, finalScale, 1);

        Vector3 pos = new Vector3(0, 0, 5);

        // Konumu s�f�rla (Tam ortada olsun)
        transform.position = pos;
        
    }

    private void AdjustCollider()
    {
        if (spawnAreaCollider == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float bgWidth = sr.bounds.size.x;
        float bgHeight = sr.bounds.size.y;

        // Collider'� arka plan�n ortas�na hizala
        spawnAreaCollider.transform.position = transform.position;

        // Collider'� arka plan�n belli bir k�sm�na g�re �l�ekle
        spawnAreaCollider.size = new Vector2(bgWidth * 0.7f, bgHeight * 0.4f); // Oran� de�i�tirebilirsin
    }
}
