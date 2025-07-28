using UnityEngine;

public class ChestManager : MonoBehaviour
{
    public static ChestManager Instance;
    private Chest currentChest;
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
    public void OpenChest(Chest chest)
    {
        if (chest == null) return;
        currentChest = chest;
        UI_Chest.Instance.ShowChestUI(currentChest.chestInventory);
    }
    public void CloseChest()
    {
        UI_Chest.Instance.HideChestUI();
        currentChest = null;
    }
    public Inventory GetCurrentChestInventory()
    {
        return currentChest?.chestInventory;
    }
}