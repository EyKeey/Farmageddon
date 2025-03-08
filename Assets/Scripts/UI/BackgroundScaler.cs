using UnityEngine;

public class BackgroundScaler : MonoBehaviour
{
    public BoxCollider2D spawnAreaCollider; // Tavuklarýn spawn olduðu collider

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

        // Kameranýn World Space geniþliði ve yüksekliði
        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;

        // Sprite'ýn orijinal boyutu
        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;

        // Arka planýn geniþliði ekran geniþliðinden küçükse geniþliði büyüt
        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        // HANGÝSÝ DAHA BÜYÜKSE ONU KULLAN (Boþluk kalmamasý için)
        float finalScale = Mathf.Max(scaleX, scaleY);

        // Sprite'ý ekrana tam oturt
        transform.localScale = new Vector3(finalScale, finalScale, 1);

        Vector3 pos = new Vector3(0, 0, 5);

        // Konumu sýfýrla (Tam ortada olsun)
        transform.position = pos;
        
    }

    private void AdjustCollider()
    {
        if (spawnAreaCollider == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float bgWidth = sr.bounds.size.x;
        float bgHeight = sr.bounds.size.y;

        // Collider'ý arka planýn ortasýna hizala
        spawnAreaCollider.transform.position = transform.position;

        // Collider'ý arka planýn belli bir kýsmýna göre ölçekle
        spawnAreaCollider.size = new Vector2(bgWidth * 0.7f, bgHeight * 0.4f); // Oraný deðiþtirebilirsin
    }
}
