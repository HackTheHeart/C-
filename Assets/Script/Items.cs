using UnityEngine;

public class Items
{
    public string itemName;
    public int amount;
    public Sprite GetSprite()
    {
        return itemsAssets.Instance.GetItemSprite(itemName);
    }
    public ItemType GetItemType()
    {
        ItemData itemData = itemsAssets.Instance.GetItemData(itemName);
        return itemData != null ? itemData.itemType : default;
    }
}
