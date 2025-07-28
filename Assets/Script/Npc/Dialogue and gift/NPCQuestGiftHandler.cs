//using UnityEngine;

//public class QuestGiftHandler : MonoBehaviour
//{
//    public bool HasQuestToGift(string npcName, ItemData item)
//    {
//        return activeQuests.Any(q =>
//            !q.isCompleted &&
//            q.questType == QuestType.GiveGift &&
//            q.targetNPC == npcName);
//    }
//    public void CompleteGiftQuest(string npcName, ItemData item)
//    {
//        var quest = activeQuests.FirstOrDefault(q =>
//            !q.isCompleted &&
//            q.questType == QuestType.GiveGift &&
//            q.targetNPC == npcName);

//        if (quest != null)
//        {
//            quest.isCompleted = true;
//            Debug.Log($"Hoàn thành nhiệm vụ: {quest.questName}");
//        }
//    }

//}
