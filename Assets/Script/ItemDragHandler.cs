using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    private int originalSiblingIndex;
    private CanvasGroup canvasGroup;
    private Image itemImage;
    private Vector3 originalPosition;
    private GameObject draggedImage;
    private Items draggedItem;
    private int draggedAmount;
    private Sprite draggedSprite;
    private Text amountText;
    private ItemSlot OverallItemslot;

    private void Awake()
    {
        itemImage = GetComponent<Image>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = transform.position;
        canvasGroup.blocksRaycasts = false;
        ItemSlot[] slots = originalParent.GetComponentsInChildren<ItemSlot>();
        ItemSlot slot = slots.FirstOrDefault(s => s.transform == transform);
        OverallItemslot = slot;
        if (slot != null)
        {
            draggedSprite = slot.GetSprite();
            draggedAmount = slot.GetAmount();
            slot.ClearItem();
        }
        else
        {
            Debug.LogError("slot is null! Cannot drag.");
        }
        if (draggedSprite == null)
        {
            Debug.LogError("Item sprite is null! Cannot drag.");
        }
        if (draggedAmount == 0)
        {
            Debug.LogError("Item amount is null! Cannot drag.");
        }
        else
        {
            Debug.Log("Dragged amount: " + draggedAmount);
        }
        Canvas canvas = FindFirstObjectByType<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Không tìm thấy Canvas! Hãy đảm bảo có Canvas trong Scene.");
        }
        draggedImage = new GameObject("DraggedImage");
        draggedImage.transform.SetParent(canvas.transform, false);
        GameObject amountTextGO = new GameObject("DraggedAmountText");
        amountTextGO.transform.SetParent(draggedImage.transform, false);

        TextMeshProUGUI amountText = amountTextGO.AddComponent<TextMeshProUGUI>();
        amountText.text = draggedAmount.ToString();
        amountText.fontSize = 28;
        amountText.alignment = TextAlignmentOptions.BottomRight;
        amountText.color = Color.white;
        amountText.raycastTarget = false;
        amountText.enabled = draggedAmount > 1;

        RectTransform textRT = amountText.GetComponent<RectTransform>();
        textRT.anchorMin = new Vector2(1f, 0f);
        textRT.anchorMax = new Vector2(1f, 0f);
        textRT.pivot = new Vector2(1f, 0f);
        textRT.anchoredPosition = new Vector2(30f, -3f);
        textRT.sizeDelta = new Vector2(60f, 30f);
        amountTextGO.transform.localScale = Vector3.one;

        Canvas draggedCanvas = draggedImage.AddComponent<Canvas>();
        draggedCanvas.overrideSorting = true;
        draggedCanvas.sortingOrder = 999;
        CanvasScaler scaler = draggedImage.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;
        draggedImage.AddComponent<GraphicRaycaster>();
        Image newImage = draggedImage.AddComponent<Image>();
        newImage.sprite = draggedSprite;
        newImage.raycastTarget = false;
        RectTransform rt = draggedImage.GetComponent<RectTransform>();
        rt.sizeDelta = slot.GetitemImage().rectTransform.sizeDelta;
        rt.anchorMin = new Vector2(0.5f, 0.5f);
        rt.anchorMax = new Vector2(0.5f, 0.5f);
        rt.position = originalPosition;
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (draggedImage != null)
        {
            RectTransform rt = draggedImage.GetComponent<RectTransform>();
            Vector3 worldPoint;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(
                FindFirstObjectByType<Canvas>().transform as RectTransform,
                Input.mousePosition,
                eventData.pressEventCamera, 
                out worldPoint
            );
            rt.position = worldPoint;
        }
    } 
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (draggedImage != null)
        {
            Destroy(draggedImage);
        }
        OverallItemslot.SetItemBack();
    }
    public Items GetDraggedItem()
    {
        return draggedItem;
    }
    public int GetDraggedAmount()
    {
        return draggedAmount;
    }
}
