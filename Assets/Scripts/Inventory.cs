using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("Inventory Settings")]
    public int maxSlots = 7;

    [Header("UI Slots")]
    public Image[] slotImages;        // Drag ItemIcon images here

    private ItemData[] items;         // Stored items

    void Awake()
    {
        Instance = this;
        items = new ItemData[maxSlots];

        // Clear UI at start
        for (int i = 0; i < slotImages.Length; i++)
        {
            slotImages[i].enabled = false;
        }
    }

    // -------------------------
    // ADD ITEM TO INVENTORY
    // -------------------------
    public bool AddItem(ItemData item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                slotImages[i].sprite = item.icon;
                slotImages[i].enabled = true;
                return true;
            }
        }

        Debug.Log("Inventory Full!");
        return false;
    }

    // -------------------------
    // USE ITEM IN SLOT
    // -------------------------
    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= maxSlots) return;
        if (items[slotIndex] == null) return;

        ItemData item = items[slotIndex];

        switch (item.type)
        {
            case ItemType.Food:
                PlayerHealth player = FindObjectOfType<PlayerHealth>();
                player.TakeDamage(-item.healAmount); // negative damage = heal
                break;

            case ItemType.Key:
                Debug.Log("Used a key!");
                break;

            default:
                Debug.Log("Used item: " + item.itemName);
                break;
        }

        // Remove item after use
        items[slotIndex] = null;
        slotImages[slotIndex].enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UseItem(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) UseItem(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) UseItem(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) UseItem(3);
        if (Input.GetKeyDown(KeyCode.Alpha5)) UseItem(4);
        if (Input.GetKeyDown(KeyCode.Alpha6)) UseItem(5);
        if (Input.GetKeyDown(KeyCode.Alpha7)) UseItem(6);
    }
}
