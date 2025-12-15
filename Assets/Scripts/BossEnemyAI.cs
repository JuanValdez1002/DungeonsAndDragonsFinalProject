using UnityEngine;
using UnityEngine.AI;

public class BossEnemyAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Ranges")]
    public float chaseRange = 15f;
    public float slamRange = 8f;

    [Header("Movement")]
    public float stopDistance = 2.5f;

    [Header("Attack")]
    public float slamCooldown = 5f;

    float slamTimer;

    NavMeshAgent agent;
    Animator anim;

    bool isSlamming;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        agent.stoppingDistance = stopDistance;
        agent.autoBraking = true;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
        }
    
    

    }

    void Update()
    {
        if (GameManager.IsGameOver)
        {
            agent.isStopped = true;
            anim.SetBool("ismoving", false);

            anim.speed = 0f;   // 🔥 FREEZE ANIMATION
            return;
        }
        else
        {
            anim.speed = 1f;   // ensure animation resumes normally
        }

        if (player == null || !agent.isOnNavMesh)
            return;

        slamTimer -= Time.deltaTime;

        if (isSlamming)
            return;

        float dist = Vector3.Distance(transform.position, player.position);

        // ---------- SLAM ----------
        if (dist <= slamRange && dist > agent.stoppingDistance && slamTimer <= 0f)
        {
            StartSlam();
            return;
        }

        // ---------- CHASE ----------
        if (dist <= chaseRange)
        {
            agent.isStopped = false;

            if (dist > agent.stoppingDistance)
            {
                agent.SetDestination(player.position);
                anim.SetBool("ismoving", true);
            }
            else
            {
                agent.isStopped = true;
                anim.SetBool("ismoving", false);
            }
        }
        // ---------- IDLE ----------
        else
        {
            agent.isStopped = true;
            anim.SetBool("ismoving", false);
        }
    }

    void StartSlam()
    {
        isSlamming = true;
        slamTimer = slamCooldown;

        agent.isStopped = true;
        anim.SetBool("ismoving", false);

        anim.SetTrigger("JumpAttack");
    }

    // 🔥 Call this from code when LAND animation finishes
    // (Animator state check or timer)
    public void EndSlam()
    {
        isSlamming = false;
    }
}
