using UnityEngine;
using UnityEngine.EventSystems;

public class SlotUI : MonoBehaviour, IPointerClickHandler
{
    [Tooltip("0 for Slot1, 1 for Slot2, ... 6 for Slot7")]
    public int index;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (InventoryManager.Instance == null) return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Debug.Log($"Slot clicked LEFT: {index}");
            InventoryManager.Instance.UseItem(index);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            Debug.Log($"Slot clicked RIGHT: {index}");
            InventoryManager.Instance.DropItem(index);
        }
    }
}
