using System.Collections.Generic;
using UnityEngine;

public class UI_Chest : MonoBehaviour
{
    public static UI_Chest Instance;
    public GameObject chestUI;
    public GameObject chestUIcanvas;
    public Transform chestSlotParent;
    public UI_Inventory chestInventoryUI;
    private Inventory chestInventory;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        chestUIcanvas.SetActive(false);
    }
    public void ShowChestUI(Inventory chestInventory)
    {
        chestUIcanvas.SetActive(true);
        this.chestInventory = chestInventory;
        chestInventoryUI.SetInventory(Player.Instance.GetInventory());
        RefreshChestUI();
        chestInventoryUI.ToggleInventory();
    }
    public void HideChestUI()
    {
        chestUIcanvas.SetActive(false);
        chestInventoryUI.ToggleInventory();
    }
    public void RefreshChestUI()
    {
        if (chestInventory == null) return;
        for (int i = 0; i < chestSlotParent.childCount; i++)
        {
            InventorySlot slot = chestSlotParent.GetChild(i).GetComponent<InventorySlot>();
            if (i < chestInventory.GetItemsList().Count)
            {
                slot.SetItem(chestInventory.GetItemsList()[i], chestInventory.GetItemsList()[i].amount);
            }
            else
            {
                slot.ClearItem();
            }
        }
    }
    public Inventory GetChestInventory()
    {
        return chestInventory;
    }
    //public void SwapItemsInChest(int index1, int index2)
    //{
    //    if (chestInventory == null) return;
    //    List<Items> itemsList = chestInventory.GetItemsList();
    //    if (index1 < 0 || index1 >= itemsList.Count || index2 < 0 || index2 >= itemsList.Count) return;
    //    Items temp = itemsList[index1];
    //    itemsList[index1] = itemsList[index2];
    //    itemsList[index2] = temp;
    //    RefreshChestUI();
    //}
    public void SwapItemsInChest(int index1, int index2)
    {
        if (chestInventory == null) return;

        chestInventory.SwapItems(index1, index2);
        RefreshChestUI();
    }

}
