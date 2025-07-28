using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UniversalGiftPreferences", menuName = "NPC/Universal Gift Preferences")]
public class UniversalGiftPreferences : ScriptableObject
{
    public List<string> universalLovedItems;
    public List<string> universalLikedItems;
    public List<string> universalHatedItems;
}
