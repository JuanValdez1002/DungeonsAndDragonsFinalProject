using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int damage = 10;
    public float lifetime = 5.0f;
    
    private bool hasHit = false; // Prevent multiple hits
    
    private void Start()
    {
        // Destroy projectile after lifetime to prevent accumulation
        Destroy(gameObject, lifetime);
        
        // Set up rigidbody for better collision detection
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        }
    }
    
    // Add trigger detection for better hit detection
    private void OnTriggerEnter(Collider other)
    {
        if (hasHit) return;
        HandleHit(other.gameObject);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (hasHit) return;
        HandleHit(collision.gameObject);
    }
    
    private void HandleHit(GameObject hitObject)
    {
        hasHit = true;
        
        // If we hit the player
        if (hitObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = hitObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log($"Player hit for {damage} damage! Distance detection worked!");
            }
            else
            {
                Debug.LogWarning("Player hit but no PlayerHealth component found!");
            }
            
            // Destroy projectile immediately to prevent physics interactions
            CreateImpactEffect();
            Destroy(gameObject);
            return;
        }
        
        // If we hit a wall, floor, or any solid object
        if (hitObject.layer == LayerMask.NameToLayer("Default") || 
            hitObject.name.Contains("Wall") || 
            hitObject.name.Contains("Mesh") ||
            hitObject.name.Contains("Floor"))
        {
            Debug.Log($"Projectile hit: {hitObject.name}");
            CreateImpactEffect();
            Destroy(gameObject);
        }
    }
    
    private void CreateImpactEffect()
    {
        // Optional: Add particle effect or sound here
        // For now, just a simple debug message
        Debug.Log("Projectile impact at: " + transform.position);
    }
}