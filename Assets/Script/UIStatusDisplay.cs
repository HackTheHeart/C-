using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStatusDisplay : MonoBehaviour
{
    [SerializeField] private TMP_Text textCurrentFunds;
    [SerializeField] private TMP_Text textTotalEarnings;
    [SerializeField] private TMP_Text textDateInfo;
    void Start()
    {
        UpdateStatus();
        MoneyManager.Instance.OnMoneyChanged += (_) => UpdateStatus();
        GameTimeManager.Instance.OnNewDayStarted += UpdateStatus;
    }
    void OnDestroy()
    {
        if (MoneyManager.Instance != null)
            MoneyManager.Instance.OnMoneyChanged -= (_) => UpdateStatus();
        if (GameTimeManager.Instance != null)
            GameTimeManager.Instance.OnNewDayStarted -= UpdateStatus;
    }
    void UpdateStatus()
    {
        var money = MoneyManager.Instance;
        var time = GameTimeManager.Instance;
        textCurrentFunds.text = $"{money.CurrentMoney}g";
        textTotalEarnings.text = $"{money.TotalEarnings}g";
        string seasonName = GetSeasonName(time.Season);
        textDateInfo.text = $"Day {time.Day} of {seasonName}, Year {time.Year}";
    }
    string GetSeasonName(int season)
    {
        switch (season)
        {
            case 0: return "Spring";
            case 1: return "Summer";
            case 2: return "Fall";
            case 3: return "Winter";
            default: return "Unknown";
        }
    }
}
