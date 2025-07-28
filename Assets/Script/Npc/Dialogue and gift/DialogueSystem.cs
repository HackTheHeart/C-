//using System.Collections;
//using TMPro;
//using UnityEngine;
//using UnityEngine.UI;

//public class DialogueManager : MonoBehaviour
//{
//    public static DialogueManager Instance;
//    public TMP_Text dialogueText;
//    public Image npcPortrait;
//    public GameObject dialogueBox;
//    public TMP_Text NPCname;
//    private string[] currentLines;
//    private bool[] autoProgressLines;
//    private float typingSpeed;
//    private float autoProgressDelay;
//    private int currentLineIndex = 0;
//    private bool isTyping = false;
//    private Coroutine typingCoroutine;
//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }
//    public void ShowLine()
//    {
//        if (currentLineIndex >= currentLines.Length) return;
//        string line = currentLines[currentLineIndex];
//        if (typingCoroutine != null)
//            StopCoroutine(typingCoroutine);
//        typingCoroutine = StartCoroutine(TypeLine(line));
//    }

//    public void StartDialogue(NPCDialogueSet.DialogueGroup group, Sprite portrait)
//    {
//        currentLines = group.lines;
//        autoProgressLines = group.autoProgressLines;
//        typingSpeed = group.typingSpeed;
//        autoProgressDelay = group.autoProgressDelay;
//        currentLineIndex = 0;
//        npcPortrait.sprite = portrait;
//        dialogueBox.SetActive(true);
//        ShowLine();
//    }
//    private IEnumerator TypeLine(string line)
//    {
//        isTyping = true;
//        dialogueText.text = "";
//        foreach (char c in line)
//        {
//            dialogueText.text += c;
//            yield return new WaitForSeconds(typingSpeed);
//        }
//        isTyping = false;

//        if (autoProgressLines.Length > currentLineIndex && autoProgressLines[currentLineIndex])
//        {
//            yield return new WaitForSeconds(autoProgressDelay);
//            NextLine();
//        }
//    }
//    public void NextLine()
//    {
//        if (isTyping)
//        {
//            StopCoroutine(typingCoroutine);
//            dialogueText.text = currentLines[currentLineIndex];
//            isTyping = false;
//            return;
//        }
//        currentLineIndex++;
//        if (currentLineIndex < currentLines.Length)
//        {
//            ShowLine();
//        }
//        else
//        {
//            dialogueBox.SetActive(false);
//        }
//    }
//    private void Update()
//    {
//        if (dialogueBox.activeSelf && Input.GetKeyDown(KeyCode.Space))
//        {
//            NextLine();
//        }
//    }
//}
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TMP_Text dialogueText;
    public Image npcPortrait;
    public GameObject dialogueBox;
    public TMP_Text NPCname;

    private string[] currentLines;
    private bool[] autoProgressLines;
    private NPCExpression[] expressions;
    private Sprite[] portraitSprites;

    private float typingSpeed;
    private float autoProgressDelay;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void StartDialogue(NPCDialogueSet dialogueSet, NPCDialogueSet.DialogueGroup group)
    {
        // Gán dữ liệu
        currentLines = group.lines;
        autoProgressLines = group.autoProgressLines;
        expressions = group.expressions;
        portraitSprites = dialogueSet.npcPortraits;
        typingSpeed = group.typingSpeed;
        autoProgressDelay = group.autoProgressDelay;
        currentLineIndex = 0;

        // Gán tên NPC nếu có
        if (NPCname != null) NPCname.text = dialogueSet.npcName;
        dialogueBox.SetActive(true);
        ShowLine();
    }
    public void ShowLine()
    {
        if (currentLineIndex >= currentLines.Length) return;
        string line = currentLines[currentLineIndex];

        // Gán biểu cảm dựa theo expressions enum
        if (expressions != null && portraitSprites != null &&
            currentLineIndex < expressions.Length &&
            (int)expressions[currentLineIndex] < portraitSprites.Length)
        {
            npcPortrait.sprite = portraitSprites[(int)expressions[currentLineIndex]];
        }

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(line));
    }
    private IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
        //if (autoProgressLines.Length > currentLineIndex && autoProgressLines[currentLineIndex])
        //{
        //    yield return new WaitForSeconds(autoProgressDelay);
        //    NextLine();
        //}
    }
    //public void NextLine()
    //{
    //    if (isTyping)
    //    {
    //        StopCoroutine(typingCoroutine);
    //        dialogueText.text = currentLines[currentLineIndex];
    //        isTyping = false;
    //        return;
    //    }

    //    currentLineIndex++;
    //    if (currentLineIndex < currentLines.Length)
    //    {
    //        ShowLine();
    //    }
    //    else
    //    {
    //        dialogueBox.SetActive(false);
    //    }
    //}
    public void NextLine()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = currentLines[currentLineIndex];
            isTyping = false;
            return;
        }
        currentLineIndex++;
        if (currentLineIndex < currentLines.Length)
        {
            ShowLine();
        }
        else
        {
            dialogueBox.SetActive(false);
        }
    }
    private void Update()
    {
        if (dialogueBox == null)
        {
            Debug.LogWarning("DialogueBox is null! Check inspector setup.");
            return;
        }
        if (dialogueBox.activeSelf && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }
}
