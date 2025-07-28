using UnityEngine;
using System;

public class GameTimeManager : MonoBehaviour
{
    public static GameTimeManager Instance;
    public event Action OnNewDayStarted;
    [SerializeField] private float dayLengthInMinutes = 13.5f;
    [SerializeField] private float timeOfDay = 6f;
    [SerializeField] private int day = 1;
    [SerializeField] private int season = 0;
    [SerializeField] private int year = 1;
    private float secondsInDay;
    private float timeAccumulator = 0f;
    public float TimeOfDay => timeOfDay;
    public int Day => day; 
    public int Season => season;
    public int Year => year;
    private string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
    private string[] weekDays = { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
    public int GetWeekOfYear()
    {
        return ((Season * 28) + (Day - 1)) / 7;
    }

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
    void Start()
    {
        secondsInDay = dayLengthInMinutes * 60f;
    }
    void Update()
    {
        timeAccumulator += Time.deltaTime;
        float timeStep = secondsInDay / (24f * 6f);
        if (timeAccumulator >= timeStep)
        {
            timeOfDay += 10f / 60f; 
            timeAccumulator = 0f;
        }
        if (timeOfDay >= 26f)
            StartNewDay();
    }
    private void StartNewDay()
    {
        timeOfDay = 6f;
        day++;
        if (day > 28)
        {
            day = 1;
            season++;
            if (season > 3)
            {
                season = 0;
                year++;
            }
        }
        OnNewDayStarted?.Invoke();
    }
    public void ForceNewDay()
    {
        timeOfDay = 26f;
    }
    public string GetFormattedTime()
    {
        int hour = Mathf.FloorToInt(timeOfDay);
        int minute = Mathf.FloorToInt((timeOfDay - hour) * 60);
        minute = Mathf.RoundToInt(minute / 10f) * 10;
        if (minute == 60)
        {
            hour += 1;
            minute = 0;
        }
        string ampm = hour >= 12 ? "PM" : "AM";
        hour = hour % 12;
        if (hour == 0) hour = 12;
        return $"{hour}:{minute:00} {ampm}";
    }
    public string GetWeekDay()
    {
        int index = (day - 1) % 7;
        return weekDays[index];
    }
}
