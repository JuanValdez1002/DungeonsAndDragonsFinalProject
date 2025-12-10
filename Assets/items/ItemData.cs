using UnityEngine;

public enum ItemType
{
    Food,
    Potion,
    Other
}

[CreateAssetMenu(menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    [Header("Basics")]
    public string itemName;
    public Sprite icon;
    public ItemType type = ItemType.Food;

    [Header("Effects")]
    public int healAmount = 0;          // how much HP this heals

    [Header("World Prefab")]
    public GameObject worldPrefab;      // pickup prefab (same one you place in rooms)
}
