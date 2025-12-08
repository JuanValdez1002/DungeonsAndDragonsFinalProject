using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public Image[] slots;   // UI images (already assigned by you)
    public ItemPickup[] inventory = new ItemPickup[7];  // actual stored items

    private int nextIndex = 0;

    void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemPickup item)
    {
        if (nextIndex >= inventory.Length)
        {
            Debug.Log("Inventory full");
            return;
        }

        // store item
        inventory[nextIndex] = item;

        // show icon
        slots[nextIndex].sprite = item.icon;
        slots[nextIndex].color = Color.white;

        Debug.Log("Picked up: " + item.itemName);

        nextIndex++;
    }
}
