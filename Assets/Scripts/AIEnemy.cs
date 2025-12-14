using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIEnemySimple : MonoBehaviour
{
    public Transform player;

    [Header("Movement")]
    public float moveSpeed = 3.5f;
    public float stopDistance = 1.5f;

    [Header("Combat")]
    public float attackRange = 1.5f;
    public float attackCooldown = 2f;
    float attackTimer;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;

    int patrolIndex = 0;
    float patrolTimer;

    [Header("Detection")]
    public float chaseRange = 10f;

    NavMeshAgent agent;
    Animator anim;

    bool isAttacking = false;

    enum EnemyState { Patrol, Chase, Attack }
    EnemyState currentState = EnemyState.Patrol;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }

        agent.speed = moveSpeed;

        if (!agent.isOnNavMesh)
            StartCoroutine(WaitForNavmesh());
    }

    void Update()
    {
        if (player == null || !agent.isOnNavMesh) return;

        attackTimer -= Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.position);

        // -------- STATE SWITCH --------
        if (distance <= attackRange)
            currentState = EnemyState.Attack;
        else if (distance <= chaseRange)
            currentState = EnemyState.Chase;
        else
            currentState = EnemyState.Patrol;

        // -------- STATE EXECUTION --------
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;

            case EnemyState.Chase:
                ChasePlayer();
                break;

            case EnemyState.Attack:
                AttackPlayer();
                break;
        }
    }

    // ---------------- PATROL ----------------
    void Patrol()
    {
        if (patrolPoints.Length == 0) return;

        agent.isStopped = false;
        anim.SetBool("isWalking", true);

        agent.SetDestination(patrolPoints[patrolIndex].position);

        float dist = Vector3.Distance(transform.position, patrolPoints[patrolIndex].position);
        if (dist < 0.5f)
        {
            patrolTimer += Time.deltaTime;
            if (patrolTimer >= patrolWaitTime)
            {
                patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
                patrolTimer = 0f;
            }
        }
    }

    // ---------------- CHASE ----------------
    void ChasePlayer()
    {
        agent.isStopped = false;
        anim.SetBool("isWalking", true);
        agent.SetDestination(player.position);
    }

    // ---------------- ATTACK ----------------
    void AttackPlayer()
    {
        if (isAttacking || attackTimer > 0f) return;

        isAttacking = true;
        attackTimer = attackCooldown;

        agent.isStopped = true;
        anim.SetBool("isWalking", false);

        int attackID = Random.Range(0, 3);
        anim.SetInteger("AttackID", attackID);
        anim.SetBool("DoAttack", true);

        Invoke(nameof(ResetAttack), 1.0f);
    }

    void ResetAttack()
    {
        anim.SetBool("DoAttack", false);
        anim.SetInteger("AttackID", -1);
        isAttacking = false;
    }

    // ---------------- DEBUG ----------------
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    IEnumerator WaitForNavmesh()
    {
        while (!agent.isOnNavMesh)
            yield return null;

        agent.speed = moveSpeed;
    }
}
