using UnityEngine;

public class SwordHitbox : MonoBehaviour
{
    public int damage = 10;
    public Collider hitbox; // Assign sword's collider here
    private bool canDamage = false;

    private void Start()
    {
        // Make sure hitbox is off at start
        if (hitbox != null)
            hitbox.enabled = false;
    }

    public void EnableHitbox()
    {
        canDamage = true;
        if (hitbox != null)
            hitbox.enabled = true;

        Debug.Log("Hitbox ENABLED");
    }

    public void DisableHitbox()
    {
        canDamage = false;
        if (hitbox != null)
            hitbox.enabled = false;

        Debug.Log("Hitbox DISABLED");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!canDamage) return;

        if (other.CompareTag("Player"))
        {
            PlayerHealth hp = other.GetComponent<PlayerHealth>();
            if (hp != null)
            {
                hp.TakeDamage(damage);
                Debug.Log("Player hit for " + damage);
            }
        }
    }
}
