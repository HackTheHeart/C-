using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Tilemap waterTilemap;
    [SerializeField] private FishingMiniGame fishingMiniGame;
    public static FishingManager Instance { get; private set; }
    private bool isFishing = false;
    private bool isHooked = false;
    private bool isRolling = false;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        player = FindFirstObjectByType<Player>();
    }
    public void TryStartFishing()
    {
        if (isFishing) return;
        if (IsFacingWater())
        {
            StartFishing();
        }
        else
        {
            //FishingAnimation.Instance.PlayFailedFishingAnimation();
        }
    }
    private bool IsFacingWater()
    {
        PolygonCollider2D collider = Player.Instance.GetComponent<PolygonCollider2D>();
        float minY = float.MaxValue, centerY = Player.Instance.transform.position.y;
        foreach (Vector2 point in collider.points)
        {
            Vector2 worldPoint = Player.Instance.transform.TransformPoint(point);
            if (worldPoint.y < minY) minY = worldPoint.y;
        }
        Vector2 feetPosition = new Vector2(Player.Instance.transform.position.x, minY);
        Vector3Int currentTilePosition = waterTilemap.WorldToCell(feetPosition);
        Vector3Int centerTilePosition = waterTilemap.WorldToCell(new Vector2(Player.Instance.transform.position.x, centerY));
        Vector3Int direction = new Vector3Int(Mathf.RoundToInt(Player.Instance.LastInteractDir.x), Mathf.RoundToInt(Player.Instance.LastInteractDir.y), 0);
        Vector3Int nextTilePositionFeet = currentTilePosition + direction;
        Vector3Int nextTilePositionCenter = centerTilePosition + direction;
        bool hasWaterTileFeet = waterTilemap.HasTile(nextTilePositionFeet);
        bool hasWaterTileCenter = waterTilemap.HasTile(nextTilePositionCenter);
        return hasWaterTileFeet || hasWaterTileCenter;
    }
    private void StartFishing()
    {
        isFishing = true;
        player.SetPerformingAction(true);
        FishingAnimation.Instance.StartFishing();
        FishingAnimation.Instance.StartCoroutine(FishingAnimation.Instance.WaitForHook());
    }
    public void TryStartRolling()
    {
        isHooked = false;
        isRolling = true;
        StartFishingMiniGame();
    }
    private void StartFishingMiniGame()
    {
        fishingMiniGame.gameObject.SetActive(true);
        fishingMiniGame.StartMiniGame(OnMiniGameComplete);
    }
    private void OnMiniGameComplete(bool success)
    {
        
    }
    public void CancelFishing()
    {
        if (!isFishing) return;
        ResetFishingState();
    }
    public void ResetFishingState()
    {
        isFishing = false;
        isHooked = false;
        isRolling = false;
        player.SetPerformingAction(false);
        fishingMiniGame.gameObject.SetActive(false);
        FishingAnimation.Instance.ResetAnimation();
    }
}
