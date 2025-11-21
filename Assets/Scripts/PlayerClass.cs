using UnityEngine;

/// <summary>
/// Defines the different player classes available
/// </summary>
public enum ClassType
{
    Warrior,    // Melee tank - high health, low damage
    Ranger,     // Ranged DPS - medium health, high ranged damage
    Mage,       // Ranged burst - low health, very high magic damage
    Cleric      // Support/Healer - medium health, healing abilities
}

/// <summary>
/// Contains all stats and data for a player class
/// </summary>
[System.Serializable]
public class PlayerClassData
{
    public ClassType classType;
    public string className;
    public string description;
    
    [Header("Base Stats")]
    public int maxHealth = 100;
    public float moveSpeed = 12f;
    public float attackDamage = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    
    [Header("Special Abilities")]
    public bool canHeal = false;
    public int healAmount = 20;
    public float healCooldown = 5f;
    
    public bool hasRangedAttack = false;
    public GameObject projectilePrefab;
    public float projectileSpeed = 20f;
    
    [Header("Visual")]
    public Color classColor = Color.white;
    public GameObject characterModel; // Optional: different character model per class
}

/// <summary>
/// Manages the player's selected class and abilities
/// </summary>
public class PlayerClass : MonoBehaviour
{
    [Header("Current Class")]
    public PlayerClassData currentClass;
    
    [Header("Class Definitions")]
    public PlayerClassData warriorClass;
    public PlayerClassData rangerClass;
    public PlayerClassData mageClass;
    public PlayerClassData clericClass;
    
    [Header("References")]
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public Transform projectileSpawnPoint;
    
    [Header("UI Feedback")]
    public GameObject attackEffect;
    
    private float lastAttackTime;
    private float lastHealTime;
    
    void Start()
    {
        // Initialize with default class if none selected
        if (currentClass == null)
        {
            Debug.LogWarning("No class selected, defaulting to Warrior");
            SetClass(ClassType.Warrior);
        }
        else
        {
            ApplyClassStats();
        }
    }
    
    void Update()
    {
        // Attack input (Left Mouse Button or Fire1)
        if (Input.GetButtonDown("Fire1") || Input.GetMouseButtonDown(0))
        {
            TryAttack();
        }
        
        // Heal ability (H key or Fire2) - if class can heal
        if (currentClass.canHeal && (Input.GetKeyDown(KeyCode.H) || Input.GetButtonDown("Fire2")))
        {
            TryHeal();
        }
        
        // Special ability (Right Mouse Button)
        if (Input.GetMouseButtonDown(1))
        {
            UseSpecialAbility();
        }
    }
    
    /// <summary>
    /// Sets the player's class and applies stats
    /// </summary>
    public void SetClass(ClassType type)
    {
        switch (type)
        {
            case ClassType.Warrior:
                currentClass = warriorClass;
                break;
            case ClassType.Ranger:
                currentClass = rangerClass;
                break;
            case ClassType.Mage:
                currentClass = mageClass;
                break;
            case ClassType.Cleric:
                currentClass = clericClass;
                break;
        }
        
        ApplyClassStats();
        Debug.Log($"Class set to: {currentClass.className}");
    }
    
    /// <summary>
    /// Applies the current class stats to the player
    /// </summary>
    private void ApplyClassStats()
    {
        if (playerHealth != null)
        {
            playerHealth.maxHealth = currentClass.maxHealth;
            playerHealth.currentHealth = currentClass.maxHealth;
        }
        
        if (playerMovement != null)
        {
            playerMovement.speed = currentClass.moveSpeed;
        }
        
        // Apply visual changes (color tint, model swap, etc.)
        ApplyClassVisuals();
    }
    
    /// <summary>
    /// Applies visual changes based on class
    /// </summary>
    private void ApplyClassVisuals()
    {
        // Change player color based on class
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers)
        {
            if (r.material != null)
            {
                r.material.color = currentClass.classColor;
            }
        }
        
        // TODO: Swap character model if different models per class
    }
    
    /// <summary>
    /// Attempts to perform an attack
    /// </summary>
    private void TryAttack()
    {
        if (Time.time - lastAttackTime < currentClass.attackCooldown)
        {
            Debug.Log("Attack on cooldown");
            return;
        }
        
        lastAttackTime = Time.time;
        
        if (currentClass.hasRangedAttack)
        {
            PerformRangedAttack();
        }
        else
        {
            PerformMeleeAttack();
        }
    }
    
    /// <summary>
    /// Performs a melee attack (raycast in front of player)
    /// </summary>
    private void PerformMeleeAttack()
    {
        Debug.Log($"{currentClass.className} performs melee attack!");
        
        // Raycast forward to detect enemies
        RaycastHit hit;
        Vector3 forward = transform.forward;
        
        if (Physics.Raycast(transform.position + Vector3.up, forward, out hit, currentClass.attackRange))
        {
            Debug.DrawRay(transform.position + Vector3.up, forward * currentClass.attackRange, Color.red, 0.5f);
            
            // Check if we hit an enemy
            EnemyAi enemy = hit.collider.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                enemy.TakeDamage((int)currentClass.attackDamage);
                Debug.Log($"Hit enemy for {currentClass.attackDamage} damage!");
                
                // Visual feedback
                if (attackEffect != null)
                {
                    Instantiate(attackEffect, hit.point, Quaternion.identity);
                }
            }
        }
        else
        {
            Debug.DrawRay(transform.position + Vector3.up, forward * currentClass.attackRange, Color.yellow, 0.5f);
            Debug.Log("Melee attack missed");
        }
    }
    
    /// <summary>
    /// Performs a ranged attack (shoots projectile)
    /// </summary>
    private void PerformRangedAttack()
    {
        Debug.Log($"{currentClass.className} shoots projectile!");
        
        if (currentClass.projectilePrefab == null)
        {
            Debug.LogWarning("No projectile prefab assigned for this class!");
            return;
        }
        
        // Spawn projectile
        Vector3 spawnPos = projectileSpawnPoint != null ? 
            projectileSpawnPoint.position : 
            transform.position + transform.forward + Vector3.up;
        
        GameObject projectile = Instantiate(currentClass.projectilePrefab, spawnPos, Quaternion.identity);
        
        // Set projectile properties
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * currentClass.projectileSpeed;
        }
        
        // Set projectile damage
        PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
        if (projScript != null)
        {
            projScript.damage = (int)currentClass.attackDamage;
        }
        
        // Destroy after 5 seconds
        Destroy(projectile, 5f);
    }
    
    /// <summary>
    /// Attempts to heal (Cleric ability)
    /// </summary>
    private void TryHeal()
    {
        if (!currentClass.canHeal)
        {
            Debug.Log("This class cannot heal");
            return;
        }
        
        if (Time.time - lastHealTime < currentClass.healCooldown)
        {
            Debug.Log("Heal on cooldown");
            return;
        }
        
        lastHealTime = Time.time;
        
        if (playerHealth != null)
        {
            playerHealth.Heal(currentClass.healAmount);
            Debug.Log($"Healed for {currentClass.healAmount} HP!");
            
            // Visual feedback (could add particle effect)
        }
    }
    
    /// <summary>
    /// Class-specific special ability
    /// </summary>
    private void UseSpecialAbility()
    {
        switch (currentClass.classType)
        {
            case ClassType.Warrior:
                // Warrior: Defensive stance (reduce damage taken temporarily)
                StartCoroutine(WarriorDefensiveStance());
                break;
                
            case ClassType.Ranger:
                // Ranger: Multi-shot (fire 3 projectiles)
                RangerMultiShot();
                break;
                
            case ClassType.Mage:
                // Mage: Explosive fireball (high damage AOE)
                MageFireball();
                break;
                
            case ClassType.Cleric:
                // Cleric: Area heal (heal over time)
                StartCoroutine(ClericAreaHeal());
                break;
        }
    }
    
    private System.Collections.IEnumerator WarriorDefensiveStance()
    {
        Debug.Log("Warrior: Defensive Stance activated!");
        // Reduce damage taken by 50% for 3 seconds
        if (playerHealth != null)
        {
            // Store original invulnerability time
            float originalInvuln = playerHealth.invulnerabilityTime;
            playerHealth.invulnerabilityTime = 0.5f; // Shorter cooldown between hits
            
            yield return new WaitForSeconds(3f);
            
            playerHealth.invulnerabilityTime = originalInvuln;
        }
        Debug.Log("Defensive Stance ended");
    }
    
    private void RangerMultiShot()
    {
        Debug.Log("Ranger: Multi-shot!");
        // Fire 3 projectiles in a spread
        for (int i = -1; i <= 1; i++)
        {
            Vector3 spawnPos = projectileSpawnPoint != null ? 
                projectileSpawnPoint.position : 
                transform.position + transform.forward + Vector3.up;
            
            GameObject projectile = Instantiate(currentClass.projectilePrefab, spawnPos, Quaternion.identity);
            
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Spread projectiles
                Vector3 direction = Quaternion.Euler(0, i * 15f, 0) * transform.forward;
                rb.linearVelocity = direction * currentClass.projectileSpeed;
            }
            
            Destroy(projectile, 5f);
        }
    }
    
    private void MageFireball()
    {
        Debug.Log("Mage: Fireball!");
        // Fire a powerful explosive projectile
        Vector3 spawnPos = projectileSpawnPoint != null ? 
            projectileSpawnPoint.position : 
            transform.position + transform.forward + Vector3.up;
        
        GameObject projectile = Instantiate(currentClass.projectilePrefab, spawnPos, Quaternion.identity);
        
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * (currentClass.projectileSpeed * 1.5f);
        }
        
        // Double damage for special ability
        PlayerProjectile projScript = projectile.GetComponent<PlayerProjectile>();
        if (projScript != null)
        {
            projScript.damage = (int)(currentClass.attackDamage * 2);
            projScript.isExplosive = true;
        }
        
        Destroy(projectile, 5f);
    }
    
    private System.Collections.IEnumerator ClericAreaHeal()
    {
        Debug.Log("Cleric: Area Heal!");
        // Heal over time
        int tickCount = 5;
        int healPerTick = currentClass.healAmount / tickCount;
        
        for (int i = 0; i < tickCount; i++)
        {
            if (playerHealth != null)
            {
                playerHealth.Heal(healPerTick);
            }
            yield return new WaitForSeconds(1f);
        }
        
        Debug.Log("Area Heal complete");
    }
}
