//using UnityEngine;

//public class DebugSavePath : MonoBehaviour
//{
//    void Start()
//    {
//        // In ra đường dẫn của persistentDataPath trong Unity console
//        Debug.Log($"Persistent Data Path: {Application.persistentDataPath}");
//    }
//}
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CropDatabase : MonoBehaviour
{
    public static CropDatabase Instance { get; private set; }
    [Header("Danh sách tất cả CropData trong game")]
    public List<CropData> cropList;
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
    }
    public CropData GetCropDataByName(string cropName)
    {
        return cropList.FirstOrDefault(crop => crop.cropName == cropName);
    }
}
