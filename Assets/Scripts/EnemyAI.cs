using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Detection & Movement")]
    public float detectionRadius = 15f;    // How far the enemy can detect the player
    public float moveSpeed = 3f;            // Movement speed
    public float attackRange = 1.5f;        // Range for attacking

    [Header("Combat")]
    public int damage = 10;                 // Damage dealt per attack
    public float attackCooldown = 1f;       // Delay between attacks

    private float lastAttackTime;
    private Transform player;
    private Rigidbody rb;
    private Dungeon3DGenerator dungeon;

    private int currentCellIndex = -1;      // Enemy’s current cell
    private List<int> currentPath = new List<int>(); // Path to follow
    private int currentPathStep = 0;        // Current node in the path

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        dungeon = Dungeon3DGenerator.Instance;
        rb = GetComponent<Rigidbody>();

        // Safety check
        if (rb == null)
        {
            Debug.LogWarning("EnemyAI requires a Rigidbody! Please add one.");
        }
        else
        {
            // Prevent the enemy from falling over
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
            rb.useGravity = true;
            rb.isKinematic = false;
        }

        currentCellIndex = GetClosestCellIndex();
    }

    void Update()
    {
        if (player == null || dungeon == null)
            return;

        float distance = Vector3.Distance(transform.position, player.position);

        // If player is within detection range, start following
        if (distance <= detectionRadius)
        {
            // Recalculate path if needed
            if (currentPath.Count == 0 || currentPathStep >= currentPath.Count)
            {
                currentPath = DijkstraPathToPlayer();
                currentPathStep = 0;
            }

            MoveAlongPath();

            // Attack if within range
            if (distance <= attackRange)
                AttackPlayer();
        }
    }

    // ------------------------------------------------------------
    // Moves the enemy along the current Dijkstra path
    // ------------------------------------------------------------
    void MoveAlongPath()
    {
        if (currentPath == null || currentPath.Count == 0)
            return;

        if (currentPathStep < 0 || currentPathStep >= currentPath.Count)
            return;

        int targetIdx = currentPath[currentPathStep];

        // Ensure this target cell exists and is part of the dungeon
        if (targetIdx < 0 || targetIdx >= dungeon.board.Count || !dungeon.board[targetIdx].visited)
            return;

        Vector3 targetPos = dungeon.IndexToWorld(targetIdx);
        targetPos.y += 1f; // Small offset to keep enemy above floor

        // Debug line (useful for visualizing in Scene view)
        Debug.DrawLine(transform.position, targetPos, Color.red);

        // Movement using Rigidbody (prevents clipping)
        if (rb != null)
        {
            rb.MovePosition(Vector3.MoveTowards(rb.position, targetPos, moveSpeed * Time.deltaTime));
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
        }

        // Smoothly rotate towards movement direction
        Vector3 direction = targetPos - transform.position;
        direction.y = 0f;
        if (direction.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }

        // Move to the next cell if close enough
        if (Vector3.Distance(transform.position, targetPos) < 0.3f)
        {
            currentPathStep++;
            currentCellIndex = targetIdx;
        }
    }

    // ------------------------------------------------------------
    // Handles attacking the player
    // ------------------------------------------------------------
    void AttackPlayer()
    {
        if (Time.time - lastAttackTime < attackCooldown)
            return;

        var playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.TakeDamage(damage);
            lastAttackTime = Time.time;
        }
    }

    // ------------------------------------------------------------
    // Finds the Dijkstra path from this enemy to the player
    // ------------------------------------------------------------
    List<int> DijkstraPathToPlayer()
    {
        int start = GetClosestCellIndex();
        int target = GetPlayerCellIndex();
        if (start == -1 || target == -1)
            return new List<int>();

        return dungeon.GetPath(start, target);
    }

    // ------------------------------------------------------------
    // Finds which cell this enemy is closest to
    // ------------------------------------------------------------
    int GetClosestCellIndex()
    {
        if (dungeon.board == null)
            return -1;

        float minDist = Mathf.Infinity;
        int idx = -1;

        for (int i = 0; i < dungeon.board.Count; i++)
        {
            Vector3 pos = dungeon.IndexToWorld(i);
            float dist = Vector3.Distance(transform.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                idx = i;
            }
        }

        return idx;
    }

    // ------------------------------------------------------------
    // Finds which cell the player is currently closest to
    // ------------------------------------------------------------
    int GetPlayerCellIndex()
    {
        if (dungeon.board == null)
            return -1;

        float minDist = Mathf.Infinity;
        int idx = -1;

        for (int i = 0; i < dungeon.board.Count; i++)
        {
            Vector3 pos = dungeon.IndexToWorld(i);
            float dist = Vector3.Distance(player.position, pos);
            if (dist < minDist)
            {
                minDist = dist;
                idx = i;
            }
        }

        return idx;
    }
}
