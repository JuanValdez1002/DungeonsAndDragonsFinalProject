using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public TakeDamageEffect damageEffect;  
    public CameraShake cameraShake;        

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;

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
        // TODO: Respawn system later
    }
}
