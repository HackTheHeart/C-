using UnityEngine;

public class Mailbox : MonoBehaviour
{
    public GameObject newMailIcon;
    private void OnEnable()
    {
        GameTimeManager.Instance.OnNewDayStarted += CheckForNewMail;
    }
    private void OnDisable()
    {
        GameTimeManager.Instance.OnNewDayStarted -= CheckForNewMail;
    }
    void CheckForNewMail()
    {
        var newMails = MailManager.Instance.GetUnreadMails();
        newMailIcon.SetActive(newMails.Count > 0);
    }
    public void Interact()
    {
        var newMails = MailManager.Instance.GetUnreadMails();
        foreach (var mail in newMails)
        {
            MailManager.Instance.MarkAsRead(mail);
        }
        if (newMails.Count > 0)
        {
            newMailIcon.SetActive(false);
            MailUI.Instance.Open(); 
        }
    }
}
