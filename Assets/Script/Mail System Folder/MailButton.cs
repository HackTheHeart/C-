using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class MailButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text titleText;
    private MailData mailData;
    private MailUI mailUI;
    public void Setup(MailData data, MailUI ui)
    {
        mailData = data;
        mailUI = ui;
        GetComponent<Button>().onClick.AddListener(() =>
        {
            Debug.Log("IsPressed");
            mailUI.DisplayMail(mailData);
        });
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        MailTooltip.Instance.Show(mailData.title, Input.mousePosition);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        MailTooltip.Instance.Hide();
    }
}
