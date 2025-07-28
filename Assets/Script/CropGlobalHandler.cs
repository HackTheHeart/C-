using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.IO;
using UnityEditor.ShaderGraph;
using static UnityEditor.PlayerSettings;
using static UnityEngine.Rendering.Universal.PixelPerfectCamera;

public class CropGlobalHandler : MonoBehaviour
{
    public static CropGlobalHandler Instance { get; private set; }
    [SerializeField] private Tilemap wateredTilemap;
    private HashSet<Vector3Int> wateredTiles = new HashSet<Vector3Int>();
    private Dictionary<Vector3Int, CropSaveData> cropMemoryData = new Dictionary<Vector3Int, CropSaveData>();
    private string savePath => Application.persistentDataPath + "/crops.json";
    private int lastSeason = 0;
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
        GameTimeManager.Instance.OnNewDayStarted += OnNewDayStarted;
    }
    private void OnDestroy()
    {
        if (GameTimeManager.Instance != null)
            GameTimeManager.Instance.OnNewDayStarted -= OnNewDayStarted;
    }
    public void AddWateredTile(Vector3Int tilePos)
    {
        wateredTiles.Add(tilePos);
        if (cropMemoryData.TryGetValue(tilePos, out var crop))
        {
            crop.isWatered = true;
        }
    }
    public void SetCropData(Vector3Int position, CropSaveData data)
    {
        cropMemoryData[position] = data;
        if (wateredTiles.Contains(position))
        {
            cropMemoryData[position].isWatered = true;
        }
    }
    public CropSaveData GetCropData(Vector3Int position)
    {
        cropMemoryData.TryGetValue(position, out var data);
        return data;
    }
    public bool IsTileWatered(Vector3Int position)
    {
        return wateredTiles.Contains(position);
    }
    public void OnNewDayStarted()
    {
        LoadCropDataFromJson();

        int currentSeason = GameTimeManager.Instance.Season;
        bool isNewSeason = (currentSeason != lastSeason);
        lastSeason = currentSeason;

        List<Vector3Int> cropsToRemove = new List<Vector3Int>();

        foreach (var kvp in cropMemoryData)
        {
            // neu chuyen mua xoa het cay truoc di
            Vector3Int pos = kvp.Key;
            CropSaveData crop = kvp.Value;
            string cropName = crop.cropName;
            if (isNewSeason && cropName != "Corn" && cropName != "Sunflower")
            {
                cropsToRemove.Add(pos);
                continue;
            }
            if (crop.isWatered)
            {
                crop.daysInStage++;
                CropData cropData = CropDatabase.Instance.GetCropDataByName(crop.cropName);
                if (cropData != null && crop.stage < cropData.daysPerStage.Length &&
                    crop.daysInStage >= cropData.daysPerStage[crop.stage])
                {
                    crop.stage++;
                    crop.daysInStage = 0;
                }
            }
            string log = $"🌱 [Crop] {crop.cropName} | Stage: {crop.stage} | DaysInStage: {crop.daysInStage} | Watered: {crop.isWatered}";
            Debug.Log(log);
            crop.isWatered = false;
        }
        foreach (var pos in cropsToRemove)
        {
            cropMemoryData.Remove(pos);
            wateredTiles.Remove(pos);
        }
        SaveCropDataToJson();
    }
    // Old Function
    //public void OnNewDayStarted()
    //{
    //    LoadCropDataFromJson();
    //    foreach (var kvp in cropMemoryData)
    //    {
    //        // neu chuyen mua xoa het cay truoc di
    //        var crop = kvp.Value;
    //        if (crop.isWatered)
    //        {
    //            crop.daysInStage++;
    //            CropData cropData = CropDatabase.Instance.GetCropDataByName(crop.cropName);
    //            if (cropData != null && crop.stage < cropData.daysPerStage.Length &&
    //                crop.daysInStage >= cropData.daysPerStage[crop.stage])
    //            {
    //                crop.stage++;
    //                crop.daysInStage = 0;
    //            }
    //        }
    //        string log = $"🌱 [Crop] {crop.cropName} | Stage: {crop.stage} | DaysInStage: {crop.daysInStage} | Watered: {crop.isWatered}";
    //        Debug.Log(log);
    //        crop.isWatered = false;
    //    }
    //    SaveCropDataToJson();
    //}
    public void SaveCropDataToJson()
    {
        List<CropSaveData> list = new List<CropSaveData>(cropMemoryData.Values);
        string json = JsonUtility.ToJson(new CropSaveDataListWrapper { crops = list }, true);
        File.WriteAllText(savePath, json);
        Debug.Log("💾 CropGlobalHandler saved crop data!");
    }

    public void LoadCropDataFromJson()
    {
        cropMemoryData.Clear();
        wateredTiles.Clear();

        if (!File.Exists(savePath))
        {
            Debug.Log("🌱 No crop global save file found.");
            return;
        }

        string json = File.ReadAllText(savePath);
        var wrapper = JsonUtility.FromJson<CropSaveDataListWrapper>(json);
        foreach (var data in wrapper.crops)
        {
            cropMemoryData[data.position] = data;
            if (data.isWatered)
            {
                wateredTiles.Add(data.position);
            }
        }

        Debug.Log("🌾 CropGlobalHandler loaded crop data!");
    }

    [System.Serializable]
    private class CropSaveDataListWrapper
    {
        public List<CropSaveData> crops;
    }

}
