using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryTooltip : MonoBehaviour
{
    public static InventoryTooltip Instance;

    public GameObject tooltipObject;
    public TextMeshProUGUI tooltipText;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string text, Vector3 position)
    {
        tooltipObject.SetActive(true);
        tooltipText.text = text;
        tooltipObject.transform.position = position;
    }

    public void Hide()
    {
        tooltipObject.SetActive(false);
    }
}
