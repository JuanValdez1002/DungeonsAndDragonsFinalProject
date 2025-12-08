using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public GameObject inventoryBar;

    void Start()
    {
        inventoryBar.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            inventoryBar.SetActive(!inventoryBar.activeSelf);
        }
    }
}
