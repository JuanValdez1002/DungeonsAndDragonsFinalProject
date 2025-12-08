using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType type;
    public int healAmount; // for food
}

public enum ItemType
{
    Food,
    Key,
    Material,
    Other
}
