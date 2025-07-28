using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MailUI : MonoBehaviour
{
    public static MailUI Instance;
    public GameObject mailButtonPrefab;
    public Transform mailListContainer;
    public TMP_Text contentText;
    void Awake()
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
    }
    private void Start()
    {
        PopulateMailList();
    }
    public void Open()
    {
        gameObject.SetActive(true);
        PopulateMailList();
    }
    void PopulateMailList()
    {
        foreach (Transform child in mailListContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var mail in MailManager.Instance.playerMails)
        {
            GameObject btn = Instantiate(mailButtonPrefab, mailListContainer);
            btn.GetComponent<MailButton>().Setup(mail, this);
        }
    }
    public void DisplayMail(MailData mail)
    {
        string playerName = Player.Instance.PlayerName;
        contentText.text = $"Dear {playerName},\n\n{mail.content}\n\nFrom: {mail.sender}";
    }
}
