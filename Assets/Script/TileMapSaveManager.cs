using System.Collections.Generic;
using System.IO;
using UnityEngine;
[System.Serializable]
public class TileSaveData
{
    public List<Vector3Int> soilTiles = new();
    public List<Vector3Int> wateredTiles = new();
}
public static class TilemapSaveManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "tilemap_save.json");
    public static void Save(TileSaveData data)
    {
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, json);
        Debug.Log($"[TileSaveManager] Saved to {SavePath}");
    }
    public static TileSaveData Load()
    {
        if (!File.Exists(SavePath)) return new TileSaveData();
        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<TileSaveData>(json);
    }
    public static void DeleteSave()
    {
        if (File.Exists(SavePath))
            File.Delete(SavePath);
    }
}
