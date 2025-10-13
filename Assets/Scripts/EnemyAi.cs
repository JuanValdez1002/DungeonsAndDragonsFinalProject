using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;
public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform player;
    public LayerMask WhatIsGround, WhatIsPlayer;
    public float health;

    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;

    //Attacking
    public float timeBetweenAttacks;
    bool alreadyAttacked;
    public GameObject projectile;

    //States
    public float sightRange, attackRange;
    public bool playerInSightRange, playerInAttackRange;

    private void Awake()
    {
        player = GameObject.Find("PlayerObj").transform;
        agent = GetComponent<NavMeshAgent>();
        
        // Debug NavMeshAgent setup
        if (agent == null)
        {
            Debug.LogError("NavMeshAgent component not found on " + gameObject.name);
        }
        else
        {
            Debug.Log($"NavMeshAgent found on {gameObject.name}. Enabled: {agent.enabled}");
        }
        
        if (player == null)
        {
            Debug.LogError("Player 'PlayerObj' not found!");
        }
    }

    private void Start()
    {
        // Check if agent is on NavMesh after scene is fully loaded
        if (agent != null)
        {
            Debug.Log($"Enemy {gameObject.name} - IsOnNavMesh: {agent.isOnNavMesh}, Position: {transform.position}");
            if (!agent.isOnNavMesh)
            {
                Debug.LogWarning($"Enemy {gameObject.name} is NOT on NavMesh! Try moving it to a valid NavMesh area.");
            }
        }
    }

    private void Update()
    {
        // Debug current state
        if (Time.frameCount % 60 == 0) // Log every 60 frames to avoid spam
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            Debug.Log($"Distance to Player: {distanceToPlayer:F2}, SightRange: {sightRange}, AttackRange: {attackRange}");
            Debug.Log($"Enemy State - PlayerInSight: {playerInSightRange}, PlayerInAttack: {playerInAttackRange}, WalkPointSet: {walkPointSet}");
            if (agent != null)
            {
                Debug.Log($"NavMeshAgent - IsOnNavMesh: {agent.isOnNavMesh}, HasPath: {agent.hasPath}, Velocity: {agent.velocity.magnitude:F2}");
            }
        }

        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, WhatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, WhatIsPlayer);

        if (!playerInSightRange && !playerInAttackRange) Patroling();
        if (playerInSightRange && !playerInAttackRange) ChasePlayer();
        if (playerInAttackRange && playerInSightRange) AttackPlayer();
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
        {
            if (agent != null && agent.isOnNavMesh)
            {
                agent.SetDestination(walkPoint);
                Debug.Log($"Enemy patrolling to: {walkPoint}, Distance: {Vector3.Distance(transform.position, walkPoint):F2}");
            }
            else
            {
                Debug.LogWarning("Cannot patrol - NavMeshAgent is not on NavMesh!");
            }
        }

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //WalkPointt reached
        if (distanceToWalkPoint.magnitude < 1f)
        {
            walkPointSet = false;
            Debug.Log("Walk point reached, searching for new point");
        }
    }

    private void SearchWalkPoint()
    {
        //Calculate random point in range
        float randomZ = Random.Range(-walkPointRange, walkPointRange);
        float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);

        Debug.Log($"Testing walk point: {walkPoint}, WhatIsGround LayerMask: {WhatIsGround.value}");
        
        if (Physics.Raycast(walkPoint, -transform.up, 2f, WhatIsGround))
        {
            walkPointSet = true;
            Debug.Log($"Valid walk point found: {walkPoint}");
        }
        else
        {
            Debug.Log($"Invalid walk point (no ground detected): {walkPoint}");
        }
    }

    private void ChasePlayer()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(player.position);
            Debug.Log($"Chasing player! Distance: {Vector3.Distance(transform.position, player.position):F2}");
        }
        else
        {
            Debug.LogWarning("Cannot chase player - NavMeshAgent is not on NavMesh!");
        }
    }

    private void AttackPlayer()
    {
        //Make sure enemy doesn't move
        agent.SetDestination(transform.position);

        transform.LookAt(player);

        if (!alreadyAttacked)
        {
            // Check if projectile prefab is assigned
            GameObject projectileInstance;
            if (projectile == null)
            {
                Debug.LogWarning($"No projectile prefab assigned to {gameObject.name}! Creating default projectile.");
                // Create a basic projectile programmatically
                projectileInstance = CreateDefaultProjectile();
            }
            else
            {
                //Attack with assigned prefab
                projectileInstance = Instantiate(projectile, transform.position + transform.forward * 1.0f, Quaternion.identity);
            }
            
            Debug.Log($"Projectile created at: {projectileInstance.transform.position}");
            Rigidbody rb = projectileInstance.GetComponent<Rigidbody>();
            
            // Add Rigidbody if it doesn't exist
            if (rb == null)
            {
                rb = projectileInstance.AddComponent<Rigidbody>();
                Debug.LogWarning("Projectile prefab missing Rigidbody! Adding one automatically.");
            }
            
            // Configure projectile physics to prevent pushing player
            rb.mass = 0.1f; // Very light projectile
            rb.linearDamping = 1.0f; // Some air resistance
            
            // Add Collider if it doesn't exist
            Collider projectileCollider = projectileInstance.GetComponent<Collider>();
            if (projectileCollider == null)
            {
                SphereCollider sphereCollider = projectileInstance.AddComponent<SphereCollider>();
                sphereCollider.radius = 0.1f;
                sphereCollider.material = null; // No physics material to reduce bounce
                Debug.LogWarning("Projectile prefab missing Collider! Adding SphereCollider automatically.");
            }
            
            // Make sure collider doesn't bounce off player
            if (projectileCollider != null)
            {
                projectileCollider.material = null; // Remove any physics material
            }
            
            // Add Projectile script if it doesn't exist
            if (projectileInstance.GetComponent<Projectile>() == null)
            {
                projectileInstance.AddComponent<Projectile>();
                Debug.LogWarning("Projectile prefab missing Projectile script! Adding automatically.");
            }
            
            // Launch projectile toward player
            Vector3 direction = (player.position - transform.position).normalized;
            rb.AddForce(direction * 32f, ForceMode.Impulse);
            rb.AddForce(transform.up * 2f, ForceMode.Impulse); // Slight upward arc

            alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private GameObject CreateDefaultProjectile()
    {
        // Create a basic sphere projectile programmatically
        GameObject projectile = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        projectile.name = "AutoProjectile";
        projectile.transform.position = transform.position + transform.forward * 1.0f;
        projectile.transform.localScale = Vector3.one * 0.2f; // Small sphere
        
        // Add and configure Rigidbody
        Rigidbody rb = projectile.AddComponent<Rigidbody>();
        rb.mass = 0.1f;
        rb.linearDamping = 1.0f;
        
        // Add Projectile script
        projectile.AddComponent<Projectile>();
        
        // Make it red for visibility
        Renderer renderer = projectile.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
        
        Debug.Log("Created default projectile programmatically");
        return projectile;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0) Invoke(nameof(DestroyEnemy), .5f);
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

}
