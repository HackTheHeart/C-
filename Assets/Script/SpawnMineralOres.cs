using System.Collections.Generic;
using UnityEngine;

public class SpawnMineralOres : MonoBehaviour
{
    public static SpawnMineralOres Instance;
    public int oreCountToSpawn = 10;
    private GridScanner gridScanner;

    private void Awake()
    {
        Instance = this;
        gridScanner = FindFirstObjectByType<GridScanner>();

        if (gridScanner == null)
        {
            Debug.LogError("[SpawnMineralOres] GridScanner is not found!");
        }
    }
    private void Start()
    {
        if (MineralDatabase.Instance == null)
        {
            Debug.LogWarning("[SpawnMineralOres] MineralDatabase.Instance is null!");
            return;
        }
    }
    public void SpawnOre(int playerLevel)
    {
        if (MineralDatabase.Instance == null)
        {
            Debug.LogWarning("[SpawnMineralOres] MineralDatabase.Instance is null!");
        }

        if (gridScanner == null)
        {
            Debug.LogWarning("[SpawnMineralOres] gridScanner is null!");
        }

        if (MineralDatabase.Instance == null || gridScanner == null)
        {
            return; 
        }

        List<MineralOreData> availableOres = MineralDatabase.Instance.GetAvailableOres(playerLevel);
        if (availableOres.Count == 0)
        {
            Debug.LogWarning("[SpawnMineralOres] No ores available for current level.");
            return;
        }

        Dictionary<Vector2Int, bool> walkableTiles = gridScanner.walkableTiles;
        if (walkableTiles == null || walkableTiles.Count == 0)
        {
            Debug.LogWarning("[SpawnMineralOres] No walkable tiles found.");
            return;
        }

        List<Vector2Int> freeTiles = new List<Vector2Int>();
        foreach (var tile in walkableTiles)
        {
            if (tile.Value)
                freeTiles.Add(tile.Key);
        }

        Debug.Log($"[SpawnMineralOres] Free walkable tiles found: {freeTiles.Count}");

        for (int i = 0; i < oreCountToSpawn; i++)
        {
            if (freeTiles.Count == 0) break;
            MineralOreData oreData = availableOres[Random.Range(0, availableOres.Count)];
            GameObject orePrefab = oreData.orePrefabs[Random.Range(0, oreData.orePrefabs.Length)];
            int index = Random.Range(0, freeTiles.Count);
            Vector2Int gridPos = freeTiles[index];
            freeTiles.RemoveAt(index);
            Vector3 worldPos = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(gridPos.x, gridPos.y, 0));
            GameObject spawnedOre = Instantiate(orePrefab, worldPos, Quaternion.identity);

            Debug.Log($"[ORE SPAWNED] {spawnedOre.name} at Grid ({gridPos.x}, {gridPos.y}) -> WorldPos {worldPos}");
        }

        gridScanner.RefreshGrid();
    }
}
