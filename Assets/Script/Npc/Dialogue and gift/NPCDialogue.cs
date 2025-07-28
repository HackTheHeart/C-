using UnityEngine;

[CreateAssetMenu(fileName = "NewNPCDialogueSet", menuName = "NPC Dialogue Set")]
public class NPCDialogueSet : ScriptableObject
{
    public string npcName;
    public Sprite[] npcPortraits;

    public DialogueGroup[] normalDialogues;
    public DialogueGroup[] likeDialogues;
    public DialogueGroup[] dislikeDialogues;
    public DialogueGroup[] loveDialogues;
    [System.Serializable]
    public class DialogueGroup
    {
        [TextArea(2, 5)]
        public string[] lines;
        public bool[] autoProgressLines;
        public NPCExpression[] expressions;
        public float typingSpeed = 0.05f;
        public float autoProgressDelay = 1.5f;
    }
}
public enum NPCExpression
{
    Neutral,
    Happy,
    Sad,
    Angry,
    Blush,
    Sparkle
}
