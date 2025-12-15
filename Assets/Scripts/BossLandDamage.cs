using UnityEngine;

public class BossLandDamage : StateMachineBehaviour
{
    [Header("Damage")]
    public float damageRadius = 6f;
    public int damage = 30;
    public LayerMask playerLayer;

    [Header("Camera Shake")]
    public bool shakeCamera = true;

    private bool hasTriggered;
    private CameraShake cameraShake;

    override public void OnStateEnter(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        if (hasTriggered) return;
        hasTriggered = true;

        Vector3 center = animator.transform.position;

        // Damage players in radius
        Collider[] hits = Physics.OverlapSphere(center, damageRadius, playerLayer);
        foreach (Collider hit in hits)
        {
            hit.GetComponent<PlayerHealth>()?.TakeDamage(damage);
        }

        // Camera shake
        if (shakeCamera)
        {
            if (cameraShake == null && Camera.main != null)
                cameraShake = Camera.main.GetComponent<CameraShake>();

            if (cameraShake != null)
                cameraShake.Shake();
        }
    }

    override public void OnStateExit(
        Animator animator,
        AnimatorStateInfo stateInfo,
        int layerIndex)
    {
        hasTriggered = false;

        //  Unlock boss AI
        BossEnemyAI boss = animator.GetComponentInParent<BossEnemyAI>();
        if (boss != null)
            boss.EndSlam();
    }
}
