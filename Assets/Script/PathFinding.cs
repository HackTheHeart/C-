using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public static Pathfinding Instance;
    public GridScanner gridScanner;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public List<Vector2> FindPath(Vector2 startWorldPos, Vector2 targetWorldPos)
    {
        Vector2Int startGridPos = WorldToGrid(startWorldPos);
        Vector2Int targetGridPos = WorldToGrid(targetWorldPos);
        //Debug.Log($"FindPath() - Start: {startGridPos}, Target: {targetGridPos}");
        List<Vector2Int> path = AStar(startGridPos, targetGridPos);
        List<string> worldPosStrings = new List<string>();
        foreach (var gridPos in path)
        {
            Vector3Int tilePos = new Vector3Int(gridPos.x, gridPos.y, 0);
            Vector3 worldPos = gridScanner.groundTilemap.CellToWorld(tilePos);
            Vector3 tileCenter = worldPos + new Vector3(gridScanner.cellSize / 2f, gridScanner.cellSize / 2f, 0);
            worldPosStrings.Add($"({tileCenter.x:F2}, {tileCenter.y:F2})");
        }
        Debug.Log($"AStar Path World Positions: {string.Join(" -> ", worldPosStrings)}");
        if (path.Count == 0)
        {
            //Debug.LogWarning("FindPath() - Không tìm thấy đường đi!");
        }
        else
        {
            //Debug.Log($"FindPath() - Tìm thấy đường đi ({path.Count} bước).");
        }
        GridScanner scanner = FindFirstObjectByType<GridScanner>();
        if (scanner != null)
        {
            scanner.ClearDebugPaths();
            scanner.AddDebugPath(path);
        }
        return ConvertGridPathToWorldPath(path);
    }
    private List<Vector2Int> AStar(Vector2Int start, Vector2Int target)
    {
        List<Vector2Int> openSet = new List<Vector2Int> { start };
        HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
        Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float> { [start] = 0 };
        Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float> { [start] = Heuristic(start, target) };
        while (openSet.Count > 0)
        {
            openSet.Sort((a, b) => fScore[a].CompareTo(fScore[b]));
            Vector2Int current = openSet[0];
            if (current == target)
                return ReconstructPath(cameFrom, current);
            openSet.RemoveAt(0);
            closedSet.Add(current);
            foreach (Vector2Int neighbor in GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor) || !gridScanner.CanMoveTo(neighbor.x, neighbor.y))
                    continue;
                float tentativeGScore = gScore[current] + Vector2Int.Distance(current, neighbor);
                if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                {
                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentativeGScore;
                    fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, target);
                    if (!openSet.Contains(neighbor))
                        openSet.Add(neighbor);
                }
            }
        }
        return new List<Vector2Int>();
    }
    private List<Vector2Int> GetNeighbors(Vector2Int node)
    {
        List<Vector2Int> neighbors = new List<Vector2Int>
        {
            new Vector2Int(node.x + 1, node.y),
            new Vector2Int(node.x - 1, node.y),
            new Vector2Int(node.x, node.y + 1),
            new Vector2Int(node.x, node.y - 1)
        };
        return neighbors;
    }
    private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
    {
        List<Vector2Int> path = new List<Vector2Int> { current };
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
    private Vector2Int WorldToGrid(Vector2 worldPos)
    {
        return new Vector2Int(Mathf.FloorToInt(worldPos.x), Mathf.FloorToInt(worldPos.y));
    }
    private List<Vector2> ConvertGridPathToWorldPath(List<Vector2Int> gridPath)
    {
        List<Vector2> worldPath = new List<Vector2>();
        foreach (Vector2Int gridPos in gridPath)
        {
            Vector3Int tilePos = new Vector3Int(gridPos.x, gridPos.y, 0);
            // Lấy vị trí góc dưới trái tile trong world space
            Vector3 worldPos = gridScanner.groundTilemap.CellToWorld(tilePos);
            // Thêm offset để lấy vị trí trung tâm tile (nếu cần)
            Vector3 tileCenter = worldPos + new Vector3(gridScanner.cellSize / 2f, gridScanner.cellSize / 2f, 0);
            worldPath.Add(tileCenter);
        }
        return worldPath;
    }

}
