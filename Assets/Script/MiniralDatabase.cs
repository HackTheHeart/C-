using System.Collections.Generic;
using UnityEngine;

public class MineralDatabase : MonoBehaviour
{
    public static MineralDatabase Instance;
    public List<MineralOreData> allOres;
    private void Awake()
    {
        Instance = this;
    }
    public List<MineralOreData> GetAvailableOres(int playerLevel)
    {
        return allOres.FindAll(ore => playerLevel >= ore.minLevel && playerLevel <= ore.maxLevel);
    }
}
