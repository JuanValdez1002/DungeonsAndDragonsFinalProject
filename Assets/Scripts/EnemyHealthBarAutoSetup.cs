using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBarAutoSetup : MonoBehaviour
{
    [Header("Auto Setup")]
    public bool setupOnStart = true;
    
    private void Start()
    {
        if (setupOnStart)
        {
            SetupHealthBar();
        }
    }
    
    [ContextMenu("Setup Health Bar")]
    public void SetupHealthBar()
    {
        // Check if health bar already exists
        EnemyHealthBar existingHealthBar = GetComponentInChildren<EnemyHealthBar>();
        if (existingHealthBar != null)
        {
            Debug.Log("Health bar already exists for " + gameObject.name);
            return;
        }
        
        // Create health bar structure
        GameObject healthBarObj = new GameObject("EnemyHealthBar");
        healthBarObj.transform.SetParent(transform);
        healthBarObj.transform.localPosition = Vector3.zero;
        
        // Create canvas
        GameObject canvasObj = new GameObject("Canvas");
        canvasObj.transform.SetParent(healthBarObj.transform);
        canvasObj.transform.localPosition = new Vector3(0, 2, 0);
        
        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.worldCamera = Camera.main;
        
        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.dynamicPixelsPerUnit = 10;
        
        // Set canvas size
        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.sizeDelta = new Vector2(2, 0.3f);
        canvasRect.localScale = Vector3.one * 0.01f;
        
        // Create slider
        GameObject sliderObj = new GameObject("Slider");
        sliderObj.transform.SetParent(canvasObj.transform);
        
        Slider slider = sliderObj.AddComponent<Slider>();
        slider.minValue = 0;
        slider.maxValue = 1;
        slider.value = 1;
        
        // Set slider to fill canvas
        RectTransform sliderRect = sliderObj.GetComponent<RectTransform>();
        sliderRect.anchorMin = Vector2.zero;
        sliderRect.anchorMax = Vector2.one;
        sliderRect.sizeDelta = Vector2.zero;
        sliderRect.anchoredPosition = Vector2.zero;
        
        // Create background
        GameObject backgroundObj = new GameObject("Background");
        backgroundObj.transform.SetParent(sliderObj.transform);
        
        Image backgroundImg = backgroundObj.AddComponent<Image>();
        backgroundImg.color = new Color(0.2f, 0.2f, 0.2f, 0.8f);
        
        RectTransform backgroundRect = backgroundObj.GetComponent<RectTransform>();
        backgroundRect.anchorMin = Vector2.zero;
        backgroundRect.anchorMax = Vector2.one;
        backgroundRect.sizeDelta = Vector2.zero;
        backgroundRect.anchoredPosition = Vector2.zero;
        
        // Create fill area
        GameObject fillAreaObj = new GameObject("Fill Area");
        fillAreaObj.transform.SetParent(sliderObj.transform);
        
        RectTransform fillAreaRect = fillAreaObj.GetComponent<RectTransform>();
        fillAreaRect.anchorMin = Vector2.zero;
        fillAreaRect.anchorMax = Vector2.one;
        fillAreaRect.sizeDelta = Vector2.zero;
        fillAreaRect.anchoredPosition = Vector2.zero;
        
        // Create fill
        GameObject fillObj = new GameObject("Fill");
        fillObj.transform.SetParent(fillAreaObj.transform);
        
        Image fillImg = fillObj.AddComponent<Image>();
        fillImg.color = Color.red;
        fillImg.type = Image.Type.Filled;
        
        RectTransform fillRect = fillObj.GetComponent<RectTransform>();
        fillRect.anchorMin = Vector2.zero;
        fillRect.anchorMax = Vector2.one;
        fillRect.sizeDelta = Vector2.zero;
        fillRect.anchoredPosition = Vector2.zero;
        
        // Set slider references
        slider.fillRect = fillRect;
        slider.targetGraphic = fillImg;
        
        // Add and configure health bar script
        EnemyHealthBar healthBarScript = healthBarObj.AddComponent<EnemyHealthBar>();
        healthBarScript.healthSlider = slider;
        healthBarScript.healthCanvas = canvas;
        
        Debug.Log("Auto-setup health bar for " + gameObject.name);
    }
}