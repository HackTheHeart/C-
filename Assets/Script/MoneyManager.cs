using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance { get; private set; }
    public event Action<int> OnMoneyChanged;
    private int totalEarnings = 500;
    private int currentMoney = 500;
    public int CurrentMoney => currentMoney;
    public int TotalEarnings => totalEarnings;
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
    }
    public void AddMoney(int amount)
    {
        currentMoney += amount;
        totalEarnings += amount;
        OnMoneyChanged?.Invoke(currentMoney);
    }
    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            OnMoneyChanged?.Invoke(currentMoney);
            return true;
        }
        return false;
    }
}
