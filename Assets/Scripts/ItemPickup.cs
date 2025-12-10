using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ItemPickup : MonoBehaviour
{
    public ItemData data;             // assign the ItemData asset in Inspector
    public float pickupDelay = 0.1f;  // small delay so dropped items aren’t grabbed instantly

    Collider col;

    void Awake()
    {
        col = GetComponent<Collider>();
        col.isTrigger = true;        // trigger is needed for pickup
    }

    void OnEnable()
    {
        // start with collider disabled briefly to avoid instant re-pickup
        if (pickupDelay > 0f && col != null)
        {
            col.enabled = false;
            Invoke(nameof(EnableCollider), pickupDelay);
        }
    }

    void EnableCollider()
    {
        if (col != null) col.enabled = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (data == null)
        {
            Debug.LogWarning("ItemPickup on " + name + " has no ItemData!");
            return;
        }

        bool added = InventoryManager.Instance.AddItem(data);
        if (added)
        {
            Debug.Log("Picked up: " + data.itemName);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("Inventory full!");
        }
    }
}
