using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;
    public List<QuestData> activeQuests = new List<QuestData>();
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    public void AddQuest(QuestData quest)
    {
        if (!activeQuests.Exists(q => q.questId == quest.questId))
        {
            activeQuests.Add(quest);
            Debug.Log($"Added quest: {quest.questName}");
        }
    }
    public void MarkQuestComplete(string questId)
    {
        var quest = activeQuests.Find(q => q.questId == questId);
        if (quest != null && !quest.isCompleted)
        {
            quest.isCompleted = true;
            Debug.Log($"Quest completed: {quest.questName}");
        }
    }
    public bool IsQuestCompleted(string questId)
    {
        return activeQuests.Exists(q => q.questId == questId && q.isCompleted);
    }
}
