using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CraftingIngredientUI : MonoBehaviour
{
    public Image icon;
    public TMP_Text nameText;
    public TMP_Text amountText;
    public void Setup(ItemData item, int amount)
    {
        icon.sprite = item.itemSprite;
        nameText.text = item.itemName;
        amountText.text = $"x{amount}";
    }
}
