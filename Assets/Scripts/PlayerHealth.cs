using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public TakeDamageEffect damageEffect;
    public CameraShake cameraShake;
    public HealthBarUI healthBar;

    public GameObject gameOverCanvas;   // NEW

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

        if (damageEffect != null)
            damageEffect.FlashRed();

        if (cameraShake != null)
            cameraShake.Shake();

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player has died.");

        // Enable Game Over UI
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
    }
}
