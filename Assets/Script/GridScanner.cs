using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GridScanner : MonoBehaviour
{
    public Tilemap groundTilemap;     
    public Tilemap collisionTilemap;  
    public float cellSize = 1f;       
    public Dictionary<Vector2Int, bool> walkableTiles;
    int currentLevel;
    private void Start()
    {
        currentLevel = SceneTransitionManager.Instance != null ? SceneTransitionManager.Instance.CurrentCaveFloor : 1;
        Debug.Log("[currentLevel] Start method called.");
        Debug.Log("[GridScanner] Start method called.");
        Debug.Log("[GridScanner] Scanning walkable area...");
        ScanWalkableArea();
        Debug.Log("[GridScanner] Spawning ores...");
        if (SpawnMineralOres.Instance != null)
        {
            SpawnMineralOres.Instance.SpawnOre(currentLevel);
        }
        else
        {
            Debug.LogWarning("[GridScanner] SpawnMineralOres.Instance is null.");
        }
        Debug.Log("[GridScanner] Starting DelayedGridRefresh coroutine.");
        StartCoroutine(DelayedGridRefresh());
        RockSpawner rockSpawner = FindFirstObjectByType<RockSpawner>();
        if (rockSpawner != null)
        {
            rockSpawner.SpawnRocks();
        }
        else
        {
            Debug.LogWarning("[GridScanner] RockSpawner not found!");
        }
        StartCoroutine(DelayedGridRefresh());
        Debug.Log("[GridScanner] Spawning goblin wave...");
        GoblinSpawner goblinSpawner = FindFirstObjectByType<GoblinSpawner>();
        if (goblinSpawner != null)
        {
            goblinSpawner.SpawnGoblinWave(currentLevel);
        }
        else
        {
            Debug.LogWarning("[GridScanner] GoblinSpawner not found!");
        }
        Debug.Log("[GridScanner] Starting another DelayedGridRefresh coroutine.");
        StartCoroutine(DelayedGridRefresh());
    }
    IEnumerator DelayedGridRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        FindFirstObjectByType<GridScanner>().RefreshGrid();
    }
    public void RefreshGrid()
    {
        ScanWalkableArea();
    }
    void ScanWalkableArea()
    {
        BoundsInt bounds = groundTilemap.cellBounds;
        walkableTiles = new Dictionary<Vector2Int, bool>();
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                Vector2Int gridPos = new Vector2Int(x, y);
                walkableTiles[gridPos] = false;
                if (!groundTilemap.HasTile(tilePos))
                    continue;
                if (collisionTilemap.HasTile(tilePos))
                    continue;
                Vector3 worldPos = groundTilemap.CellToWorld(tilePos) + new Vector3(cellSize / 2, cellSize / 2, 0);
                int ignoreCameraBounds = ~(1 << LayerMask.NameToLayer("CameraBounds"));
                Collider2D hit = Physics2D.OverlapCircle(worldPos, cellSize / 3, ignoreCameraBounds);
                if (hit != null)
                {
                    if (hit.CompareTag("Player"))
                        walkableTiles[gridPos] = true;
                    if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        walkableTiles[gridPos] = true;
                    }
                    continue;
                }
                walkableTiles[gridPos] = true;
            }
        }
        Debug.Log("Scan completed! Walkable tiles count: " + walkableTiles.Count);
    }
    public bool CanMoveTo(int x, int y)
    {
        Vector2Int gridPos = new Vector2Int(x, y);
        if (walkableTiles.ContainsKey(gridPos))
        {
            return walkableTiles[gridPos];
        }
        return false;
    }
    //void OnDrawGizmos()
    //{
    //    if (walkableTiles == null) return;

    //    foreach (var tile in walkableTiles)
    //    {
    //        Vector3 worldPos = groundTilemap.CellToWorld(new Vector3Int(tile.Key.x, tile.Key.y, 0));
    //        Gizmos.color = tile.Value ? Color.green : Color.red;
    //        Gizmos.DrawCube(worldPos + new Vector3(cellSize / 2, cellSize / 2, 0), Vector3.one * 0.5f);
    //    }
    //}
    public List<List<Vector2Int>> debugPaths = new List<List<Vector2Int>>();
    public void ClearDebugPaths()
    {
        debugPaths.Clear();
    }
    public void AddDebugPath(List<Vector2Int> path)
    {
        if (path != null && path.Count > 1)
        {
            debugPaths.Add(path);
        }
    }
    void OnDrawGizmos()
    {
        if (walkableTiles == null) return;

        // Vẽ grid
        foreach (var tile in walkableTiles)
        {
            Vector3 worldPos = groundTilemap.CellToWorld(new Vector3Int(tile.Key.x, tile.Key.y, 0));
            Gizmos.color = tile.Value ? Color.green : Color.red;
            Gizmos.DrawCube(worldPos + new Vector3(cellSize / 2, cellSize / 2, 0), Vector3.one * 0.5f);
        }

        // Vẽ path debug
        Gizmos.color = Color.cyan;
        foreach (var path in debugPaths)
        {
            for (int i = 0; i < path.Count - 1; i++)
            {
                Vector3 from = groundTilemap.CellToWorld((Vector3Int)path[i]) + new Vector3(cellSize / 2, cellSize / 2, 0);
                Vector3 to = groundTilemap.CellToWorld((Vector3Int)path[i + 1]) + new Vector3(cellSize / 2, cellSize / 2, 0);
                Gizmos.DrawLine(from, to);
            }
        }
    }

}
