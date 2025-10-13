using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public int maxHealth = 100;
    public int currentHealth;
    
    [Header("Damage Settings")]
    public float invulnerabilityTime = 1.0f; // Time player is invulnerable after taking damage
    private float lastDamageTime;
    
    [Header("Death Settings")]
    public float respawnHeight = 2.0f; // Height to respawn player at if they fall through map
    
    private void Start()
    {
        currentHealth = maxHealth;
        lastDamageTime = -invulnerabilityTime; // Allow damage immediately
    }
    
    private void Update()
    {
        // Check if player fell through the map (more aggressive checking)
        if (transform.position.y < -5f)
        {
            Debug.LogWarning("Player fell through map! Respawning...");
            RespawnPlayer();
        }
    }
    
    public void TakeDamage(int damage)
    {
        // Check invulnerability
        if (Time.time - lastDamageTime < invulnerabilityTime)
        {
            Debug.Log("Player is invulnerable, damage ignored");
            return;
        }
        
        // Store current position before taking damage
        Vector3 positionBeforeDamage = transform.position;
        
        currentHealth -= damage;
        lastDamageTime = Time.time;
        
        Debug.Log($"Player took {damage} damage. Health: {currentHealth}/{maxHealth}");
        
        // Prevent player from being pushed by freezing rigidbody temporarily
        Rigidbody playerRb = GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            // Temporarily disable physics to prevent being pushed
            playerRb.linearVelocity = Vector3.zero;
            playerRb.angularVelocity = Vector3.zero;
            
            // Optional: Freeze position briefly to prevent physics push
            StartCoroutine(TemporarilyFreezePlayer(playerRb));
        }
        
        // Visual feedback (optional - could add screen flash, etc.)
        StartCoroutine(DamageFlash());
        
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    
    private System.Collections.IEnumerator TemporarilyFreezePlayer(Rigidbody playerRb)
    {
        // Briefly freeze the player to prevent physics push from projectiles
        RigidbodyConstraints originalConstraints = playerRb.constraints;
        playerRb.constraints = RigidbodyConstraints.FreezeAll;
        
        yield return new WaitForSeconds(0.1f);
        
        // Restore original constraints
        playerRb.constraints = originalConstraints;
        playerRb.linearVelocity = Vector3.zero; // Ensure no leftover velocity
    }
    
    private System.Collections.IEnumerator DamageFlash()
    {
        // Simple damage feedback - could expand this
        yield return new WaitForSeconds(0.1f);
        // Could add red screen flash or player model flash here
    }
    
    public void Heal(int healAmount)
    {
        currentHealth = Mathf.Min(currentHealth + healAmount, maxHealth);
        Debug.Log($"Player healed for {healAmount}. Health: {currentHealth}/{maxHealth}");
    }
    
    private void Die()
    {
        Debug.Log("Player died!");
        
        // Reset health and respawn
        currentHealth = maxHealth;
        RespawnPlayer();
    }
    
    private void RespawnPlayer()
    {
        // Find a safe respawn position
        Vector3 respawnPosition = FindSafeRespawnPosition();
        transform.position = respawnPosition;
        
        Debug.Log($"Player respawned at: {respawnPosition}");
    }
    
    private Vector3 FindSafeRespawnPosition()
    {
        // Try to find the DungeonCreator and get a safe position
        DungeonCreator dungeonCreator = FindFirstObjectByType<DungeonCreator>();
        if (dungeonCreator != null)
        {
            // Use the center of the dungeon area as a safe spawn
            return new Vector3(
                dungeonCreator.dungeonWidth / 2, 
                respawnHeight, 
                dungeonCreator.dungeonLength / 2
            );
        }
        
        // Fallback: spawn at origin above ground
        return new Vector3(0, respawnHeight, 0);
    }
    
    // Public getter for UI or other systems
    public float GetHealthPercentage()
    {
        return (float)currentHealth / maxHealth;
    }
}