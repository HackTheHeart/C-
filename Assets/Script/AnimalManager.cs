using UnityEngine;
using System.Collections.Generic;

public class AnimalManager : MonoBehaviour
{
    public static AnimalManager Instance { get; private set; }
    [SerializeField] private List<AnimalGrowthData> animalGrowthDataList;
    private Dictionary<string, (GameObject adultPrefab, float growthTime)> babyDataMap = new();
    private void Awake()
    {
        if (Instance == null) { 
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else Destroy(gameObject);
        foreach (var data in animalGrowthDataList)
        {
            if (data.babyPrefab != null && data.adultPrefab != null)
            {
                string name = data.babyPrefab.name;
                babyDataMap[name] = (data.adultPrefab, data.growthTime);
            }
        }
    }
    public GameObject GetAdultVersion(string babyName)
    {
        return babyDataMap.TryGetValue(babyName, out var data) ? data.adultPrefab : null;
    }
    public float GetGrowthTime(string babyName)
    {
        return babyDataMap.TryGetValue(babyName, out var data) ? data.growthTime : 5f;
    }
}
