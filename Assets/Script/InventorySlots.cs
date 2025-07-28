using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    private ItemSlot itemSlot;
    private UI_Inventory uiInventory;
    private UI_Toolbar uiToolbar;
    private void Awake()
    {
        itemSlot = GetComponent<ItemSlot>();
        uiInventory = GetComponentInParent<UI_Inventory>();
        uiToolbar = GetComponentInParent<UI_Toolbar>();
    }
    public void OnDrop(PointerEventData eventData)
    {
        GameObject droppedImageObject = eventData.pointerDrag;
        ItemDragHandler dragHandler = droppedImageObject?.GetComponent<ItemDragHandler>();
        if (dragHandler != null)
        {
            InventorySlot fromSlot = dragHandler.GetComponentInParent<InventorySlot>();
            if (fromSlot == this) return;
            Inventory fromInventory = fromSlot.IsChestSlot() ? UI_Chest.Instance.GetChestInventory() :
                                     fromSlot.IsToolbarSlot() ? Player.Instance.Toolbar :
                                     Player.Instance.Inventory;
            Inventory toInventory = IsChestSlot() ? UI_Chest.Instance.GetChestInventory() :
                                 IsToolbarSlot() ? Player.Instance.Toolbar :
                                 Player.Instance.Inventory;
            int fromIndex = fromSlot.transform.GetSiblingIndex();
            int toIndex = transform.GetSiblingIndex();
            if (fromInventory == toInventory)
            {
                if (IsToolbarSlot())
                {
                    uiToolbar.SwapItemsInToolbar(fromIndex, toIndex);
                }
                else if (IsChestSlot())
                {
                    UI_Chest.Instance.SwapItemsInChest(fromIndex, toIndex);
                }
                else
                {
                    uiInventory.SwapItemsInInventory(fromIndex, toIndex);
                }
            }
            else
            {
                MoveItemBetweenInventories(fromInventory, toInventory, fromIndex, toIndex);
            }
            Player.Instance.GetUIInventory().RefreshInventoryUI();
            if (UI_Chest.Instance.chestInventoryUI != null && UI_Chest.Instance.chestInventoryUI.gameObject.activeInHierarchy)
            {
                UI_Chest.Instance.chestInventoryUI.RefreshInventoryUI();
            }
            UI_Chest.Instance.RefreshChestUI();
            if (UI_ShippingBin.Instance.playerInventoryUI != null && UI_ShippingBin.Instance.playerInventoryUI.gameObject.activeInHierarchy)
            {
                UI_ShippingBin.Instance.playerInventoryUI.RefreshInventoryUI();
            }    
             Player.Instance.GetUIToolbar().RefreshToolbarUI();
        }
    }
    private void MoveItemBetweenInventories(Inventory fromInventory, Inventory toInventory, int fromIndex, int toIndex)
    {
        List<Items> fromList = fromInventory.GetItemsList();
        List<Items> toList = toInventory.GetItemsList();
        if (fromIndex < 0 || fromIndex >= fromList.Count) return;
        Items itemToMove = fromList[fromIndex];
        ItemData itemData = itemsAssets.Instance.GetItemData(itemToMove.itemName);
        if (itemData == null) return;
        if (itemData.stackable)
        {
            foreach (Items targetItem in toList)
            {
                if (targetItem.itemName == itemToMove.itemName)
                {
                    targetItem.amount += itemToMove.amount;
                    fromInventory.RemoveItem(itemToMove.itemName, itemToMove.amount);
                    return;
                }
            }
        }

        if (toIndex < toList.Count)
        {
            Items itemInTargetSlot = toList[toIndex];
            fromList[fromIndex] = itemInTargetSlot;
            toList[toIndex] = itemToMove;
        }
        else
        {
            toInventory.AddItem(itemToMove.itemName, itemToMove.amount);
            fromInventory.RemoveItem(itemToMove.itemName, itemToMove.amount);
        }
    }
    private bool IsToolbarSlot()
    {
        return uiToolbar != null;
    }
    private bool IsChestSlot()
    {
        return GetComponentInParent<UI_Chest>() != null;
    }
    public void SetItem(Items item, int amount)
    {
        itemSlot.SetItem(item?.GetSprite(), amount, item);
    }
    public void ClearItem()
    {
        itemSlot.ClearItem();
    }
    public Items GetItem()
    {
        return itemSlot.GetItem();
    }
    public Sprite GetSprite()
    {
        return itemSlot.GetSprite();
    }
    public int GetAmount()
    {
        return itemSlot.GetAmount();
    }
}
