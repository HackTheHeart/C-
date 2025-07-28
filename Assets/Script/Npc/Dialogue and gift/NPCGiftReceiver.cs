//using System.Collections.Generic;
//using UnityEngine;

//public class NPCGiftReceiver : MonoBehaviour
//{
//    public string npcName;
//    public List<ItemData> likedGifts;
//    public Animator facialAnimator;
//    public void ReceiveGift(ItemData gift)
//    {
//        if (likedGifts.Contains(gift))
//        {
//            facialAnimator.SetTrigger("Happy");
//        }
//        else
//        {
//            facialAnimator.SetTrigger("Neutral");
//        }
//        foreach (var quest in QuestManager.Instance.activeQuests)
//        {
//            if (quest.questType == QuestType.GiveGift &&
//                quest.targetNPC == npcName &&
//                !quest.isCompleted)
//            {
//                QuestManager.Instance.MarkQuestComplete(quest.questId);
//            }
//        }
//    }
//}
