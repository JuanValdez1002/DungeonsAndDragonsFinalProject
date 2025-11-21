using UnityEngine;

/// <summary>
/// Initializes the player with the selected class from the class selection screen
/// Place this on a GameObject in your game scene (not the class selection scene)
/// </summary>
public class GameManager : MonoBehaviour
{
    [Header("Player Setup")]
    public GameObject playerPrefab;
    public Transform playerSpawnPoint;
    
    [Header("Class Projectile Prefabs")]
    public GameObject rangerArrowPrefab;
    public GameObject mageFireballPrefab;
    public GameObject clericLightPrefab;
    
    private GameObject playerInstance;
    
    void Start()
    {
        InitializePlayer();
    }
    
    /// <summary>
    /// Spawns and initializes player with selected class
    /// </summary>
    private void InitializePlayer()
    {
        // Get selected class from PlayerPrefs (set in class selection)
        int classIndex = PlayerPrefs.GetInt("SelectedClass", 0); // Default to Warrior
        ClassType selectedClass = (ClassType)classIndex;
        
        Debug.Log($"Initializing player as {selectedClass}");
        
        // Find existing player or spawn new one
        playerInstance = GameObject.FindWithTag("Player");
        
        if (playerInstance == null && playerPrefab != null)
        {
            Vector3 spawnPos = playerSpawnPoint != null ? playerSpawnPoint.position : Vector3.zero;
            playerInstance = Instantiate(playerPrefab, spawnPos, Quaternion.identity);
            playerInstance.tag = "Player";
        }
        
        if (playerInstance != null)
        {
            // Get or add PlayerClass component
            PlayerClass playerClass = playerInstance.GetComponent<PlayerClass>();
            if (playerClass == null)
            {
                playerClass = playerInstance.AddComponent<PlayerClass>();
            }
            
            // Setup class data
            SetupClassData(playerClass);
            
            // Set the selected class
            playerClass.SetClass(selectedClass);
            
            Debug.Log($"Player initialized as {selectedClass}");
        }
        else
        {
            Debug.LogError("No player found or spawned!");
        }
    }
    
    /// <summary>
    /// Sets up the class data with predefined stats
    /// </summary>
    private void SetupClassData(PlayerClass playerClass)
    {
        // Warrior - Melee tank
        playerClass.warriorClass = new PlayerClassData
        {
            classType = ClassType.Warrior,
            className = "Warrior",
            description = "Melee tank with high health",
            maxHealth = 150,
            moveSpeed = 10f,
            attackDamage = 15f,
            attackRange = 2.5f,
            attackCooldown = 0.8f,
            canHeal = false,
            hasRangedAttack = false,
            classColor = new Color(0.8f, 0.2f, 0.2f) // Red
        };
        
        // Ranger - Ranged DPS
        playerClass.rangerClass = new PlayerClassData
        {
            classType = ClassType.Ranger,
            className = "Ranger",
            description = "High damage ranged attacker",
            maxHealth = 100,
            moveSpeed = 14f,
            attackDamage = 20f,
            attackRange = 20f,
            attackCooldown = 0.6f,
            canHeal = false,
            hasRangedAttack = true,
            projectilePrefab = rangerArrowPrefab,
            projectileSpeed = 25f,
            classColor = new Color(0.2f, 0.8f, 0.2f) // Green
        };
        
        // Mage - Burst damage
        playerClass.mageClass = new PlayerClassData
        {
            classType = ClassType.Mage,
            className = "Mage",
            description = "Powerful magic attacker",
            maxHealth = 75,
            moveSpeed = 12f,
            attackDamage = 30f,
            attackRange = 25f,
            attackCooldown = 1.2f,
            canHeal = false,
            hasRangedAttack = true,
            projectilePrefab = mageFireballPrefab,
            projectileSpeed = 20f,
            classColor = new Color(0.2f, 0.2f, 0.8f) // Blue
        };
        
        // Cleric - Healer/Support
        playerClass.clericClass = new PlayerClassData
        {
            classType = ClassType.Cleric,
            className = "Cleric",
            description = "Support class with healing",
            maxHealth = 120,
            moveSpeed = 11f,
            attackDamage = 12f,
            attackRange = 15f,
            attackCooldown = 1.0f,
            canHeal = true,
            healAmount = 20,
            healCooldown = 5f,
            hasRangedAttack = true,
            projectilePrefab = clericLightPrefab,
            projectileSpeed = 18f,
            classColor = new Color(0.9f, 0.9f, 0.2f) // Yellow/Gold
        };
        
        // Setup references
        playerClass.playerHealth = playerInstance.GetComponent<PlayerHealth>();
        playerClass.playerMovement = playerInstance.GetComponent<PlayerMovement>();
        
        // Setup projectile spawn point (front of player)
        Transform spawnPoint = playerInstance.transform.Find("ProjectileSpawnPoint");
        if (spawnPoint == null)
        {
            GameObject spawnObj = new GameObject("ProjectileSpawnPoint");
            spawnObj.transform.parent = playerInstance.transform;
            spawnObj.transform.localPosition = new Vector3(0, 1.5f, 1f); // In front and at chest height
            playerClass.projectileSpawnPoint = spawnObj.transform;
        }
        else
        {
            playerClass.projectileSpawnPoint = spawnPoint;
        }
    }
    
    /// <summary>
    /// Can be called to change class mid-game (optional)
    /// </summary>
    public void ChangePlayerClass(ClassType newClass)
    {
        if (playerInstance != null)
        {
            PlayerClass playerClass = playerInstance.GetComponent<PlayerClass>();
            if (playerClass != null)
            {
                playerClass.SetClass(newClass);
                Debug.Log($"Class changed to {newClass}");
            }
        }
    }
}
