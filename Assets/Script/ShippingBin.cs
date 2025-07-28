using UnityEngine;

public class ShippingBin : MonoBehaviour
{
    public static ShippingBin Instance { get; private set; }
    [SerializeField] private GameObject closedBinVisual;
    [SerializeField] private GameObject openBinVisual;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private bool isOpen = false;
    private void Start()
    {
        CloseBin();
    }
    public void Interact()
    {
        if (isOpen)
        {
            CloseBin();
        }
        else
        {
            OpenBin();
        }
    }
    private void OpenBin()
    {
        closedBinVisual.SetActive(false);
        openBinVisual.SetActive(true);
        isOpen = true;
        UI_ShippingBin.Instance.ShowUI();
    }
    public void CloseBin()
    {
        closedBinVisual.SetActive(true);
        openBinVisual.SetActive(false);
        isOpen = false;
        UI_ShippingBin.Instance.HideUI();
    }
}
