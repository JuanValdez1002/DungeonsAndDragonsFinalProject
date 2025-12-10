using UnityEngine;

public class MProjectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5f;
    private bool hasHit = false;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;

        EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        if (enemy != null)
        {
            hasHit = true;
            enemy.TakeDamage(damage);
            Debug.Log("Enemy damaged for " + damage);
            Destroy(gameObject);
            return;
        }

        // Hit wall or anything else solid
        if (!other.isTrigger)
        {
            Destroy(gameObject);
        }
    }
}
