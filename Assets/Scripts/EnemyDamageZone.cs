using UnityEngine;

public class EnemyDamageZone : MonoBehaviour
{
    public int damage = 10;

    private bool hasHitPlayer = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasHitPlayer)
            {
                hasHitPlayer = true;

                PlayerHealth hp = other.GetComponent<PlayerHealth>();
                if (hp != null)
                {
                    hp.TakeDamage(damage);
                    Debug.Log("Player took damage.");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Reset when they separate so next touch can damage again
            hasHitPlayer = false;
        }
    }
}
