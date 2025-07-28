using System.Collections.Generic;
using UnityEngine;

public class WarpManager : MonoBehaviour
{
    public static WarpManager Instance;
    [SerializeField]
    private List<WarpTile> warpTilesList = new();
    private Dictionary<string, Dictionary<Vector2Int, WarpTile>> warpTileMap = new();
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            CacheWarpTiles();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void CacheWarpTiles()
    {
        warpTileMap.Clear();
        foreach (var warp in warpTilesList)
        {
            if (!warpTileMap.ContainsKey(warp.fromScene))
                warpTileMap[warp.fromScene] = new Dictionary<Vector2Int, WarpTile>();

            warpTileMap[warp.fromScene][warp.tilePosition] = warp;
        }

        Debug.Log($"[WarpManager] Cached {warpTilesList.Count} warp tiles.");
    }
    public WarpTile GetWarpAt(string sceneName, Vector2Int tile)
    {
        if (warpTileMap.TryGetValue(sceneName, out var sceneWarpDict) &&
            sceneWarpDict.TryGetValue(tile, out var warpTile))
        {
            return warpTile;
        }
        return null;
    }
    public bool IsWarpTile(string sceneName, Vector2Int tile)
    {
        return GetWarpAt(sceneName, tile) != null;
    }
    public WarpTile GetWarpToScene(string fromScene, string toScene)
    {
        if (!warpTileMap.ContainsKey(fromScene)) return null;

        foreach (var warp in warpTileMap[fromScene].Values)
        {
            if (warp.toScene == toScene)
                return warp;
        }

        return null;
    }
    public Vector2 TileToWorldPosition(Vector2Int tilePos)
    {
        return new Vector2(tilePos.x + 0.5f, tilePos.y + 0.5f);
    }
}
