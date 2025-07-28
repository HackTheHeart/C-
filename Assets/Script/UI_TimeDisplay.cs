using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_TimeDisplay : MonoBehaviour
{
    [SerializeField] private Image timeImage;
    [SerializeField] private TMP_Text timeText;
    [SerializeField] private TMP_Text dayText;
    [SerializeField] private Sprite[] timeSprites;
    [SerializeField] private Color normalColor = Color.white;
    [SerializeField] private Color midnightColor = Color.red;
    [SerializeField] private List<TMP_Text> moneyDigits;
    private void Start()
    {
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged += UpdateMoneyUI;
        UpdateMoneyUI(MoneyManager.Instance?.CurrentMoney ?? 500);
    }
    private void UpdateMoneyUI(int money)
    {
        string moneyString = money.ToString();
        for (int i = 0; i < moneyDigits.Count; i++)
        {
            int digitIndex = moneyString.Length - 1 - i;
            if (digitIndex >= 0)
            {
                moneyDigits[i].text = moneyString[digitIndex].ToString();
            }
            else
            {
                moneyDigits[i].text = "";
            }
        }
    }
    private void Update()
    {
        if (GameTimeManager.Instance == null) return;
        UpdateTimeDisplay();
    }
    private void UpdateTimeDisplay()
    {
        GameTimeManager timeManager = GameTimeManager.Instance;
        timeText.text = timeManager.GetFormattedTime();
        dayText.text = $"{timeManager.GetWeekDay()}. {timeManager.Day}";
        timeText.color = (timeManager.TimeOfDay >= 24f) ? midnightColor : normalColor;
        int timeIndex = GetTimeImageIndex(timeManager.TimeOfDay);
        timeImage.sprite = timeSprites[timeIndex];
    }
    private int GetTimeImageIndex(float timeOfDay)
    {
        if (timeOfDay >= 6f && timeOfDay < 14f) return 0;
        if (timeOfDay >= 14f && timeOfDay < 18f) return 1;
        return 2;
    }
}
