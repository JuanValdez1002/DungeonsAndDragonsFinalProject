using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory")]
    public ItemData[] inventory = new ItemData[7];   // 7 slots

    [Header("UI")]
    public Image[] slotImages;      // drag Slot1..Slot7 Image components here

    [Header("Drop Settings")]
    public Transform player;        // player transform
    public Transform dropPoint;     // empty object in front of player (optional)

    Color clearColor = new Color(1f, 1f, 1f, 0f);

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        RefreshUI();
    }

    // ---------------- ADD ITEM ----------------
    public bool AddItem(ItemData data)
    {
        if (data == null) return false;

        for (int i = 0; i < inventory.Length; i++)
        {
            if (inventory[i] == null)
            {
                inventory[i] = data;
                Debug.Log("Item added: " + data.itemName);
                RefreshSlot(i);
                return true;
            }
        }

        Debug.Log("Inventory full!");
        return false;
    }

    // ---------------- USE ITEM (LEFT CLICK) ----------------
    public void UseItem(int index)
    {
        if (!IsValidSlot(index)) return;

        ItemData data = inventory[index];
        if (data == null) return;

        // Heal based on type
        switch (data.type)
        {
            case ItemType.Food:
            case ItemType.Potion:
                if (PlayerHealth.Instance != null)
                {
                    PlayerHealth.Instance.Heal(data.healAmount);
                    Debug.Log($"Used {data.itemName}, healed {data.healAmount}");
                }
                break;

            default:
                Debug.Log("Used item: " + data.itemName);
                break;
        }

        ClearSlot(index);
    }

    // ---------------- DROP ITEM (RIGHT CLICK) ----------------
    public void DropItem(int index)
    {
        if (!IsValidSlot(index)) return;

        ItemData data = inventory[index];
        if (data == null) return;

        if (player == null)
        {
            Debug.LogWarning("InventoryManager has no player assigned.");
            return;
        }

        // Choose drop position
        Vector3 dropPos;
        Quaternion dropRot = player.rotation;

        if (dropPoint != null)
        {
            dropPos = dropPoint.position;
        }
        else
        {
            dropPos = player.position + player.forward * 2f + Vector3.up * 0.5f;
        }

        if (data.worldPrefab != null)
        {
            Instantiate(data.worldPrefab, dropPos, dropRot);
            Debug.Log("Dropped: " + data.itemName);
        }
        else
        {
            Debug.LogWarning("No worldPrefab set on ItemData: " + data.itemName);
        }

        ClearSlot(index);
    }

    // ---------------- INTERNAL HELPERS ----------------

    void ClearSlot(int index)
    {
        if (!IsValidSlot(index)) return;

        inventory[index] = null;

        if (slotImages != null && index < slotImages.Length && slotImages[index] != null)
        {
            slotImages[index].sprite = null;
            slotImages[index].color = clearColor;
        }
    }

    void RefreshSlot(int index)
    {
        if (!IsValidSlot(index)) return;
        if (slotImages == null || index >= slotImages.Length || slotImages[index] == null) return;

        ItemData data = inventory[index];
        if (data == null)
        {
            slotImages[index].sprite = null;
            slotImages[index].color = clearColor;
        }
        else
        {
            slotImages[index].sprite = data.icon;
            slotImages[index].color = Color.white;
        }
    }

    public void RefreshUI()
    {
        for (int i = 0; i < inventory.Length; i++)
        {
            RefreshSlot(i);
        }
    }

    bool IsValidSlot(int index)
    {
        return index >= 0 && index < inventory.Length;
    }
}
