using UnityEngine;

[CreateAssetMenu(fileName = "NewCropData", menuName = "Crops/New Crop")]
public class CropData : ScriptableObject
{
    public string cropName;
    public Sprite[] growthStages;
    public int[] daysPerStage;
    public bool requiresWater = true;
    public string harvestItemName;
    public int harvestAmount = 1;
    public int growingSeason;
}
