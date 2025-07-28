using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private GameObject closedChestVisual;
    [SerializeField] private GameObject openChestVisual;
    private bool isOpen = false;
    public Inventory chestInventory;
    private void Awake()
    {
        chestInventory = new Inventory(20);
    }
    public void Interact()
    {
        if (isOpen)
        {
            Hide();
        }
        else
        {
            Show();
        }
    }
    private void Show()
    {
        closedChestVisual.SetActive(false);
        openChestVisual.SetActive(true);
        isOpen = true;
        ChestManager.Instance.OpenChest(this);
    }
    private void Hide()
    {
        closedChestVisual.SetActive(true);
        openChestVisual.SetActive(false);
        isOpen = false;
        ChestManager.Instance.CloseChest();
    }
}
