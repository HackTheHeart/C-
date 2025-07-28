using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image itemImage;
    [SerializeField] private TMP_Text amountText;
    [SerializeField] private GameObject OnChosenImage;
    private Items currentItem;
    private int currentAmount;
    //public static event Action<ItemSlot> OnItemSlotClicked;
    //private void Start()
    //{
    //    GetComponent<Button>().onClick.AddListener(() => OnItemSlotClicked?.Invoke(this));
    //}
    public void SetItem(Sprite itemSprite, int amount)
    {
        if (itemSprite != null)
        {
            currentAmount = amount;
            itemImage.sprite = itemSprite;
            itemImage.gameObject.SetActive(true);
            if (amount > 1)
            {
                amountText.text = amount.ToString();
                amountText.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(false);
            }
        }
        else
        {
            ClearItem();
        }
    }
    public void SetItem(Sprite itemSprite, int amount, Items item)
    {
        currentItem = item;
        currentAmount = amount;
        SetItem(itemSprite, amount);
    }
    public void SetChosen(bool isChosen)
    {
        if (OnChosenImage != null)
        {
            OnChosenImage.SetActive(isChosen);
        }
    }
    public Items GetItem()
    {
        return currentItem;
    }
    public void ClearItem()
    {
        itemImage.gameObject.SetActive(false);
        amountText.gameObject.SetActive(false);
    }
    public void SetItemBack()
    {
        itemImage.gameObject.SetActive(true);
        amountText.gameObject.SetActive(true);
    }
    public int GetAmount() => currentAmount;
    public Image GetitemImage() => itemImage;
    public TMP_Text GetamountText() => amountText;
    public Sprite GetSprite()
    {
        return itemImage.sprite;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            TooltipUI.Instance.ShowTooltip(currentItem);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance.HideTooltip();
    }
}
