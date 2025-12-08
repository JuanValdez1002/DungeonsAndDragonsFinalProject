using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;

    [Header("Damage Settings")]
    public float invulnerabilityTime = 1.0f;
    private float lastDamageTime;

    [Header("UI + Effects")]
    public TakeDamageEffect damageEffect;
    public CameraShake cameraShake;
    public HealthBarUI healthBar;

    [Header("Game Over Screen")]
    public GameObject gameOverCanvas;

    private Rigidbody playerRb;

    void Start()
    {
        currentHealth = maxHealth;
        lastDamageTime = -invulnerabilityTime;

        playerRb = GetComponent<Rigidbody>();

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public void TakeDamage(int amount)
    {
        // Invulnerability window
        if (Time.time - lastDamageTime < invulnerabilityTime)
            return;

        lastDamageTime = Time.time;
        currentHealth -= amount;

        // UI update
        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);

        // Damage effects
        if (damageEffect != null)
            damageEffect.FlashRed();

        if (cameraShake != null)
            cameraShake.Shake();

        // Prevent physics knockback (optional)
        if (playerRb != null)
        {
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            StartCoroutine(TemporarilyFreezePlayer());
        }

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        Debug.Log("Player has died.");

        // Show Game Over Menu
        if (gameOverCanvas != null)
            gameOverCanvas.SetActive(true);

        // Freeze game
        Time.timeScale = 0f;
    }

    IEnumerator TemporarilyFreezePlayer()
    {
        RigidbodyConstraints original = playerRb.constraints;
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        yield return new WaitForSeconds(0.1f);
        playerRb.constraints = original;
        playerRb.linearVelocity = Vector3.zero;
    }

    IEnumerator DamageFlash()
    {
        yield return new WaitForSeconds(0.1f);
    }

    public void Heal(int amount)
    {
        currentHealth = Mathf.Min(currentHealth + amount, maxHealth);

        if (healthBar != null)
            healthBar.UpdateHealthBar(currentHealth, maxHealth);
    }

    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}
