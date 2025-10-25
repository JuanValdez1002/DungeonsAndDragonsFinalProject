using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator playerAnimator;

    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 1f;

    private bool isAttacking = false;
    private float lastAttackTime = 0f;

    private void Update()
    {
        HandleAttackInput();
    }

    private void HandleAttackInput()
    {
        if (Input.GetMouseButtonDown(0) && !isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
        }
    }

    private void Attack()
    {
        isAttacking = true;
        lastAttackTime = Time.time;

        playerAnimator.SetTrigger("Attack");

        Invoke(nameof(ResetAttack), 0.8f);
    }

    private void ResetAttack()
    {
        isAttacking = false;
    }
}