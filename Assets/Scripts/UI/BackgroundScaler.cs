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


        float worldScreenHeight = Camera.main.orthographicSize * 2;
        float worldScreenWidth = worldScreenHeight * Screen.width / Screen.height;


        float spriteWidth = sr.sprite.bounds.size.x;
        float spriteHeight = sr.sprite.bounds.size.y;


        float scaleX = worldScreenWidth / spriteWidth;
        float scaleY = worldScreenHeight / spriteHeight;

        float finalScale = Mathf.Max(scaleX, scaleY);

        transform.localScale = new Vector3(finalScale, finalScale, 1);

        Vector3 pos = new Vector3(0, 0, 5);

        transform.position = pos;
        
    }

    private void AdjustCollider()
    {
        if (spawnAreaCollider == null) return;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        float bgWidth = sr.bounds.size.x;
        float bgHeight = sr.bounds.size.y;

        spawnAreaCollider.transform.position = transform.position;

        spawnAreaCollider.size = new Vector2(bgWidth * 0.7f, bgHeight * 0.4f); 
    }
}
