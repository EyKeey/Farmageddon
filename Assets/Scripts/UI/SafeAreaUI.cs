using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class SafeAreaUI : MonoBehaviour
{
    private RectTransform rectTransform;
    private Rect lastSafeArea;

    [SerializeField] private bool isTopBar = false; 

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        ApplySafeArea();
    }

    private void Update()
    {
        if (Screen.safeArea != lastSafeArea)
        {
            ApplySafeArea();
        }
    }

    private void ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        lastSafeArea = safeArea;

        float safeTop = safeArea.yMax / Screen.height;
        float safeBottom = safeArea.yMin / Screen.height;

        Vector2 anchorMin = rectTransform.anchorMin;
        Vector2 anchorMax = rectTransform.anchorMax;

        if (isTopBar)
        {
            
            anchorMin.y = safeTop;
            anchorMax.y = 1f;
            rectTransform.pivot = new Vector2(0.5f, 1f); 
        }
        else
        {
            
            anchorMin.y = 0f;
            anchorMax.y = safeBottom;
            rectTransform.pivot = new Vector2(0.5f, 0f); 
        }

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;
    }
}
