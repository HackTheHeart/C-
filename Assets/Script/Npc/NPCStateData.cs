using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class NPCState
{
    public string npcName;
    public string currentScene;
    public Vector2 currentPosition;
    public int friendshipPoints = 0;
    public List<NPCScheduleEntry> schedule = new();

    public NPCState(string name, string startingScene, Vector2 startPos, List<NPCScheduleEntry> scheduleEntries)
    {
        npcName = name;
        currentScene = startingScene;
        currentPosition = startPos;
        schedule = scheduleEntries;
    }

    public void UpdateState(float timeOfDay, float deltaTime)
    {
        for (int i = schedule.Count - 1; i >= 0; i--)
        {
            var entry = schedule[i];
            if (timeOfDay >= entry.timeOfDay)
            {
                currentScene = entry.sceneName;
                currentPosition = entry.position;
                break;
            }
        }
    }
}
