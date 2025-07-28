using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;

public class GridScannerToJson : MonoBehaviour
{
    public Tilemap groundTilemap;
    public float cellSize = 1f;
    public LayerMask obstacleLayers; // <-- Layer để kiểm tra collider (bạn chọn trong Inspector)

    [System.Serializable]
    public class WalkableSceneData
    {
        public string sceneName;
        public List<Vector2Int> walkableTiles = new();
    }

    [System.Serializable]
    public class WalkableDataCollection
    {
        public List<WalkableSceneData> scenes = new();
    }

    private void Start()
    {
        Debug.Log("[GridScannerToJson] Scanning started...");
        WalkableSceneData sceneData = new WalkableSceneData();
        sceneData.sceneName = SceneManager.GetActiveScene().name;

        BoundsInt bounds = groundTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (!groundTilemap.HasTile(tilePos)) continue;

                Vector3 worldPos = groundTilemap.CellToWorld(tilePos) + groundTilemap.cellSize / 2f;
                Vector2 boxSize = groundTilemap.cellSize * 0.9f;

                Collider2D hit = Physics2D.OverlapBox(worldPos, boxSize, 0f, obstacleLayers);
                if (hit != null) continue;

                Vector2Int gridPos = new Vector2Int(x, y);
                sceneData.walkableTiles.Add(gridPos);
            }
        }

        string path = Path.Combine(Application.persistentDataPath, "WalkableTileData.json");
        WalkableDataCollection allData;

        if (File.Exists(path))
        {
            string existingJson = File.ReadAllText(path);
            allData = JsonUtility.FromJson<WalkableDataCollection>(existingJson);
        }
        else
        {
            allData = new WalkableDataCollection();
        }

        allData.scenes.RemoveAll(s => s.sceneName == sceneData.sceneName);
        allData.scenes.Add(sceneData);
        string newJson = JsonUtility.ToJson(allData, true);
        File.WriteAllText(path, newJson);
        Debug.Log($"[GridScannerToJson] Walkable tiles saved for scene '{sceneData.sceneName}' at:\n{path}");
    }

    private void OnDrawGizmos()
    {
        if (groundTilemap == null) return;

        BoundsInt bounds = groundTilemap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int tilePos = new Vector3Int(x, y, 0);
                if (!groundTilemap.HasTile(tilePos)) continue;

                Vector3 worldPos = groundTilemap.CellToWorld(tilePos) + groundTilemap.cellSize / 2f;
                Vector2 boxSize = groundTilemap.cellSize * 0.9f;

                Collider2D hit = Physics2D.OverlapBox(worldPos, boxSize, 0f, obstacleLayers);
                Gizmos.color = (hit != null) ? Color.red : Color.green;

                Gizmos.DrawWireCube(worldPos, new Vector3(boxSize.x, boxSize.y, 0.1f));
            }
        }
    }
}
