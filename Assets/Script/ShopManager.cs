using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] private Transform contentPanel;
    [SerializeField] private GameObject shopItemPrefab;

    private Dictionary<string, int> cropSellPrices = new Dictionary<string, int>()
    {
        {"Parsnip Seed", 45}, {"Potato Seed", 62}, {"Cauliflower Seed", 225},
        {"Strawberry Seed", 150}, {"Cabbage Seed", 125}, {"Carrot Seed", 62},
        {"Onion Seed", 75}, {"Springonion Seed", 31}, {"Broccoli Seed", 100},
        {"Wheat Seed", 31}, {"Blueberry Seed", 70}
    };

    private Dictionary<int, List<string>> seedsBySeason = new Dictionary<int, List<string>>()
    {
        { 0, new List<string> { "Parsnip Seed", "Potato Seed", "Cauliflower Seed", "Strawberry Seed", "Cabbage Seed", "Carrot Seed", "Onion Seed", "Springonion Seed", "Broccoli Seed", "Wheat Seed", "Blueberry Seed" } },
        { 1, new List<string> {} },
        { 2, new List<string> {} },
        { 3, new List<string>() }
    };

    private void Start()
    {
        PopulateShop();
    }

    private void PopulateShop()
    {
        int currentSeason = GameTimeManager.Instance.Season;
        Debug.Log($"[ShopUIManager] Current season: {currentSeason}");

        if (!seedsBySeason.ContainsKey(currentSeason))
        {
            Debug.LogWarning("[ShopUIManager] No seeds for current season.");
            return;
        }

        // Clear previous items
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (string seedName in seedsBySeason[currentSeason])
        {
            Debug.Log($"[ShopUIManager] Processing seed: {seedName}");

            if (!cropSellPrices.TryGetValue(seedName, out int sellPrice))
            {
                Debug.LogWarning($"[ShopUIManager] No sell price found for {seedName}");
                continue;
            }

            int buyPrice = Mathf.RoundToInt(sellPrice * 1.5f);
            string normalizedName = NormalizeItemName(seedName);

            var itemData = itemsAssets.Instance.GetItemData(normalizedName);
            if (itemData == null)
            {
                Debug.LogWarning($"[ShopUIManager] ItemData not found for normalized name: {normalizedName}");
                continue;
            }

            GameObject go = Instantiate(shopItemPrefab, contentPanel);
            ShopItemUI ui = go.GetComponent<ShopItemUI>();
            ui.Setup(seedName, itemData.itemSprite, buyPrice);

            Debug.Log($"[ShopUIManager] Spawned shop item for {seedName} at price {buyPrice}");
        }
    }
    private string NormalizeItemName(string name)
    {
        string noSpace = name.Replace(" ", "");
        if (string.IsNullOrEmpty(noSpace)) return noSpace;
        return char.ToUpper(noSpace[0]) + noSpace.Substring(1).ToLower();
    }
}
