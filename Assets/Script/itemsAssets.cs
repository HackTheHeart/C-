using System.Collections.Generic;
using UnityEngine;

public class itemsAssets : MonoBehaviour
{
    public static itemsAssets Instance { get; private set; }
    [SerializeField] private List<ItemData> itemDataList;
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
    public Sprite GetItemSprite(string itemName)
    {
        ItemData itemData = GetItemData(itemName);
        return itemData != null ? itemData.itemSprite : null;
    }
    public ItemData GetItemData(string itemName)
    {
        return itemDataList.Find(item => item.itemName == itemName);
    }
}
