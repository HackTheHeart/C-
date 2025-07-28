using UnityEngine;

[CreateAssetMenu(fileName = "MineralData", menuName = "Mining/Mineral Ore")]
public class MineralOreData : ScriptableObject
{
    public string oreName;
    public GameObject[] orePrefabs;
    public int minLevel;
    public int maxLevel;
}
