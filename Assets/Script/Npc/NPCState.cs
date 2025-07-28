//using System;
//using UnityEngine;
//using System.Collections.Generic;

//[Serializable]
//public class NPCState
//{
//    public string npcId;                    
//    public string currentScene;             
//    public Vector3 currentPosition;         
//    public int currentScheduleIndex = 0;    
//    public float timeSinceLastStep = 0f;    
//    public int friendshipPoints = 0;        
//    public int giftsThisWeek = 0;
//    public int lastGiftedWeek = -1;
//    public List<NPCScheduleEntry> schedule = new List<NPCScheduleEntry>(); 
//    public void UpdateSchedule(float deltaTime)
//    {
//        timeSinceLastStep += deltaTime;
//        if (schedule == null || schedule.Count == 0)
//            return;
//        float currentTime = GameTimeManager.Instance.TimeOfDay;
//        for (int i = 0; i < schedule.Count; i++)
//        {
//            var entry = schedule[i];
//            if (Mathf.Abs(currentTime - entry.timeOfDay) < 0.05f)
//            {
//                currentScheduleIndex = i;
//                if (entry.sceneName != currentScene)
//                {
//                    NPCWarpManager.Instance.WarpNPC(npcId, entry.sceneName, entry.position);
//                    return;
//                }
//                currentPosition = entry.position;
//            }
//        }
//    }
//}
