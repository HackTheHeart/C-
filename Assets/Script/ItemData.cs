using UnityEngine;
public enum ItemType
{
    None,
    Tool,
    Seed,
    Placeable,
    Dish,
    Resources,
    RiverFish,
    SeaFish,
    MineralNuggets,
    MineralBars,
    Gemstone,
    GemstoneDust,
    Fruit,
    Vegetable
}
[CreateAssetMenu(fileName = "ItemsSO", menuName = "Inventory/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public int itemID;
    public Sprite itemSprite;
    public bool stackable;
    public ItemType itemType;
    [Header("Seed Data")]
    public CropData cropData;
    [Header("Sell Price")]
    public int sellPrice;
    [Header("Item Stats")]
    [TextArea(2, 4)] public string description;
    public int energy;
    public int health;
    public int speed;
}
