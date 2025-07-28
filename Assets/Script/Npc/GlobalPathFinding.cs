using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class GlobalPathfinding : MonoBehaviour
{
    public static GlobalPathfinding Instance;

    private Dictionary<string, HashSet<Vector2Int>> walkableTilesByScene;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadWalkableTilesFromJson();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void LoadWalkableTilesFromJson()
    {
        walkableTilesByScene = new Dictionary<string, HashSet<Vector2Int>>();
        string path = Path.Combine(Application.persistentDataPath, "WalkableTileData.json");
        if (!File.Exists(path))
        {
            Debug.LogError("[GlobalPathfinding] Không tìm thấy file WalkableTileData.json.");
            return;
        }
        string json = File.ReadAllText(path);
        WalkableDataCollection data = JsonUtility.FromJson<WalkableDataCollection>(json);
        foreach (var sceneData in data.scenes)
        {
            HashSet<Vector2Int> tileSet = new(sceneData.walkableTiles);
            walkableTilesByScene[sceneData.sceneName] = tileSet;
        }
        Debug.Log($"[GlobalPathfinding] Đã load dữ liệu walkable tiles cho {walkableTilesByScene.Count} scene.");
    }
    public List<Vector2> FindPath(string sceneName, Vector2 startWorld, Vector2 targetWorld)
    {
        Vector2Int start = WorldToGrid(startWorld);
        Vector2Int target = WorldToGrid(targetWorld);
        if (!walkableTilesByScene.ContainsKey(sceneName))
        {
            Debug.LogWarning($"[GlobalPathfinding] Scene '{sceneName}' chưa có dữ liệu walkable.");
            return new List<Vector2>();
        }
        List<Vector2Int> gridPath = AStar(sceneName, start, target);
        return GridToWorldPath(gridPath);
    }
    private List<Vector2Int> AStar(string sceneName, Vector2Int start, Vector2Int goal)
    {
        var walkables = walkableTilesByScene[sceneName];
        var openSet = new List<Vector2Int> { start };
        var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        var gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
        var fScore = new Dictionary<Vector2Int, float> { [start] = Heuristic(start, goal) };
        var closedSet = new HashSet<Vector2Int>();
        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            Vector2Int current = openSet[0];
            if (current == goal)
                return ReconstructPath(cameFrom, current);
            openSet.RemoveAt(0);
            closedSet.Add(current);
            foreach (var neighbor in GetNeighbors(current))
            {
                if (!walkables.Contains(neighbor) || closedSet.Contains(neighbor))
                    continue;

                float tentativeG = gScore[current] + 1;

                if (!gScore.ContainsKey(neighbor) || tentativeG < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeG;
                    fScore[neighbor] = tentativeG + Heuristic(neighbor, goal);

                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }

        return new List<Vector2Int>(); // không tìm được đường
    }

    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new() { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Add(current);
        }
        path.Reverse();
        return path;
    }

    private float Heuristic(Vector2Int a, Vector2Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        return new List<Vector2Int>
        {
            new(node.x + 1, node.y),
            new(node.x - 1, node.y),
            new(node.x, node.y + 1),
            new(node.x, node.y - 1)
        };
    }

    private Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
    }
    private List<Vector2> GridToWorldPath(List<Vector2Int> gridPath)
    {
        List<Vector2> worldPath = new();
        foreach (var pos in gridPath)
        {
            worldPath.Add(new Vector2(pos.x + 0.5f, pos.y + 0.5f));
        }
        return worldPath;
    }
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
    public List<Vector2> FindMultiScenePath(string fromScene, Vector2 fromWorld, string toScene, Vector2 toWorld)
    {
        WarpTile warp = WarpManager.Instance.GetWarpToScene(fromScene, toScene);
        if (warp == null) return new List<Vector2>();
        List<Vector2> pathToWarp = FindPath(fromScene, fromWorld, new Vector2(warp.tilePosition.x + 0.5f, warp.tilePosition.y + 0.5f));
        List<Vector2> pathInTargetScene = FindPath(toScene, warp.targetPosition, toWorld);
        List<Vector2> fullPath = new();
        fullPath.AddRange(pathToWarp);
        fullPath.AddRange(pathInTargetScene);
        return fullPath;
    }
}
