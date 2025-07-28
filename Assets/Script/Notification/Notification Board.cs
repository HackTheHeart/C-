using UnityEngine;
using TMPro;

public class SimpleNotification : MonoBehaviour
{
    public static SimpleNotification Instance;
    [SerializeField] private GameObject panel;
    [SerializeField] private TMP_Text messageText;
    [SerializeField] private float duration = 2f;
    private Coroutine hideCoroutine;
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
        panel.SetActive(false);
    }
    public void Show(string message)
    {
        messageText.text = message;
        panel.SetActive(true);
        if (hideCoroutine != null)
            StopCoroutine(hideCoroutine);

        hideCoroutine = StartCoroutine(HideAfterDelay());
    }
    private System.Collections.IEnumerator HideAfterDelay()
    {
        yield return new WaitForSeconds(duration);
        panel.SetActive(false);
    }
}
