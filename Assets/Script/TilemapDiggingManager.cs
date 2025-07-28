using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapDiggingManager : MonoBehaviour
{
    public static TilemapDiggingManager Instance { get; private set; }
    [SerializeField] private Tilemap groundTilemap;
    [SerializeField] private Tilemap soilTilemap;
    [SerializeField] private Tilemap wateredTilemap;
    [SerializeField] private RuleTile soilRuleTile;
    [SerializeField] private RuleTile wateredRuleTile; 
    [SerializeField] private float digRange = 1.5f;
    public event Action<Vector2> OnTileDug;
    public event Action<Vector2> OnTileWatered;
    public event System.Action<Vector3Int> OnTileWateredRaw;
    private TileSaveData currentSaveData = new();
    public Tilemap GroundTilemap => groundTilemap;
    public Tilemap SoilTilemap => soilTilemap;
    public Tilemap WateredTilemap => wateredTilemap;
    private bool isSceneActive = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (groundTilemap == null || soilTilemap == null || wateredTilemap == null)
            AssignTilemaps();
    }
    private void OnNewDayStarted()
    {
        if (isSceneActive)
        {
            SaveToFile();
            ResetWateredTiles();
        }
        else
        {
            SaveToMemory();
        }
    }
    private void AssignTilemaps()
    {
        GameObject farmingMap = GameObject.Find("Farming Map");
        GameObject digibleMap = GameObject.Find("Diggible TileMap");
        if (farmingMap != null)
        {
            soilTilemap = farmingMap.transform.Find("SoilTilemap")?.GetComponent<Tilemap>();
            wateredTilemap = farmingMap.transform.Find("WateredTilemap")?.GetComponent<Tilemap>();
        }
        if(digibleMap != null)
        {
            groundTilemap = farmingMap.transform.Find("Diggible TileMap")?.GetComponent<Tilemap>();

        }
        if (groundTilemap == null || soilTilemap == null || wateredTilemap == null)
        {
            Debug.LogError("❌ One or more tilemaps not found. Check your Farming Map hierarchy.");
        }
    }
    private void Start()
    {
        LoadTilemapFromFile();
    }
    void LoadTilemapFromFile()
    {
        currentSaveData = TilemapSaveManager.Load();

        foreach (var pos in currentSaveData.soilTiles)
            soilTilemap.SetTile(pos, soilRuleTile);

        foreach (var pos in currentSaveData.wateredTiles)
            wateredTilemap.SetTile(pos, wateredRuleTile);
    }
    public void TryDigAtPosition(Vector2 playerPosition)
    {
        Vector3Int tilePosition = groundTilemap.WorldToCell(playerPosition);
        if (IsInRange(playerPosition, tilePosition) && groundTilemap.HasTile(tilePosition) && !soilTilemap.HasTile(tilePosition))   
        {
            TryDigTile(tilePosition);
        }
    }
    public void TryWaterAtPosition(Vector2 playerPosition)
    {
        Vector3Int tilePosition = soilTilemap.WorldToCell(playerPosition);
        if (IsInRange(playerPosition, tilePosition))
        {
            TryWaterTile(tilePosition);
        }
    }
    bool IsInRange(Vector2 playerPosition, Vector3Int tilePosition)
    {
        Vector3 tileWorldPos = groundTilemap.CellToWorld(tilePosition);
        return Vector2.Distance(playerPosition, tileWorldPos) <= digRange;
    }
    void TryDigTile(Vector3Int position)
    {
        if (soilTilemap.HasTile(position)) return;

        soilTilemap.SetTile(position, soilRuleTile);
        currentSaveData.soilTiles.Add(position);

        if (WeatherManager.Instance.IsRaining)
            TryWaterTile(position);

        TilemapSaveManager.Save(currentSaveData);
        OnTileDug?.Invoke((Vector2)groundTilemap.CellToWorld(position));
    }
    void TryWaterTile(Vector3Int position)
    {
        if (wateredTilemap.HasTile(position)) return;
        wateredTilemap.SetTile(position, wateredRuleTile);
        currentSaveData.wateredTiles.Add(position);
        TilemapSaveManager.Save(currentSaveData);
        OnTileWatered?.Invoke((Vector2)soilTilemap.CellToWorld(position));
        OnTileWateredRaw?.Invoke(position); 
    }
    public void AutoWaterOnRain()
    {
        BoundsInt bounds = soilTilemap.cellBounds;
        foreach (var position in bounds.allPositionsWithin)
        {
            if (soilTilemap.HasTile(position) && !wateredTilemap.HasTile(position))
            {
                wateredTilemap.SetTile(position, wateredRuleTile);
                OnTileWatered?.Invoke((Vector2)soilTilemap.CellToWorld(position));
            }
        }
    }
    private void OnEnable()
    {
        isSceneActive = true;
        WeatherManager.Instance.OnRainStarted += AutoWaterOnRain;
        GameTimeManager.Instance.OnNewDayStarted += OnNewDayStarted;
    }
    private void OnDisable()
    {
        isSceneActive = false;
        WeatherManager.Instance.OnRainStarted -= AutoWaterOnRain;
        GameTimeManager.Instance.OnNewDayStarted -= OnNewDayStarted;
    }
    private void ResetWateredTiles()
    {
        wateredTilemap.ClearAllTiles();
    }
    public void SaveToFile()
    {
        TilemapSaveManager.Save(currentSaveData);
        Debug.Log("🌱 Tilemap saved at end of day.");
    }
    private TileSaveData memorySaveData;
    public void SaveToMemory()
    {
        memorySaveData = new TileSaveData
        {
            soilTiles = new List<Vector3Int>(currentSaveData.soilTiles),
            wateredTiles = new List<Vector3Int>()
        };
        Debug.Log("💾 Tilemap cached in memory for future save.");
    }

}
