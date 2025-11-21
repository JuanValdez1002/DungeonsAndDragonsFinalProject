using UnityEngine;

/// <summary>
/// Makes an enemy respawn after being killed.
/// Attach this to the enemy GameObject.
/// </summary>
public class EnemyRespawner : MonoBehaviour
{
    [Header("Respawn Settings")]
    [Tooltip("Time in seconds before enemy respawns")]
    public float respawnDelay = 5f;
    
    [Tooltip("Should the enemy respawn at all?")]
    public bool canRespawn = true;
    
    [Tooltip("Maximum number of times this enemy can respawn (-1 = infinite)")]
    public int maxRespawns = -1;
    
    [Header("Visual Effects")]
    [Tooltip("Show countdown timer in console?")]
    public bool showDebugCountdown = true;
    
    // Private tracking variables
    private Vector3 spawnPosition;
    private Quaternion spawnRotation;
    private int currentRespawnCount = 0;
    private EnemyAi enemyAi;
    private bool isRespawning = false;

    private void Awake()
    {
        // Remember where this enemy started
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        
        // Get reference to EnemyAi script
        enemyAi = GetComponent<EnemyAi>();
        
        if (enemyAi == null)
        {
            Debug.LogError($"EnemyRespawner on {gameObject.name} requires an EnemyAi component!");
        }
    }

    private void Update()
    {
        // Check if enemy is dead (health <= 0)
        if (enemyAi != null && enemyAi.health <= 0 && !isRespawning)
        {
            // Check if we can still respawn
            if (canRespawn && (maxRespawns == -1 || currentRespawnCount < maxRespawns))
            {
                isRespawning = true;
                Invoke(nameof(RespawnEnemy), respawnDelay);
                
                if (showDebugCountdown)
                {
                    Debug.Log($"{gameObject.name} will respawn in {respawnDelay} seconds...");
                }
            }
            else if (showDebugCountdown)
            {
                Debug.Log($"{gameObject.name} has reached max respawns or respawning is disabled.");
            }
        }
    }

    private void RespawnEnemy()
    {
        // Reset position and rotation
        transform.position = spawnPosition;
        transform.rotation = spawnRotation;
        
        // Reset health
        if (enemyAi != null)
        {
            enemyAi.health = GetDefaultHealth();
            
            // Call the OnRespawn method to re-enable everything
            enemyAi.OnRespawn();
        }
        
        // Increment respawn counter
        currentRespawnCount++;
        isRespawning = false;
        
        Debug.Log($"{gameObject.name} has respawned! (Respawn #{currentRespawnCount})");
        
        // Optional: Add a visual effect here (particle system, flash, etc.)
    }

    /// <summary>
    /// Gets the default health value for this enemy type.
    /// You can customize this based on enemy difficulty.
    /// </summary>
    private float GetDefaultHealth()
    {
        // You can change this value or make it a public variable
        return 100f;
    }

    /// <summary>
    /// Call this to manually trigger a respawn
    /// </summary>
    public void ForceRespawn()
    {
        CancelInvoke(nameof(RespawnEnemy));
        isRespawning = false;
        RespawnEnemy();
    }

    /// <summary>
    /// Call this to prevent this enemy from respawning anymore
    /// </summary>
    public void DisableRespawn()
    {
        canRespawn = false;
        CancelInvoke(nameof(RespawnEnemy));
    }
}
