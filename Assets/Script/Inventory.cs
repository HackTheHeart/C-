using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Inventory
{
    private List<Items> itemsList;
    private int maxItems;
    public event System.Action OnInventoryChanged;
    public Inventory(int maxItems = 36)
    {
        this.maxItems = maxItems;
        itemsList = new List<Items>
        {
        new Items { itemName = "Axe", amount = 1 },
        new Items { itemName = "Pickaxe", amount = 1 },
        new Items { itemName = "Hoe", amount = 1 },
        new Items { itemName = "Wateringcan", amount = 1 },
        new Items { itemName = "Fishingrod", amount = 1 },
        new Items { itemName = "Sword", amount = 1 },
        new Items { itemName = "Wood", amount = 100 },
        new Items { itemName = "Stone", amount = 100 },
        new Items { itemName = "Strawberryseed", amount = 5 },
        new Items { itemName = "Cauliflowerseed", amount = 5 },
        new Items { itemName = "Parsnip", amount = 10 },
        };
    }
    public bool AddItem(string itemName)
    {
        ItemData itemData = itemsAssets.Instance.GetItemData(itemName);
        if (itemData == null) return false;
        if (itemsList.Count >= maxItems) return false;
        if (itemData.stackable)
        {
            foreach (Items invItem in itemsList)
            {
                if (invItem.itemName == itemName)
                {
                    invItem.amount++;
                    return true;
                }
            }
        }
        itemsList.Add(new Items { itemName = itemName, amount = 1 });
        return true;
    }
    public bool RemoveItem(string itemName, int amount = 1)
    {
        foreach (Items invItem in itemsList)
        {
            if (invItem.itemName == itemName)
            {
                invItem.amount -= amount;
                if (invItem.amount <= 0)
                {
                    itemsList.Remove(invItem);
                    OnInventoryChanged?.Invoke();
                }
                return true;
            }
        }
        return false;
    }
    public List<Items> GetItemsList()
    {
        return itemsList;
    }
    //public void SwapItems(int index1, int index2)
    //{
    //    if (index1 < 0 || index2 < 0 || index1 >= itemsList.Count || index2 >= itemsList.Count) return;
    //    if (index2 >= itemsList.Count)
    //    {
    //        Items movedItem = itemsList[index1];
    //        itemsList.RemoveAt(index1);
    //        itemsList.Add(movedItem);
    //    }
    //    else
    //    {
    //        (itemsList[index1], itemsList[index2]) = (itemsList[index2], itemsList[index1]);
    //    }
    //}
    public void SwapItems(int index1, int index2)
    {
        if (index1 < 0 || index2 < 0 || index1 >= itemsList.Count || index2 >= itemsList.Count) return;
        Items item1 = itemsList[index1];
        Items item2 = itemsList[index2];
        ItemData data1 = itemsAssets.Instance.GetItemData(item1.itemName);
        ItemData data2 = itemsAssets.Instance.GetItemData(item2.itemName);
        if (data1 != null && data2 != null && data1.stackable && item1.itemName == item2.itemName)
        {
            item2.amount += item1.amount;
            if (index1 < index2)
                itemsList.RemoveAt(index1);
            else
                itemsList.RemoveAt(index2);
        }
        else
        {
            (itemsList[index1], itemsList[index2]) = (itemsList[index2], itemsList[index1]);
        }
        //ReorderInventory();
        OnInventoryChanged?.Invoke();
    }
    public void ReorderInventory()
    {
        itemsList = itemsList.Where(item => item != null && item.amount > 0).ToList();
        while (itemsList.Count < maxItems)
        {
            itemsList.Add(null);
        }
    }

    public void DebugInventory()
    {
        string inventoryContents = "Inventory Items: ";
        for (int i = 0; i < itemsList.Count; i++)
        {
            inventoryContents += $"[{i}]: {itemsList[i].itemName} (x{itemsList[i].amount}), ";
        }
        Debug.Log(inventoryContents);
    }
    public bool AddItem(string itemName, int amount)
    {
        if (amount <= 0) return false; 
        ItemData itemData = itemsAssets.Instance.GetItemData(itemName);
        if (itemData == null) return false;
        if (itemsList.Count >= maxItems) return false;
        if (itemData.stackable)
        {
            foreach (Items invItem in itemsList)
            {
                if (invItem.itemName == itemName)
                {
                    invItem.amount += amount;
                    return true;
                }
            }
        }
        itemsList.Add(new Items { itemName = itemName, amount = amount });
        return true;
    }
    public bool IsFull(string itemName = null)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            return itemsList.Count >= maxItems;
        }

        ItemData itemData = itemsAssets.Instance.GetItemData(itemName);
        if (itemData == null) return true;

        if (itemData.stackable)
        {
            foreach (Items invItem in itemsList)
            {
                if (invItem.itemName == itemName)
                {
                    return false;
                }
            }
        }
        return itemsList.Count >= maxItems;
    }
    public int GetItemAmount(string itemName)
    {
        return itemsList
            .Where(item => item != null && item.itemName == itemName)
            .Sum(item => item.amount);
    }
}
