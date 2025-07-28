using System.Collections.Generic;
using UnityEngine;

public class MailManager : MonoBehaviour
{
    public static MailManager Instance;
    public List<MailData> allMails = new List<MailData>();
    public List<MailData> playerMails = new List<MailData>();
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void AddMail(MailData mail)
    {
        allMails.Add(mail);
    }
    public List<MailData> GetUnreadMails()
    {
        return allMails.FindAll(mail =>
            !mail.isRead &&
            mail.day == GameTimeManager.Instance.Day &&
            mail.season == GameTimeManager.Instance.Season &&
            mail.year == GameTimeManager.Instance.Year);
    }
    public void MarkAsRead(MailData mail)
    {
        mail.isRead = true;
        if (!playerMails.Contains(mail))
            playerMails.Add(mail);
    }
}
