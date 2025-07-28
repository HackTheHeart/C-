//using UnityEngine;

//public class NPCInteraction : MonoBehaviour
//{
//    private NPC npc;
//    private NPCGiftReceiver giftReceiver;
//    private DialogueManager dialogueSystem;
//    private void Awake()
//    {
//        npc = GetComponent<NPC>();
//        giftReceiver = GetComponent<NPCGiftReceiver>();
//        dialogueSystem = FindFirstObjectByType<DialogueManager>();
//    }
//    public void Interact()
//    {
//        Items selectedItem = Player.Instance.GetSelectedItem();

//        if (selectedItem == null || selectedItem.amount <= 0 || selectedItem.GetItemType() == ItemType.Tool)
//        {
//            dialogueSystem.ShowDialogue(npc.npcName);
//        }
//        else
//        {
//            giftReceiver.ReceiveGift(itemsAssets.Instance.GetItemData(selectedItem.itemName));
//            Player.Instance.GetToolbar().RemoveItem(selectedItem.itemName, 1); 
//        }
//        npc.OnPlayerMet(); 
//    }
//    //public void Interact()
//    //{
//    //    npc.StopMoving();

//    //    Items selectedItem = Player.Instance.GetSelectedItem();
//    //    if (selectedItem == null || selectedItem.amount <= 0 || selectedItem.GetItemType() == ItemType.Tool)
//    //    {
//    //        dialogueSystem.ShowDialogue(npc.npcName);
//    //        Player.Instance.canMove = false;
//    //    }
//    //    else
//    //    {
//    //        giftReceiver.ReceiveGift(itemsAssets.Instance.GetItemData(selectedItem.itemName));
//    //        Player.Instance.GetToolbar().RemoveItem(selectedItem.itemName, 1);
//    //    }
//    //    npc.OnPlayerMet();
//    //}
//}
