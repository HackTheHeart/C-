using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CropManager : MonoBehaviour
{
    public static CropManager Instance { get; private set; }
    private Dictionary<Vector3Int, Crop> cropsDictionary = new Dictionary<Vector3Int, Crop>();
    [SerializeField] private Tilemap soilTilemap;
    [SerializeField] private Transform cropParent;
    [SerializeField] private GameObject cropPrefab;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Texture2D harvestCursor;
    [SerializeField] private Texture2D defaultCursor;
    public Dictionary<Vector3Int, Crop> GetAllCrops() => cropsDictionary;
    public Crop GetCropAt(Vector3Int tilePos)
    {
        cropsDictionary.TryGetValue(tilePos, out var crop);
        return crop;
    }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        if (soilTilemap == null )
            AssignTilemaps();
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }
    private void AssignTilemaps()
    {
        GameObject farmingMap = GameObject.Find("Farming Map");
        if (farmingMap != null)
        {
            soilTilemap = farmingMap.transform.Find("SoilTilemap")?.GetComponent<Tilemap>();
        }
        if (soilTilemap == null )
        {
            Debug.LogError("❌ One or more tilemaps not found. Check your Farming Map hierarchy.");
        }
    }
    private void Start()
    {
        LoadCrops();
    }
    public bool PlantCrop(Vector2 position, CropData cropData)
    {
        Vector3Int tilePosition = soilTilemap.WorldToCell(position);
        if (!soilTilemap.HasTile(tilePosition)) return false;
        Collider2D[] crops = Physics2D.OverlapCircleAll(
            (Vector2)soilTilemap.CellToWorld(tilePosition) + new Vector2(0.5f, 0.5f),
            0.2f
        );
        foreach (Collider2D crop in crops)
        {
            if (crop.GetComponent<Crop>() != null)
            {
                return false;
            }
        }
        if (cropsDictionary.ContainsKey(tilePosition)) return false;
        Vector3 spawnPosition = soilTilemap.CellToWorld(tilePosition) + new Vector3(0.5f, 0.5f, 0);
        GameObject newCrop = Instantiate(cropPrefab, spawnPosition, Quaternion.identity, cropParent);
        Crop cropScript = newCrop.GetComponent<Crop>();
        cropScript.Initialize(cropData);
        cropsDictionary[tilePosition] = cropScript;
        SaveCrops();
        return true;
    }
    public void HarvestCrop(Vector2 position)
    {
        Vector3Int tilePosition = soilTilemap.WorldToCell(position);
        if (cropsDictionary.ContainsKey(tilePosition))
        {
            Crop crop = cropsDictionary[tilePosition];
            if (crop.IsFullyGrown())
            {
                Vector2 direction = (position - (Vector2)Player.Instance.transform.position).normalized;
                Player.Instance.PlayPickUpFruitAnimation(direction);
                crop.Harvest(Player.Instance);
                cropsDictionary.Remove(tilePosition);
                SaveCrops();
            }
        }
    }
    void Update()
    {
        if (CropManager.Instance != null)
        {
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int tilePosition = soilTilemap.WorldToCell(mousePos);
            if (cropsDictionary.ContainsKey(tilePosition))
            {
                Crop crop = cropsDictionary[tilePosition];
                if (crop.IsFullyGrown())
                {
                    Cursor.SetCursor(harvestCursor, Vector2.zero, CursorMode.Auto);
                    if (Input.GetMouseButtonDown(0))
                    {
                        HarvestCrop(mousePos);
                    }
                }
                else
                {
                    Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
                }
            }
            else
            {
                Cursor.SetCursor(defaultCursor, Vector2.zero, CursorMode.Auto);
            }
        }
    }
    public void SaveCrops()
    {
        List<CropSaveData> saveDataList = new List<CropSaveData>();
        foreach (var kvp in cropsDictionary)
        {
            Crop crop = kvp.Value;
            CropSaveData data = new CropSaveData()
            {
                cropName = crop.CropName,
                position = kvp.Key,
                stage = crop.CurrentStage,
                daysInStage = crop.DaysInCurrentStage,
                isWatered = crop.IsWatered()
            };
            saveDataList.Add(data);
        }
        string json = JsonUtility.ToJson(new CropSaveDataListWrapper { crops = saveDataList }, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/crops.json", json);
        Debug.Log("✅ Crops saved!");
    }
    public void SaveCropAt(Vector3Int tilePosition)
    {
        if (!cropsDictionary.TryGetValue(tilePosition, out var crop)) return;

        string path = Application.persistentDataPath + "/crops.json";
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        CropSaveDataListWrapper wrapper = JsonUtility.FromJson<CropSaveDataListWrapper>(json);

        bool found = false;
        foreach (var data in wrapper.crops)
        {
            if (data.position == tilePosition)
            {
                data.isWatered = true;
                found = true;
                break;
            }
        }

        if (found)
        {
            string updatedJson = JsonUtility.ToJson(wrapper, true);
            File.WriteAllText(path, updatedJson);
            Debug.Log($"💾 [CropManager] Updated isWatered = true at {tilePosition}");
        }
        else
        {
            Debug.LogWarning($"❌ [CropManager] Cannot find crop at {tilePosition} to update.");
        }
    }

    [System.Serializable]
    private class CropSaveDataListWrapper
    {
        public List<CropSaveData> crops;
    }
    public void LoadCrops()
    {
        string path = Application.persistentDataPath + "/crops.json";
        if (!System.IO.File.Exists(path))
        {
            Debug.Log("No crops save file found.");
            return;
        }
        string json = System.IO.File.ReadAllText(path);
        CropSaveDataListWrapper wrapper = JsonUtility.FromJson<CropSaveDataListWrapper>(json);
        Debug.Log($"📦 Loaded {wrapper.crops.Count} crop save data entries from file.");
        foreach (CropSaveData data in wrapper.crops)
        {
            if (!soilTilemap.HasTile(data.position)) continue;
            CropData cropData = CropDatabase.Instance.GetCropDataByName(data.cropName);
            if (cropData == null)
            {
                Debug.LogWarning("❌ Không tìm thấy CropData: " + data.cropName);
                continue;
            }
            Vector3 spawnPosition = soilTilemap.CellToWorld(data.position) + new Vector3(0.5f, 0.5f, 0);
            Debug.Log($"🌱 Spawning crop '{data.cropName}' at tile {data.position}, world position {spawnPosition}");
            GameObject newCrop = Instantiate(cropPrefab, spawnPosition, Quaternion.identity, cropParent);
            Crop cropScript = newCrop.GetComponent<Crop>();
            cropScript.Initialize(cropData);
            cropScript.SetState(data.stage, data.daysInStage);
            cropsDictionary[data.position] = cropScript;
        }
        Debug.Log("✅ Crops loaded!");
    }
}
