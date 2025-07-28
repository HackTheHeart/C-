using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlotClickable : MonoBehaviour, IPointerClickHandler
{
    private ItemSlot itemSlot;
    public System.Action<ItemSlot> OnSlotClicked;
    private void Awake()
    {
        itemSlot = GetComponent<ItemSlot>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (itemSlot != null)
        {
            OnSlotClicked?.Invoke(itemSlot);
        }
    }
}
