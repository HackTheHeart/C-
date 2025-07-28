using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AnimalData
{
    public string animalID;
    public string animalType;
    public Vector3 position;  
    public int mood;
    public int friendship;
    public int foodCountToday;

    public bool hasEatenToday;
    public bool isBaby;
    public float growthProgress;

    public string productPrefabName;
    public string maxFriendshipProductPrefabName;
}
[System.Serializable]
public class AnimalDailyState
{
    public string id;
    public int mood;
    public int friendship;
    public bool wasFed;
}

public class AnimalDailyManager : MonoBehaviour
{
    public static AnimalDailyManager Instance;
    public List<AnimalDailyState> dailyStates = new List<AnimalDailyState>();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void SaveAnimalState(Animal animal)
    {
        var data = new AnimalDailyState
        {
            id = animal.name,
            mood = animal.mood,
            friendship = animal.friendship,
            wasFed = animal.HasEatenToday
        };

        dailyStates.RemoveAll(a => a.id == data.id);
        dailyStates.Add(data);
    }
    public List<AnimalDailyState> GetAllStates() => dailyStates;
    public void ClearStates() => dailyStates.Clear();
}
