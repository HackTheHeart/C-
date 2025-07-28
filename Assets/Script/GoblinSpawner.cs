using System.Collections.Generic;
using UnityEngine;

public class GoblinSpawner : MonoBehaviour
{
    public GameObject spearPrefab;
    public GameObject archerPrefab;
    public GameObject bombPrefab;
    public int goblinsPerWave = 1;
    public int spawnRadius = 2;
    private GridScanner gridScanner;
    private void Awake()
    {
        gridScanner = FindFirstObjectByType<GridScanner>();
    }
    public void SpawnGoblinWave(int level)
    {
        if (gridScanner == null || gridScanner.walkableTiles == null)
        {
            Debug.LogWarning("[GoblinSpawner] GridScanner or walkableTiles is null!");
            return;
        }
        List<Vector2Int> walkableList = new List<Vector2Int>();
        foreach (var tile in gridScanner.walkableTiles)
        {
            if (tile.Value) walkableList.Add(tile.Key);
        }
        if (walkableList.Count == 0)
        {
            Debug.LogWarning("[GoblinSpawner] No walkable tiles available!");
            return;
        }
        Vector2Int center = walkableList[Random.Range(0, walkableList.Count)];
        Debug.Log($"[GoblinSpawner] Selected center tile for group spawn: {center}");
        int spawned = 0;
        int attempts = 0;
        while (spawned < goblinsPerWave && attempts < goblinsPerWave * 5)
        {
            attempts++;
            Vector2Int offset = new Vector2Int(Random.Range(-spawnRadius, spawnRadius + 1), Random.Range(-spawnRadius, spawnRadius + 1));
            Vector2Int spawnPos = center + offset;
            if (!gridScanner.walkableTiles.ContainsKey(spawnPos) || !gridScanner.walkableTiles[spawnPos])
            {
                Debug.Log($"[GoblinSpawner] Tile {spawnPos} not walkable or doesn't exist.");
                continue;
            }
            Vector3 worldPos = gridScanner.groundTilemap.CellToWorld((Vector3Int)spawnPos) + new Vector3(0.5f, 0.5f, 0f);
            GameObject prefab = GetGoblinByLevel(level);
            GameObject goblin = Instantiate(prefab, worldPos, Quaternion.identity);
            Debug.Log($"[GOBLIN SPAWNED] {goblin.name} at Grid ({spawnPos.x}, {spawnPos.y}) -> WorldPos {worldPos}");
            gridScanner.walkableTiles[spawnPos] = false;
            spawned++;
        }
        Debug.Log($"[GoblinSpawner] Spawned {spawned}/{goblinsPerWave} goblins after {attempts} attempts.");
        gridScanner.RefreshGrid();
    }
    private GameObject GetGoblinByLevel(int level)
    {
        int r = Random.Range(0, 100);
        GameObject result;
        if (r < 40)
            result = spearPrefab;
        else if (r < 70)
            result = archerPrefab;
        else
            result = bombPrefab;
        Debug.Log($"[GoblinSpawner] Chose goblin type: {result.name} (random {r})");
        return result;
    }
}
