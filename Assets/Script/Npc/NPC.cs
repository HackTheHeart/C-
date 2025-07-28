using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCRole { Vendor, Blacksmith, Regular }

public class NPC : NPCBase
{
    [Header("NPC Info")]
    [SerializeField] private LayerMask npcLayerMask;
    public string npcName;
    public NPCRole role;
    public bool isInteractable = true;
    public NPCDialogueSet dialogueSet;
    public NPCSchedule schedule;
    public UniversalGiftPreferences universalGiftPreferences; // assign trong Inspector
    [Header("NPC Schedule")]
    public List<NPCScheduleEntry> scheduleEntries = new();

    protected override void Awake()
    {
        base.Awake();
    }
    private void LateUpdate()
    {
        if (isInteractable)
        {
            //Vector2 targetDirection = schedule.GetDirectionByTime(GameTimeManager.Instance.TimeOfDay);
            //inputVector = targetDirection.normalized;
        }
    }
    protected override void Update()
    {
        base.Update(); // Từ NPCBase để xử lý animation và movement
        MoveAlongPathAStar(); // Di chuyển theo path nếu có
    }
    public void Interact()
    {
        Debug.Log("Interact() called on NPC: " + npcName);
        StopMoving();
        isInteractable = false;
        if (dialogueSet != null && dialogueSet.normalDialogues.Length > 0)
        {
            int randomIndex = Random.Range(0, dialogueSet.normalDialogues.Length);
            var group = dialogueSet.normalDialogues[randomIndex];

            // Debug toàn bộ nội dung của đoạn hội thoại
            Debug.Log($"[{npcName}] Chọn dialogue index: {randomIndex}");
            if (group.lines != null && group.lines.Length > 0)
            {
                for (int i = 0; i < group.lines.Length; i++)
                {
                    Debug.Log($"[{npcName}] Line {i}: {group.lines[i]}");
                }
            }
            else
            {
                Debug.LogWarning($"[{npcName}] group.lines rỗng hoặc null");
            }

            DialogueManager.Instance.StartDialogue(dialogueSet, group);
        }
        else
        {
            Debug.LogWarning($"[{npcName}] dialogueSet null hoặc không có normalDialogues!");
        }

        StartCoroutine(ReenableMovementAfterDialogue());
    }
    private System.Collections.IEnumerator ReenableMovementAfterDialogue()
    {
        while (DialogueManager.Instance.dialogueBox.activeSelf)
        {
            yield return null;
        }
        isInteractable = true;
    }
    public void OnPlayerMet()
    {
        foreach (var quest in QuestManager.Instance.activeQuests)
        {
            if (quest.questType == QuestType.MeetNPC &&
                quest.targetNPC == npcName &&
                !quest.isCompleted)
            {
                QuestManager.Instance.MarkQuestComplete(quest.questId);
            }
        }
    }

    // Gift 
    private int giftsThisWeek = 0;
    private int lastGiftedWeek = -1;
    public GiftPreferenceData giftPreferences;
    private int friendshipPoints = 0; 
    private int heartLevel => friendshipPoints / 250;
    //public void GiveGift(Items item)
    //{
    //    if (item == null || item.GetItemType() == ItemType.Tool) return;
    //    StopMoving();
    //    isInteractable = false;
    //    string itemName = item.itemName;
    //    string reaction = "normal";
    //    int pointGain = 0;
    //    if (giftPreferences.lovedItems.Contains(itemName))
    //    {
    //        reaction = "love";
    //        pointGain = 80;
    //    }
    //    else if (giftPreferences.likedItems.Contains(itemName))
    //    {
    //        reaction = "like";
    //        pointGain = 45;
    //    }
    //    else if (giftPreferences.hatedItems.Contains(itemName))
    //    {
    //        reaction = "dislike";
    //        pointGain = -25;
    //    }
    //    friendshipPoints = Mathf.Max(friendshipPoints + pointGain, 0);
    //    switch (reaction)
    //    {
    //        case "love":
    //            PlayDialogue(dialogueSet.loveDialogues);
    //            break;
    //        case "like":
    //            PlayDialogue(dialogueSet.likeDialogues);
    //            break;
    //        case "dislike":
    //            PlayDialogue(dialogueSet.dislikeDialogues);
    //            break;
    //        default:
    //            PlayDialogue(dialogueSet.normalDialogues);
    //            break;
    //    }
    //    item.amount -= 1;
    //    StartCoroutine(ReenableMovementAfterDialogue());
    //}
    public void GiveGift(Items item)
    {
        int currentWeek = GameTimeManager.Instance.GetWeekOfYear();
        if (lastGiftedWeek != currentWeek)
        {
            giftsThisWeek = 0;
            lastGiftedWeek = currentWeek;
        }
        if (giftsThisWeek >= 2)
        {
            Debug.Log($"{npcName} can't receive more gifts this week.");
            return;
        }
        if (item == null || item.GetItemType() == ItemType.Tool) return;
        StopMoving();
        isInteractable = false;
        string itemName = item.itemName;
        string reaction = "normal";
        int pointGain = 0;
        bool isLoved = giftPreferences != null && giftPreferences.lovedItems.Contains(itemName);
        bool isLiked = giftPreferences != null && giftPreferences.likedItems.Contains(itemName);
        bool isHated = giftPreferences != null && giftPreferences.hatedItems.Contains(itemName);
        if (!isLoved && !isLiked && !isHated && universalGiftPreferences != null)
        {
            if (universalGiftPreferences.universalLovedItems.Contains(itemName)) isLoved = true;
            else if (universalGiftPreferences.universalLikedItems.Contains(itemName)) isLiked = true;
            else if (universalGiftPreferences.universalHatedItems.Contains(itemName)) isHated = true;
        }
        if (isLoved)
        {
            reaction = "love";
            pointGain = 80;
        }
        else if (isLiked)
        {
            reaction = "like";
            pointGain = 45;
        }
        else if (isHated)
        {
            reaction = "dislike";
            pointGain = -25;
        }

        friendshipPoints = Mathf.Max(friendshipPoints + pointGain, 0);

        switch (reaction)
        {
            case "love":
                PlayDialogue(dialogueSet.loveDialogues);
                break;
            case "like":
                PlayDialogue(dialogueSet.likeDialogues);
                break;
            case "dislike":
                PlayDialogue(dialogueSet.dislikeDialogues);
                break;
            default:
                PlayDialogue(dialogueSet.normalDialogues);
                break;
        }
        item.amount -= 1;
        giftsThisWeek++;
        StartCoroutine(ReenableMovementAfterDialogue());
    }

    private void PlayDialogue(NPCDialogueSet.DialogueGroup[] groupArray)
    {
        Debug.Log($"[PlayDialogue] Called for NPC: {npcName}");

        if (groupArray == null)
        {
            Debug.LogWarning("[PlayDialogue] groupArray is null!");
            return;
        }

        if (groupArray.Length == 0)
        {
            Debug.LogWarning("[PlayDialogue] groupArray is empty!");
            return;
        }

        int randomIndex = Random.Range(0, groupArray.Length);
        var group = groupArray[randomIndex];

        Debug.Log($"[PlayDialogue] Selected group index: {randomIndex}");

        if (group.lines != null && group.lines.Length > 0)
        {
            for (int i = 0; i < group.lines.Length; i++)
            {
                Debug.Log($"[PlayDialogue] Line {i}: {group.lines[i]}");
            }
        }
        else
        {
            Debug.LogWarning("[PlayDialogue] group.lines is null or empty!");
        }

        DialogueManager.Instance.StartDialogue(dialogueSet, group);
    }
    public void InitFromState(NPCState state)
    {
        this.npcName = state.npcName;
        this.scheduleEntries = state.schedule;
        this.friendshipPoints = state.friendshipPoints;
        this.transform.position = state.currentPosition;
        // Nếu có animation/visual cần cập nhật thì gọi ở đây
        Debug.Log($"[NPC] Init {npcName} tại {state.currentPosition} ở scene {state.currentScene}");
    }
    //Moving
    public List<Vector2> currentPath = new(); 
    private int currentPathIndex = 0;
    private Vector2 lastInputVector = Vector2.right; 
    public void MoveAlongPathAStar()
    {
        if (currentPath == null || currentPath.Count == 0 || currentPathIndex >= currentPath.Count)
        {
            inputVector = Vector2.zero;
            UpdateIdleFacing();
            return;
        }
        Vector2 currentTarget = currentPath[currentPathIndex];
        Vector2 direction = (currentTarget - (Vector2)transform.position);
        float distance = direction.magnitude;
        if (distance < 0.1f)
        {
            currentPathIndex++;
            inputVector = Vector2.zero;
            UpdateIdleFacing();
            return;
        }
        inputVector = direction.normalized;
        lastInputVector = inputVector;
    }
    private void UpdateIdleFacing()
    {
        animator.SetFloat("X", lastInputVector.x);
        animator.SetFloat("Y", lastInputVector.y);
        animator.SetBool("IsWalking", false);
    }

    // Daily Schedule
    // Scene Transition
}
