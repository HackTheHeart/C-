using System.Collections.Generic;
using UnityEngine;

public class CaveProgressManager : MonoBehaviour
{
    public static CaveProgressManager Instance;
    private HashSet<int> unlockedFloors = new HashSet<int>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadProgress();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void UnlockFloor(int floor)
    {
        if (!unlockedFloors.Contains(floor))
        {
            unlockedFloors.Add(floor);
            PlayerPrefs.SetInt("Floor_" + floor, 1);
        }
    }
    public bool IsFloorUnlocked(int floor)
    {
        return PlayerPrefs.GetInt("Floor_" + floor, 0) == 1;
    }
    private void LoadProgress()
    {
        for (int i = 5; i <= 20; i += 5)
        {
            if (PlayerPrefs.GetInt("Floor_" + i, 0) == 1)
                unlockedFloors.Add(i);
        }
    }
    public bool HasUnlockedAnyFloor()
    {
        foreach (int floor in new int[] { 5, 10, 15, 20 })
        {
            if (IsFloorUnlocked(floor)) return true;
        }
        return false;
    }
}
