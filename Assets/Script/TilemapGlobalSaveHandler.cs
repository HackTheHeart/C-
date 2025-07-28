using UnityEngine;

public class TilemapGlobalSaveHandler : MonoBehaviour
{
    public static TilemapGlobalSaveHandler Instance;
    private TileSaveData memorySaveData;
    private bool isNewDayProcessed = false;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        GameTimeManager.Instance.OnNewDayStarted += OnNewDayStarted;
    }
    private void OnDisable()
    {
        if (GameTimeManager.Instance != null)
            GameTimeManager.Instance.OnNewDayStarted -= OnNewDayStarted;
    }
    //private void OnNewDayStarted()
    //{
    //    isNewDayProcessed = true;
    //    if (memorySaveData == null)
    //    {
    //        memorySaveData = TilemapSaveManager.Load();
    //        Debug.Log("💾 [Global] Loaded old save for new day.");
    //    }
    //    memorySaveData.wateredTiles.Clear();
    //    TilemapSaveManager.Save(memorySaveData);
    //    Debug.Log("✅ [Global] Cleared watered tiles and saved to JSON.");
    //}
    private void OnNewDayStarted()
    {
        isNewDayProcessed = true;

        // Load lại dữ liệu nếu chưa có trong bộ nhớ
        if (memorySaveData == null)
        {
            memorySaveData = TilemapSaveManager.Load();
            Debug.Log("💾 [Global] Loaded old save for new day.");
        }

        //// ❗ XÓA CÁC Ô ĐẤT KHÔNG CÓ CÂY
        //memorySaveData.soilTiles.RemoveAll(pos =>
        //{
        //    var crop = CropGlobalHandler.Instance?.GetCropData(pos);
        //    bool shouldRemove = crop == null;
        //    if (shouldRemove)
        //        Debug.Log($"🗑️ [Global] Removed soil at {pos} (no crop)");
        //    return shouldRemove;
        //});

        // ✅ XÓA NƯỚC mỗi ngày mới như cũ
        memorySaveData.wateredTiles.Clear();

        // 💾 LƯU FILE
        TilemapSaveManager.Save(memorySaveData);
        Debug.Log("✅ [Global] Cleaned up unused soil and cleared watered tiles.");
    }

    public void ReceiveMemory(TileSaveData fromScene)
    {
        memorySaveData = fromScene;
        Debug.Log("📤 [Global] Received memory from scene.");
    }
    public TileSaveData GetCurrentSaveData()
    {
        return memorySaveData;
    }
    public bool IsNewDayProcessed()
    {
        return isNewDayProcessed;
    }
    public void ResetNewDayFlag()
    {
        isNewDayProcessed = false;
    }

    public void ClearMemory()
    {
        memorySaveData = null;
    }
}
