using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ShopItemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("UI References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image backgroundImage;
    [SerializeField] private Image coinImage;
    [SerializeField] private TMP_Text priceText;
    [SerializeField] private TMP_Text itemNameText;
    private Button button;
    private Color normalColor = Color.white;
    private Color hoverColor = new Color(1f, 1f, 0.8f);
    private string itemName;
    private int buyPrice;
    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClickBuy);
    }
    public void Setup(string displayName, Sprite sprite, int price)
    {
        itemName = NormalizeItemName(displayName);
        buyPrice = price;

        itemIcon.sprite = sprite;
        itemNameText.text = displayName;  
        priceText.text = price.ToString();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        button.image.color = hoverColor;
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        button.image.color = normalColor;
    }
    private void OnClickBuy()
    {
        if (MoneyManager.Instance.SpendMoney(buyPrice))
        {
            bool success = Player.Instance.AddToInventory(itemName, 1);
            if (!success)
            {
                MoneyManager.Instance.AddMoney(buyPrice);
            }
        }
    }
    private string NormalizeItemName(string name)
    {
        string noSpace = name.Replace(" ", "");
        if (string.IsNullOrEmpty(noSpace)) return noSpace;
        return char.ToUpper(noSpace[0]) + noSpace.Substring(1).ToLower();
    }
}
