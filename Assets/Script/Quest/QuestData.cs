public enum QuestType { MeetNPC, GiveGift }

[System.Serializable]
public class QuestData
{
    public string questId;
    public string questName;
    public string description;
    public QuestType questType;
    public string targetNPC; 
    public bool isCompleted;
}
