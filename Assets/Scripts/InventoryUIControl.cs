using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryCanvas;   // your InventoryCanvas
    public MouseMovement mouseLook;      // drag the MouseMovement component here

    bool isOpen = false;

    void Start()
    {
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false);

        LockCursor(true);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;

        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(isOpen);

        // Enable/disable mouse look and cursor
        LockCursor(!isOpen);
        if (mouseLook != null)
            mouseLook.enabled = !isOpen;
    }

    void LockCursor(bool locked)
    {
        if (locked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
