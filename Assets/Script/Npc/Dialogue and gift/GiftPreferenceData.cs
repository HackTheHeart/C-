using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewGiftPreferences", menuName = "NPC/Gift Preferences")]
public class GiftPreferenceData : ScriptableObject
{
    public string npcName;
    public List<string> lovedItems;
    public List<string> likedItems;
    public List<string> hatedItems;
}
