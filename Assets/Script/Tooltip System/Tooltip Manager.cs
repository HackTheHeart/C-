using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    [Header("Main Tooltip Object")]
    [SerializeField] private RectTransform tooltipObject;

    [Header("Texts")]
    [SerializeField] private TMP_Text itemNameText;
    [SerializeField] private TMP_Text itemTypeText;
    [SerializeField] private TMP_Text descriptionText;

    [Header("Stat Groups")]
    [SerializeField] private GameObject energyGroup;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private GameObject healthGroup;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private GameObject sellGroup;
    [SerializeField] private TMP_Text sellText;

    private Canvas parentCanvas;
    private Vector2 padding = new Vector2(10f, 10f);

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
        parentCanvas = GetComponentInParent<Canvas>();
        HideTooltip();
    }

    private void Update()
    {
        if (tooltipObject.gameObject.activeSelf)
            FollowMouse();
    }

    private void FollowMouse()
    {
        Vector2 mousePosition = Input.mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentCanvas.transform as RectTransform,
            mousePosition,
            parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : parentCanvas.worldCamera,
            out Vector2 localPoint
        );

        Vector2 tooltipSize = tooltipObject.sizeDelta;
        Vector2 canvasSize = parentCanvas.GetComponent<RectTransform>().sizeDelta;

        Vector2 anchoredPosition = localPoint + padding;

        float pivotX = (anchoredPosition.x + tooltipSize.x > canvasSize.x / 2f) ? 1f : 0f;
        float pivotY = (anchoredPosition.y + tooltipSize.y > canvasSize.y / 2f) ? 1f : 0f;

        tooltipObject.pivot = new Vector2(pivotX, pivotY);
        tooltipObject.anchoredPosition = anchoredPosition;
    }

    public void ShowTooltip(Items item)
    {
        if (item == null) return;

        ItemData data = itemsAssets.Instance.GetItemData(item.itemName);
        if (data == null) return;

        itemNameText.text = data.itemName;
        itemTypeText.text = $"Type: {data.itemType}";
        descriptionText.text = data.description;

        energyGroup.SetActive(false);
        healthGroup.SetActive(false);
        sellGroup.SetActive(false);
        descriptionText.gameObject.SetActive(false);

        switch (data.itemType)
        {
            case ItemType.Tool:
            case ItemType.Placeable:
            case ItemType.Seed:
            case ItemType.Resources:
                descriptionText.gameObject.SetActive(true);
                break;

            case ItemType.RiverFish:
            case ItemType.SeaFish:
            case ItemType.Fruit:
            case ItemType.Vegetable:
                descriptionText.gameObject.SetActive(true);
                energyGroup.SetActive(true);
                healthGroup.SetActive(true);
                sellGroup.SetActive(true);

                energyText.text = data.energy.ToString();
                healthText.text = data.health.ToString();
                sellText.text = $"{data.sellPrice}g";
                break;

            case ItemType.MineralNuggets:
            case ItemType.MineralBars:
            case ItemType.Gemstone:
            case ItemType.GemstoneDust:
                descriptionText.gameObject.SetActive(true);
                sellGroup.SetActive(true);
                sellText.text = $"{data.sellPrice}g";
                break;
        }

        tooltipObject.gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        tooltipObject.gameObject.SetActive(false);
    }
}
