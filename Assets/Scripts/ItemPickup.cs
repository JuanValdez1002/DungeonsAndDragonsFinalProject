using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    [Header("Item Settings")]
    public string itemName;
    public Sprite icon;  // <--- THIS creates the field in the Inspector

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InventoryManager.Instance.AddItem(this);
            Destroy(gameObject);
        }
    }
}
