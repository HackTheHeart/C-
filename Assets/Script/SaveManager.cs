//using System.IO;
//using UnityEngine;
//using System.Collections.Generic;

//[System.Serializable]
//public class CropSaveData
//{
//    public Vector3Int tilePosition;
//    public string cropName;
//    public int currentStage;
//    public int daysInCurrentStage;
//}

//[System.Serializable]
//public class TileSaveData
//{
//    public List<Vector3Int> soilTiles = new();
//    public List<Vector3Int> wateredTiles = new();
//    public List<CropSaveData> crops = new();
//}

//public class SaveManager : MonoBehaviour
//{
//    private string savePath => Application.persistentDataPath + "/farm_save.json";
//    public void SaveFarm()
//    {
//        TileSaveData saveData = new();
//        foreach (var pos in TilemapDiggingManager.Instance.SoilTilemap.cellBounds.allPositionsWithin)
//        {
//            if (TilemapDiggingManager.Instance.SoilTilemap.HasTile(pos))
//                saveData.soilTiles.Add(pos);
//        }
//        foreach (var pos in TilemapDiggingManager.Instance.WateredTilemap.cellBounds.allPositionsWithin)
//        {
//            if (TilemapDiggingManager.Instance.WateredTilemap.HasTile(pos))
//                saveData.wateredTiles.Add(pos);
//        }
//        foreach (var kvp in CropManager.Instance.GetAllCrops())
//        {
//            Crop crop = kvp.Value;
//            CropSaveData cropData = new()
//            {
//                tilePosition = kvp.Key,
//                cropName = crop.CropName, // thêm property nếu chưa có
//                currentStage = crop.CurrentStage,
//                daysInCurrentStage = crop.DaysInCurrentStage
//            };
//            saveData.crops.Add(cropData);
//        }
//        string json = JsonUtility.ToJson(saveData, true);
//        File.WriteAllText(savePath, json);
//    }

//    public void LoadFarm()
//    {
//        if (!File.Exists(savePath)) return;

//        string json = File.ReadAllText(savePath);
//        TileSaveData saveData = JsonUtility.FromJson<TileSaveData>(json);

//        // Soil tiles
//        foreach (var pos in saveData.soilTiles)
//        {
//            TilemapDiggingManager.Instance.SoilTilemap.SetTile(pos, TilemapDiggingManager.Instance.soilRuleTile);
//        }

//        // Watered tiles
//        foreach (var pos in saveData.wateredTiles)
//        {
//            TilemapDiggingManager.Instance.WateredTilemap.SetTile(pos, TilemapDiggingManager.Instance.wateredRuleTile);
//        }

//        // Crops
//        foreach (var crop in saveData.crops)
//        {
//            CropData data = CropDatabase.Instance.GetCropByName(crop.cropName); // bạn cần class CropDatabase
//            CropManager.Instance.PlantCrop(TilemapDiggingManager.Instance.SoilTilemap.CellToWorld(crop.tilePosition), data);

//            Crop plantedCrop = CropManager.Instance.GetCropAt(crop.tilePosition);
//            if (plantedCrop != null)
//            {
//                plantedCrop.SetState(crop.currentStage, crop.daysInCurrentStage); // cần thêm method này
//            }
//        }
//    }
//}
