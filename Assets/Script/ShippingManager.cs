using System.Collections.Generic;
using UnityEngine;

public class ShippingManager : MonoBehaviour
{
    public static ShippingManager Instance { get; private set; }
    private List<Items> soldItems = new List<Items>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Start()
    {
        GameTimeManager.Instance.OnNewDayStarted += CalculateEarnings;
    }
    public void AddItemToSell(Items item)
    {
        ItemData itemData = itemsAssets.Instance.GetItemData(item.itemName);
        if (itemData != null && itemData.itemType != ItemType.Tool)
        {
            soldItems.Add(item);
        }
        else
        {

        }
    }
    private void CalculateEarnings()
    {
        int totalGold = 0;
        Debug.Log("=== 🧾 Items Sold Today ===");
        foreach (Items item in soldItems)
        {
            ItemData itemData = itemsAssets.Instance.GetItemData(item.itemName);
            if (itemData != null)
            {
                int itemGold = itemData.sellPrice * item.amount;
                totalGold += itemGold;
                Debug.Log($"🔹 {item.itemName} x{item.amount} → 💰 {itemGold}g");
            }
            else
            {
                Debug.LogWarning($"⚠ Không tìm thấy dữ liệu cho {item.itemName}");
            }
        }
        Debug.Log($"=== 💵 Total Earned: {totalGold}g ===");
        if (totalGold > 0)
        {
            MoneyManager.Instance.AddMoney(totalGold);
        }
        soldItems.Clear();
    }
    public void DebugPrintSalesNow()
    {
        if (soldItems.Count == 0)
        {
            Debug.Log("📦 Chưa có vật phẩm nào được bán.");
            return;
        }
        int totalGold = 0;
        Debug.Log("=== 🧾 Items Sold So Far ===");
        foreach (Items item in soldItems)
        {
            ItemData data = itemsAssets.Instance.GetItemData(item.itemName);
            if (data != null)
            {
                int itemGold = data.sellPrice * item.amount;
                totalGold += itemGold;
                Debug.Log($"🔹 {item.itemName} x{item.amount} → 💰 {itemGold}g");
            }
        }
        Debug.Log($"=== 💰 Tổng tạm tính: {totalGold}g ===");
    }

}
