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
    private float attackTimer = 0f;

    [Header("Debug / Gizmos")]
    public float chaseRange = 10f;

    private NavMeshAgent agent;
    private Animator anim;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        if (agent == null)
            Debug.LogError("NavMeshAgent missing on enemy.");
        if (anim == null)
            Debug.LogError("Animator missing on enemy child.");

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }

        agent.speed = moveSpeed;

        // SAFETY CHECK — ensures NavMeshAgent is ON the NavMesh
        if (!agent.isOnNavMesh)
            {
                StartCoroutine(WaitForNavmesh());
            }
    }


    void Update()
    {
        if (player == null) return;

        // SAFETY CHECK — prevents the NavMeshAgent spam error 
        if (!agent.isOnNavMesh)
        {
            anim.SetBool("isWalking", false);
            return;
        }

        attackTimer -= Time.deltaTime;
        float distance = Vector3.Distance(transform.position, player.position);

        // ATTACK LOGIC
        if (distance <= attackRange && attackTimer <= 0f && !isAttacking)
        {
            StartAttack();
            return;
        }

        // MOVEMENT LOGIC
        if (!isAttacking)
        {
            if (distance < chaseRange)
            {
                if (distance > stopDistance)
                {
                    agent.isStopped = false;
                    agent.SetDestination(player.position);
                    anim.SetBool("isWalking", true);
                }
                else
                {
                    agent.isStopped = true;
                    anim.SetBool("isWalking", false);
                }
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool("isWalking", false);
            }
        }
    }

    // -----------------------------
    // ATTACK SYSTEM (Matches Animator)
    // -----------------------------
    void StartAttack()
    {
        isAttacking = true;
        attackTimer = attackCooldown;

        agent.isStopped = true;
        anim.SetBool("isWalking", false);

        // Pick random attack
        int attackID = Random.Range(0, 3);  // 0=Slash01, 1=Slash02, 2=Stab
        anim.SetInteger("AttackID", attackID);

        // Trigger attack
        anim.SetBool("DoAttack", true);

        // Reset DoAttack shortly after (required for AnyState transitions)
        Invoke(nameof(ResetAttackTrigger), 0.05f);

        // End attack after animation (adjust length if needed)
        Invoke(nameof(EndAttack), 1.0f);
    }

    void ResetAttackTrigger()
    {
        anim.SetBool("DoAttack", false);
    }

    void EndAttack()
    {
        isAttacking = false;
        anim.SetInteger("AttackID", -1);
    }

    // ------------------------
    // GIZMOS FOR VISUAL DEBUG
    // ------------------------
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, chaseRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }


    IEnumerator WaitForNavmesh()
    {
        // Wait until the agent is properly placed on a NavMesh
        while (!agent.isOnNavMesh)
            yield return null;

        // Once safe, set movement speed (or any other initialization)
        agent.speed = moveSpeed;
    }

}
