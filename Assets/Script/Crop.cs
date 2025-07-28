using UnityEngine;
using UnityEngine.Tilemaps;

public class Crop : MonoBehaviour
{
    private CropData cropData;
    private int currentStage = 0;
    private int daysInCurrentStage = 0;
    private bool isWatered = false;
    private CropVisual cropVisual;
    public string CropName => cropData.cropName;
    public int CurrentStage => currentStage;
    public int DaysInCurrentStage => daysInCurrentStage;
    public bool IsWatered() => isWatered;
    private Vector3Int tilePosition;
    public GameObject fruitPickupEffectPrefab;
    private void OnTileWatered(Vector3Int wateredTile)
    {
        if (wateredTile == tilePosition)
        {
            Debug.Log("iswatered");
            isWatered = true;
            CropManager.Instance?.SaveCropAt(tilePosition);
        }
    }
    public void SetState(int stage, int days)
    {
        currentStage = stage;
        daysInCurrentStage = days;
        cropVisual.SetSprite(cropData.growthStages[currentStage]);
    }

    private void Awake()
    {
        cropVisual = GetComponentInChildren<CropVisual>();
    }
    public void Initialize(CropData data)
    {
        cropData = data;
        cropVisual.SetSprite(cropData.growthStages[currentStage]);
        CheckIfWatered();
    }
    private void Start()
    {
        GameTimeManager.Instance.OnNewDayStarted += OnNewDayStarted;
        tilePosition = TilemapDiggingManager.Instance.WateredTilemap.WorldToCell(transform.position);
        TilemapDiggingManager.Instance.OnTileWateredRaw += OnTileWatered;
        CheckIfWatered();
    }

    private void OnDestroy()
    {
        GameTimeManager.Instance.OnNewDayStarted -= OnNewDayStarted;
        if (TilemapDiggingManager.Instance != null)
            TilemapDiggingManager.Instance.OnTileWateredRaw -= OnTileWatered;
    }
    private void OnNewDayStarted()
    {
        CheckIfWatered();
        if (cropData.requiresWater && !isWatered) return;
        daysInCurrentStage++;
        if (currentStage < cropData.daysPerStage.Length && daysInCurrentStage >= cropData.daysPerStage[currentStage])
        {
            daysInCurrentStage = 0;
            currentStage++;
            cropVisual.SetSprite(cropData.growthStages[currentStage]);
        }
    }
    private void CheckIfWatered()
    {
        Vector3Int tilePosition = TilemapDiggingManager.Instance.WateredTilemap.WorldToCell(transform.position);
        isWatered = TilemapDiggingManager.Instance.WateredTilemap.HasTile(tilePosition);
        CropManager.Instance?.SaveCropAt(tilePosition);
    }
    public bool IsFullyGrown()
    {
        return currentStage >= cropData.growthStages.Length - 1;
    }
    public void Harvest(Player player)
    {
        if (IsFullyGrown())
        {
            Sprite fruitSprite = itemsAssets.Instance.GetItemSprite(cropData.harvestItemName);
            Vector2 dir = (transform.position - player.transform.position).normalized;
            Transform spawnPoint = GetFruitSpawnPoint(dir, player);
            Vector3 targetPos = player.fruitPickupTarget.position;
            GameObject fx = Instantiate(fruitPickupEffectPrefab, spawnPoint.position, Quaternion.identity);
            fx.GetComponent<FruitPickupEffect>().Setup(fruitSprite, targetPos);
            player.AddToInventory(cropData.harvestItemName);
            Destroy(gameObject);
        }
    }
    private Transform GetFruitSpawnPoint(Vector2 direction, Player player)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return (direction.x > 0) ? player.fruitPickupRight : player.fruitPickupLeft;
        }
        else
        {
            return (direction.y > 0) ? player.fruitPickupTop : player.fruitPickupBottom;
        }
    }
    public void Shake()
    {
        cropVisual.Shake();
    }
}
