using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthSlider;
    public Canvas healthCanvas;
    
    [Header("Settings")]
    public Vector3 offset = new Vector3(0, 2.0f, 0); // Offset above enemy
    public bool hideWhenFull = true;
    public bool alwaysFaceCamera = true;
    
    private EnemyAi enemyAi;
    private Camera mainCamera;
    
    private void Start()
    {
        // Get the enemy AI component
        enemyAi = GetComponentInParent<EnemyAi>();
        if (enemyAi == null)
        {
            Debug.LogWarning("EnemyHealthBar: No EnemyAi component found in parent!");
        }
        
        // Find the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            mainCamera = FindFirstObjectByType<Camera>();
        }
        
        // Set up the canvas to face the camera
        if (healthCanvas != null)
        {
            healthCanvas.worldCamera = mainCamera;
        }
        
        // Initialize health bar
        UpdateHealthBar();
    }
    
    private void Update()
    {
        if (enemyAi != null)
        {
            UpdateHealthBar();
            UpdatePosition();
            
            if (alwaysFaceCamera && mainCamera != null)
            {
                // Make health bar always face the camera
                transform.LookAt(transform.position + mainCamera.transform.rotation * Vector3.forward,
                                mainCamera.transform.rotation * Vector3.up);
            }
        }
    }
    
    private void UpdateHealthBar()
    {
        if (healthSlider == null || enemyAi == null) return;
        
        // Calculate health percentage
        float healthPercentage = enemyAi.health / 100f; // Assuming max health is 100
        healthSlider.value = healthPercentage;
        
        // Hide/show health bar based on settings
        if (hideWhenFull && healthPercentage >= 1.0f)
        {
            healthCanvas.gameObject.SetActive(false);
        }
        else
        {
            healthCanvas.gameObject.SetActive(true);
        }
    }
    
    private void UpdatePosition()
    {
        if (enemyAi != null)
        {
            // Position the health bar above the enemy
            transform.position = enemyAi.transform.position + offset;
        }
    }
}