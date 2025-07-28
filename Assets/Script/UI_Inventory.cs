using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Inventory : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private GameObject itemSlotTemplate;
    [SerializeField] public Transform itemSlotContainer;
    private bool isInventoryOpen = false;
    private int rows = 4;
    private int columns = 4;
    public bool IsOpen()
    {
        return isInventoryOpen;
    }

    private void Start()
    {
        //gameObject.SetActive(false);
    }
    public void SetInventory(Inventory inventory)
    {
        this.inventory = inventory;
        RefreshInventoryUI();
    }
    public void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen;
        gameObject.SetActive(isInventoryOpen);
        if (isInventoryOpen)
        {
            CreateInventorySlots();
            RefreshInventoryUI();
        }
        else
        {
            ClearInventorySlots();
        }
    }
    public void ForceClose()
    {
        if (isInventoryOpen)
        {
            HideInventoryUI();
        }
    }
    private void CreateInventorySlots()
    {
        if (itemSlotContainer.childCount > 0) return;
        for (int i = 0; i < rows * columns; i++)
        {
            GameObject itemSlot = Instantiate(itemSlotTemplate, itemSlotContainer);
            itemSlot.SetActive(true);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(itemSlotContainer.GetComponent<RectTransform>());
    }
    public void RefreshInventoryUI()
    {
        if (inventory == null) return;
        List<Items> items = inventory.GetItemsList();
        for (int i = 0; i < itemSlotContainer.childCount; i++)
        {
            Transform itemSlotTransform = itemSlotContainer.GetChild(i);
            ItemSlot itemSlotScript = itemSlotTransform.GetComponent<ItemSlot>();
            if (i < items.Count && items[i] != null)
            {
                itemSlotScript.SetItem(items[i].GetSprite(), items[i].amount, items[i]);
            }
            else
            {
                itemSlotScript.ClearItem();
            }
        }
    }

    private void ClearInventorySlots()
    {
        foreach (Transform child in itemSlotContainer)
        {
            Destroy(child.gameObject);
        }
    }
    //public void RefreshInventoryUI()
    //{
    //    if (inventory == null) return;

    //    List<Items> items = inventory.GetItemsList();
    //    for (int i = 0; i < itemSlotContainer.childCount; i++)
    //    {
    //        Transform itemSlotTransform = itemSlotContainer.GetChild(i);
    //        ItemSlot itemSlotScript = itemSlotTransform.GetComponent<ItemSlot>();

    //        if (i < items.Count && items[i] != null)
    //        {
    //            itemSlotScript.SetItem(items[i].GetSprite(), items[i].amount);
    //        }
    //        else
    //        {
    //            itemSlotScript.ClearItem();
    //        }
    //    }
    //}

    public void SwapItemsInInventory(int index1, int index2)
    {
        if (inventory != null)
        {
            inventory.SwapItems(index1, index2);
            RefreshInventoryUI();
        }
    }
    public void ShowInventoryUI()
    {
        isInventoryOpen = true;
        gameObject.SetActive(true);
        CreateInventorySlots();
        RefreshInventoryUI();
    }
    public void HideInventoryUI()
    {
        isInventoryOpen = false;
        gameObject.SetActive(false);
        ClearInventorySlots();
    }
}
