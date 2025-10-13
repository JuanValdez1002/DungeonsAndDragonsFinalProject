using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [Header("UI References")]
    public Slider healthSlider;
    public Text healthText; // Optional text display
    public Image healthFillImage;
    
    [Header("Health Bar Colors")]
    public Color healthyColor = Color.green;
    public Color damagedColor = Color.yellow;
    public Color criticalColor = Color.red;
    
    private PlayerHealth playerHealth;
    
    private void Start()
    {
        // Find the player's health component
        playerHealth = FindFirstObjectByType<PlayerHealth>();
        
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealthUI: No PlayerHealth component found in scene!");
            return;
        }
        
        if (healthSlider == null)
        {
            Debug.LogError("PlayerHealthUI: Health slider not assigned!");
            return;
        }
        
        // Initialize the health bar
        UpdateHealthBar();
    }
    
    private void Update()
    {
        if (playerHealth != null)
        {
            UpdateHealthBar();
        }
    }
    
    private void UpdateHealthBar()
    {
        // Update slider value
        float healthPercentage = playerHealth.GetHealthPercentage();
        healthSlider.value = healthPercentage;
        
        // Update health text if available
        if (healthText != null)
        {
            healthText.text = $"{playerHealth.currentHealth}/{playerHealth.maxHealth}";
        }
        
        // Update health bar color based on health percentage
        if (healthFillImage != null)
        {
            if (healthPercentage > 0.6f)
            {
                healthFillImage.color = healthyColor;
            }
            else if (healthPercentage > 0.3f)
            {
                healthFillImage.color = damagedColor;
            }
            else
            {
                healthFillImage.color = criticalColor;
            }
        }
    }
}