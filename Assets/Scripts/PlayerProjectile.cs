using UnityEngine;

/// <summary>
/// Simple projectile script for player attacks
/// </summary>
public class PlayerProjectile : MonoBehaviour
{
    public int damage = 10;
    public bool isExplosive = false;
    public float explosionRadius = 3f;
    
    private void OnTriggerEnter(Collider other)
    {
        // Don't hit the player
        if (other.CompareTag("Player"))
            return;
        
        // Check if we hit an enemy
        EnemyAi enemy = other.GetComponent<EnemyAi>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"Player projectile hit enemy for {damage} damage");
            
            // Explosive damage (Mage fireball)
            if (isExplosive)
            {
                DealExplosiveDamage();
            }
            
            Destroy(gameObject);
        }
        // Hit wall or other object
        else if (!other.isTrigger)
        {
            if (isExplosive)
            {
                DealExplosiveDamage();
            }
            Destroy(gameObject);
        }
    }
    
    private void DealExplosiveDamage()
    {
        // Find all enemies in radius
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, explosionRadius);
        
        foreach (Collider hit in hitColliders)
        {
            EnemyAi enemy = hit.GetComponent<EnemyAi>();
            if (enemy != null)
            {
                // Reduced damage for AOE
                enemy.TakeDamage(damage / 2);
                Debug.Log("Explosion hit enemy");
            }
        }
        
        // Visual feedback (could add particle effect here)
        Debug.Log("BOOM! Explosion!");
    }
    
    private void OnDrawGizmosSelected()
    {
        if (isExplosive)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, explosionRadius);
        }
    }
}
