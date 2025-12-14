using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryCanvas;
    public MouseMovement mouseLook;
    public PlayerAttack playerAttack;
    public bool isOpen = false;

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
    Debug.Log("Inventory toggled. isOpen = " + isOpen);

    if (inventoryCanvas != null)
        inventoryCanvas.SetActive(isOpen);

    LockCursor(!isOpen);

    if (mouseLook != null)
        mouseLook.enabled = !isOpen;

    if (playerAttack != null)
        playerAttack.enabled = !isOpen;
}


    void LockCursor(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }
}
