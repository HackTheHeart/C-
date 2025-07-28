using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Toolbar : MonoBehaviour
{
    private Inventory toolbar;
    [SerializeField] private List<ItemSlot> toolbarSlots;
    [SerializeField] private Image selectorImage;
    private int selectedSlotIndex = 0;
    private void Start()
    {
        RefreshToolbarUI();
        UpdateSelectorPosition();
    }
    public void SetToolbar(Inventory toolbar)
    {
        this.toolbar = toolbar;
        RefreshToolbarUI();
    }
    public void RefreshToolbarUI()
    {
        if (toolbar == null) return;
        List<Items> items = toolbar.GetItemsList();
        for (int i = 0; i < toolbarSlots.Count; i++)
        {
            if (i < items.Count)
            {
                toolbarSlots[i].SetItem(items[i].GetSprite(), items[i].amount, items[i]);
                toolbarSlots[i].SetChosen(i == selectedSlotIndex); 
            }
            else
            {
                toolbarSlots[i].ClearItem();
                toolbarSlots[i].SetChosen(false);
            }
        }
    }
    public void SelectSlot(int index)
    {
        if (index >= 0 && index < toolbarSlots.Count)
        {
            selectedSlotIndex = index;
            RefreshToolbarUI();
        }
    }
    private void UpdateSelectorPosition()
    {
        if (selectorImage != null)
        {
            selectorImage.transform.position = toolbarSlots[selectedSlotIndex].transform.position;
        }
    }
    public void SwapItemsInToolbar(int index1, int index2)
    {
        if (toolbar != null)
        {
            toolbar.SwapItems(index1, index2);
            RefreshToolbarUI();
        }
    }
}
