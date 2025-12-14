using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;

    public Image foregroundFill;

    void Start()
    {
        currentHealth = maxHealth;
        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        UpdateUI();

        if (currentHealth <= 0)
            Die();
    }

    void UpdateUI()
    {
        if (foregroundFill != null)
            foregroundFill.fillAmount = Mathf.Clamp01((float)currentHealth / maxHealth);
    }

    void Die()
    {
        Debug.Log("Enemy dead");
        Destroy(gameObject);
    }
}
