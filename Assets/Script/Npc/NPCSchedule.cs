using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC/Schedule")]
public class NPCSchedule : ScriptableObject
{
    public List<ScheduleEntry> schedule;
    [System.Serializable]
    public struct ScheduleEntry
    {
        public string dayOfWeek;
        public float time;
        public Vector2 direction;
    }
    public Vector2 GetDirectionByTime(float currentTime)
    {
        foreach (var entry in schedule)
        {
            if (Mathf.FloorToInt(currentTime) == Mathf.FloorToInt(entry.time))
                return entry.direction;
        }
        return Vector2.zero;
    }  
}
