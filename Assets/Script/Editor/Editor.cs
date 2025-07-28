using UnityEditor;
using UnityEngine;

public class CropDataEditor : EditorWindow
{
    [MenuItem("Tools/Update CropData")]
    public static void UpdateAllCropData()
    {
        string[] guids = AssetDatabase.FindAssets("t:CropData");
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            CropData cropData = AssetDatabase.LoadAssetAtPath<CropData>(path);
            if (cropData != null)
            {
                cropData.harvestItemName = cropData.cropName;
                cropData.harvestAmount = 1;
                EditorUtility.SetDirty(cropData);
            }
        }
        AssetDatabase.SaveAssets();
    }
}
