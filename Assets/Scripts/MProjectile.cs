using UnityEngine;

public class MProjectile : MonoBehaviour
{
    public int damage = 10;
    public float speed = 30f;
    public float lifetime = 3f;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.linearVelocity = transform.forward * speed;

        Destroy(gameObject, lifetime);
    }

    // --- HANDLE TRIGGER HITS (if any colliders are triggers) ---
    private void OnTriggerEnter(Collider other)
    {
        TryHitEnemy(other);
    }

    // --- HANDLE NORMAL COLLISION HITS (your enemy uses this) ---
    private void OnCollisionEnter(Collision collision)
    {
        TryHitEnemy(collision.collider);
    }

    void TryHitEnemy(Collider col)
    {
        // ignore player
        if (col.CompareTag("Player")) return;

        // hit ROOT object so any child collider works
        Transform root = col.transform.root;

        EnemyHealth enemy = root.GetComponent<EnemyHealth>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log("Enemy took damage: " + damage);

            if (enemy.currentHealth <= 0)
                Debug.Log("Enemy died!");
        }

        // always destroy projectile after ANY valid hit
        Destroy(gameObject);
    }
}
