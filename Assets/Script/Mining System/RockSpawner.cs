using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockSpawner : MonoBehaviour
{
    public GameObject[] rockPrefabs;
    [Range(0f, 1f)] public float rockDensity = 0.33f;
    private GridScanner gridScanner;
    private void Awake()
    {
        gridScanner = FindFirstObjectByType<GridScanner>();
        if (gridScanner == null)
        {
            Debug.LogWarning("[RockSpawner] GridScanner not found!");
        }
    }
    public void SpawnRocks()
    {
        if (gridScanner == null || gridScanner.walkableTiles == null)
        {
            Debug.LogWarning("[RockSpawner] GridScanner is null or walkableTiles not initialized!");
            return;
        }
        List<Vector2Int> walkableList = new List<Vector2Int>();
        foreach (var kvp in gridScanner.walkableTiles)
        {
            if (kvp.Value) walkableList.Add(kvp.Key);
        }
        if (walkableList.Count == 0)
        {
            Debug.LogWarning("[RockSpawner] No walkable tiles available!");
            return;
        }
        int maxRocks = Mathf.RoundToInt(walkableList.Count * rockDensity);
        Shuffle(walkableList);
        int spawned = 0;
        for (int i = 0; i < walkableList.Count && spawned < maxRocks; i++)
        {
            Vector2Int tilePos = walkableList[i];
            Vector3 worldPos = gridScanner.groundTilemap.GetCellCenterWorld(new Vector3Int(tilePos.x, tilePos.y, 0));
            GameObject rockPrefab = rockPrefabs[Random.Range(0, rockPrefabs.Length)];
            GameObject rock = Instantiate(rockPrefab, worldPos, Quaternion.identity);
            Debug.Log($"[ROCK SPAWNED] {rock.name} at Grid ({tilePos.x}, {tilePos.y}) -> WorldPos {worldPos}");
            gridScanner.walkableTiles[tilePos] = false;
            spawned++;
        }
        Debug.Log($"[RockSpawner] Spawned {spawned}/{maxRocks} rocks.");
        gridScanner.RefreshGrid();
    }
    private void Shuffle(List<Vector2Int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            Vector2Int temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}
