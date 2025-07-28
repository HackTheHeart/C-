using UnityEngine;
using TMPro;

public class MailTooltip : MonoBehaviour
{
    public static MailTooltip Instance;
    [SerializeField] private GameObject tooltipPanel;
    [SerializeField] private TMP_Text tooltipText;
    void Awake()
    {
        Instance = this;
        tooltipPanel.SetActive(false);
    }
    public void Show(string message, Vector2 position)
    {
        tooltipPanel.SetActive(true);
        tooltipText.text = message;
        tooltipPanel.transform.position = position;
    }
    public void Hide()
    {
        tooltipPanel.SetActive(false);
    }
}
