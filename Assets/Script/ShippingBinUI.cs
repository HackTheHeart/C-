using UnityEngine;

public class UI_ShippingBin : MonoBehaviour
{
    public static UI_ShippingBin Instance { get; private set; }
    [SerializeField] private GameObject shippingBinUI;
    public UI_Inventory playerInventoryUI;
    [SerializeField] private ItemSlot sellSlot;
    private Items sellItem;
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
        shippingBinUI.SetActive(false);
        playerInventoryUI.ToggleInventory();
    }
    private void Start()
    {
        sellSlot.GetComponent<ItemSlotClickable>().OnSlotClicked += HandleSellSlotClick;
        RegisterItemSlotEvents();
    }
    private void OnEnable()
    {
        RegisterItemSlotEvents();
    }
    private void OnDestroy()
    {
        sellSlot.GetComponent<ItemSlotClickable>().OnSlotClicked -= HandleSellSlotClick;
        UnregisterItemSlotEvents();
    }
    public void ShowUI()
    {
        shippingBinUI.SetActive(true);
        playerInventoryUI.SetInventory(Player.Instance.GetInventory());
        playerInventoryUI.ToggleInventory();
        RegisterItemSlotEvents();
    }
    public void HideUI()
    {
        shippingBinUI.SetActive(false);
        playerInventoryUI.ToggleInventory();
        UnregisterItemSlotEvents();
    }
    private void RegisterItemSlotEvents()
    {
        foreach (Transform slotTransform in playerInventoryUI.itemSlotContainer)
        {
            ItemSlotClickable clickable = slotTransform.GetComponent<ItemSlotClickable>();
            if (clickable != null)
            {
                clickable.OnSlotClicked -= HandleItemSlotClick;
                clickable.OnSlotClicked += HandleItemSlotClick;
            }
        }
    }
    private void UnregisterItemSlotEvents()
    {
        foreach (Transform slotTransform in playerInventoryUI.itemSlotContainer)
        {
            ItemSlotClickable clickable = slotTransform.GetComponent<ItemSlotClickable>();
            if (clickable != null)
            {
                clickable.OnSlotClicked -= HandleItemSlotClick;
            }
        }
    }
    private void HandleItemSlotClick(ItemSlot clickedSlot)
    {
        Items selectedItem = clickedSlot.GetItem();
        if (selectedItem == null)
        {
            return;
        }

        int amountInSlot = clickedSlot.GetAmount(); 

        ItemData itemData = itemsAssets.Instance.GetItemData(selectedItem.itemName);
        if (itemData == null)
        {
            return;
        }

        if (itemData.itemType == ItemType.Tool)
        {
            return;
        }

        if (sellItem != null)
        {
            ShippingManager.Instance.AddItemToSell(sellItem);
        }
        Player.Instance.GetInventory().RemoveItem(selectedItem.itemName, amountInSlot);
        sellItem = new Items
        {
            itemName = selectedItem.itemName,
            amount = amountInSlot
        };
        sellSlot.SetItem(sellItem.GetSprite(), sellItem.amount, sellItem);
        playerInventoryUI.RefreshInventoryUI();
    }
    private void HandleSellSlotClick(ItemSlot clickedSlot)
    {
        if (sellItem != null)
        {
            bool addedBack = Player.Instance.AddOnlyToInventory(sellItem.itemName, sellItem.amount);
            if (addedBack)
            {
                sellItem = null;
                sellSlot.ClearItem();
                playerInventoryUI.RefreshInventoryUI();
            }
            else
            {
                Debug.Log("Inventory full! Cannot take item back.");
            }
        }
    }
    public void ConfirmSale()
    {
        if (sellItem != null)
        {
            ShippingManager.Instance.AddItemToSell(sellItem);
            sellItem = null;
            sellSlot.ClearItem();
        }
        ShippingManager.Instance.DebugPrintSalesNow();
        ShippingBin.Instance.CloseBin();
    }
}
