using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System;
using System.Collections.Generic;

public class FishingMiniGame : MonoBehaviour
{
    public static FishingMiniGame Instance { get; private set; }
    [SerializeField] private Image fishImage;
    [SerializeField] private Image fishingBarImage;
    [SerializeField] private Image progressBarImage;
    [SerializeField] private RectTransform fishingImage;
    [SerializeField] private float fishSpeed = 200f;
    [SerializeField] private float barSpeed = 100f;
    [SerializeField] private float progressDecreaseRate = 0.001f;
    [SerializeField] private float progressIncreaseRate = 0.001f;    
    [SerializeField] private GameObject uiPanel;
    [SerializeField] private bool isRiver;
    private List<string> riverFishList = new List<string>
    {
        "Bullhead Catfish", "Carp", "Chub", "Discus", "Dorado", "Eel",
        "Ghost Catfish", "Largemouth Bass", "Perch", "Pike Fish", "Piranha",
        "Rainbow Trout", "Shad", "Sturgeon", "Sunfish", "Tiger Trout", "Walleye"
    };
    private List<string> seaFishList = new List<string>
    {
        "Albacore", "Anchovy", "Anglerfish", "BlobFish", "Bream",
        "Crimson Snapper", "Flounder", "Goby", "Halibut", "Herring",
        "Lingcod", "LionFish", "Pufferfish", "Red Mullet", "Red Snapper",
        "Salmon", "Sardine", "Sea bullhead", "Smallmouth Bass", "Tuna"
    };
    private Dictionary<string, float> fishDifficulties = new Dictionary<string, float>
    {
        { "Bullhead Catfish", 0.15f },
        { "Carp", 0.1f },
        { "Chub", 0.12f },
        { "Discus", 0.22f },
        { "Dorado", 0.17f },
        { "Eel", 0.16f },
        { "Ghost Catfish", 0.28f },
        { "Largemouth Bass", 0.17f },
        { "Perch", 0.13f },
        { "Pike Fish", 0.17f },
        { "Piranha", 0.33f },
        { "Rainbow Trout", 0.14f },
        { "Shad", 0.13f },
        { "Sturgeon", 0.28f },
        { "Sunfish", 0.1f },
        { "Tiger Trout", 0.22f },
        { "Walleye", 0.18f },
        { "Albacore", 0.15f },
        { "Anchovy", 0.1f },
        { "Anglerfish", 1.0f },
        { "BlobFish", 0.12f },
        { "Bream", 0.59f },
        { "Crimson Snapper", 0.22f },
        { "Flounder", 0.17f },
        { "Goby", 0.22f },
        { "Halibut", 0.15f },
        { "Herring", 0.1f },
        { "Lingcod", 0.19f },
        { "LionFish", 0.17f },
        { "Pufferfish", 0.28f },
        { "Red Mullet", 0.15f },
        { "Red Snapper", 0.12f },
        { "Salmon", 0.15f },
        { "Sardine", 0.11f },
        { "Sea bullhead", 0.22f },
        { "Smallmouth Bass", 0.12f },
        { "Tuna", 0.17f }
    };
    private Dictionary<string, List<string>> fishSeasons = new Dictionary<string, List<string>>
    {
        { "Bullhead Catfish", new List<string> { "Spring", "Fall" } },
        { "Carp", new List<string> { "Spring", "Summer", "Fall", "Winter" } },
        { "Chub", new List<string> { "Spring", "Summer", "Fall", "Winter" } },
        { "Discus", new List<string> { "Summer" } },
        { "Dorado", new List<string> { "Summer" } },
        { "Eel", new List<string> { "Spring", "Fall" } },
        { "Ghost Catfish", new List<string> { "Fall", "Winter" } },
        { "Largemouth Bass", new List<string> { "Spring", "Summer", "Fall", "Winter" } },
        { "Perch", new List<string> { "Winter" } },
        { "Pike Fish", new List<string> { "Summer", "Winter" } },
        { "Piranha", new List<string> { "Summer" } },
        { "Rainbow Trout", new List<string> { "Summer" } },
        { "Shad", new List<string> { "Spring", "Summer", "Fall" } },
        { "Sturgeon", new List<string> { "Summer", "Winter" } },
        { "Sunfish", new List<string> { "Spring", "Summer" } },
        { "Tiger Trout", new List<string> { "Fall", "Winter" } },
        { "Walleye", new List<string> { "Fall", "Winter" } },
        { "Albacore", new List<string> { "Fall", "Winter" } },
        { "Anchovy", new List<string> { "Spring", "Fall" } },
        { "Anglerfish", new List<string> { "Fall" } },
        { "BlobFish", new List<string> { "Winter" } },
        { "Bream", new List<string> { "Spring", "Summer", "Fall" } },
        { "Crimson Snapper", new List<string> { "Summer" } },
        { "Flounder", new List<string> { "Spring", "Summer" } },
        { "Goby", new List<string> { "Fall", "Winter" } },
        { "Halibut", new List<string> { "Spring", "Summer", "Winter" } },
        { "Herring", new List<string> { "Spring", "Winter" } },
        { "Lingcod", new List<string> { "Winter" } },
        { "LionFish", new List<string> { "Summer" } },
        { "Pufferfish", new List<string> { "Summer" } },
        { "Red Mullet", new List<string> { "Summer", "Winter" } },
        { "Red Snapper", new List<string> { "Summer", "Fall" } },
        { "Salmon", new List<string> { "Fall" } },
        { "Sardine", new List<string> { "Spring", "Fall", "Winter" } },
        { "Sea bullhead", new List<string> { "Winter" } },
        { "Smallmouth Bass", new List<string> { "Spring", "Fall" } },
        { "Tuna", new List<string> { "Summer", "Winter" } }
    };
    private Vector2 fishStartPos;
    private Vector2 barStartPos;
    private float progress;
    private bool isGameOver;
    private System.Action<bool> onGameComplete;
    private float maxY, minY;
    private float originalMinY, originalMaxY;
    public event Action<bool> OnMiniGameEnd;
    private string targetFish;
    private float targetFishDifficulty;
    private string GetCurrentSeasonName()
    {
        string[] seasons = { "Spring", "Summer", "Fall", "Winter" };
        return seasons[GameTimeManager.Instance.Season];
    }
    public float GetProgress()
    {
        return progress;
    }
    public bool IsGameOver => isGameOver;
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
    public void StartMiniGame(System.Action<bool> callback)
    {
        onGameComplete = callback;
        gameObject.SetActive(true);
        uiPanel.SetActive(true);
        isGameOver = false;
        targetFish = GetRandomFish();
        if (string.IsNullOrEmpty(targetFish))
        {
            Debug.LogWarning("Không tìm được cá hợp lệ!");
            EndMiniGame(false);
            return;
        }
        progress = 0.33f;
        progressBarImage.fillAmount = progress;
        StartCoroutine(SetupGame());
    }
    private IEnumerator SetupGame()
    {
        yield return StartCoroutine(UpdateFishingBounds());
        fishStartPos = fishImage.rectTransform.anchoredPosition;
        barStartPos = fishingBarImage.rectTransform.anchoredPosition;
        ResetMiniGame();
    }
    private void ResetMiniGame()
    {
        isGameOver = false;
        progress = 0.33f;
        progressBarImage.fillAmount = progress;
        fishImage.rectTransform.anchoredPosition = fishStartPos;
        fishingBarImage.rectTransform.anchoredPosition = barStartPos;
        minY = originalMinY;
        maxY = originalMaxY;
    }
    private void Update()
    {
        if (isGameOver) return;
        if (Input.GetKey(KeyCode.Space))
            MoveFishingBarUp();
        else
            MoveFishingBarDown();
        UpdateProgress();
    }
    private IEnumerator UpdateFishingBounds()
    {
        yield return new WaitForEndOfFrame();
        float fishStartY = fishImage.rectTransform.anchoredPosition.y;
        float moveRange = fishingImage.rect.height - fishImage.rectTransform.rect.height;
        if (originalMinY == 0 && originalMaxY == 0)
        {
            originalMinY = fishStartY;
            originalMaxY = fishStartY + moveRange;
        }
        minY = originalMinY;
        maxY = originalMaxY;

        StartCoroutine(MoveFish());
    }
    private void MoveFishingBarUp()
    {
        fishingBarImage.rectTransform.anchoredPosition += new Vector2(0, barSpeed * Time.deltaTime);
        fishingBarImage.rectTransform.anchoredPosition = new Vector2(
            fishingBarImage.rectTransform.anchoredPosition.x,
            Mathf.Clamp(fishingBarImage.rectTransform.anchoredPosition.y, minY, maxY)
        );
    }
    private IEnumerator MoveFish()
    {
        while (!isGameOver)
        {
            float direction = UnityEngine.Random.Range(-1f, 1f);
            float moveTime = UnityEngine.Random.Range(0.2f, 1.2f);
            float timer = 0f;
            float difficultyMultiplier = Mathf.Lerp(1f, 3f, targetFishDifficulty);
            float newSpeed = fishSpeed * difficultyMultiplier;
            while (timer < moveTime)
            {
                fishImage.rectTransform.anchoredPosition += new Vector2(0, direction * newSpeed * Time.deltaTime);
                fishImage.rectTransform.anchoredPosition = new Vector2(
                    fishImage.rectTransform.anchoredPosition.x,
                    Mathf.Clamp(fishImage.rectTransform.anchoredPosition.y, minY, maxY)
                );
                timer += Time.deltaTime;
                yield return null;
            }
        }
    }
    private void UpdateProgress()
    {
        float progressModifier = 0.2f;
        if (IsFishInsideBar())
            progress += (progressIncreaseRate * progressModifier) * Time.deltaTime;
        else
            progress -= progressDecreaseRate * Time.deltaTime;
        progress = Mathf.Clamp01(progress);
        progressBarImage.fillAmount = progress;
        if (progress >= 1f)
            EndMiniGame(true);
        else if (progress <= 0f)
            EndMiniGame(false);
    }
    private bool IsFishInsideBar()
    {
        float fishTop = fishImage.rectTransform.anchoredPosition.y + fishImage.rectTransform.rect.height / 2;
        float fishBottom = fishImage.rectTransform.anchoredPosition.y - fishImage.rectTransform.rect.height / 2;
        float barTop = fishingBarImage.rectTransform.anchoredPosition.y + fishingBarImage.rectTransform.rect.height / 2;
        float barBottom = fishingBarImage.rectTransform.anchoredPosition.y - fishingBarImage.rectTransform.rect.height / 2;
        return fishBottom < barTop && fishTop > barBottom;
    }
    private void MoveFishingBarDown()
    {
        fishingBarImage.rectTransform.anchoredPosition -= new Vector2(0, barSpeed * Time.deltaTime);
        fishingBarImage.rectTransform.anchoredPosition = new Vector2(
            fishingBarImage.rectTransform.anchoredPosition.x,
            Mathf.Clamp(fishingBarImage.rectTransform.anchoredPosition.y, minY, maxY)
        );
    }
    private string GetRandomFish()
    {
        string currentSeason = GetCurrentSeasonName();
        List<string> fishPool = isRiver ? riverFishList : seaFishList;
        List<string> validFish = fishPool.FindAll(fish =>
            fishSeasons.ContainsKey(fish) && fishSeasons[fish].Contains(currentSeason)
        );
        if (validFish.Count == 0)
        {
            Debug.LogWarning("Không có cá hợp mùa hiện tại!");
            return null;
        }
        string fish = validFish[UnityEngine.Random.Range(0, validFish.Count)];
        targetFish = fish;
        fishDifficulties.TryGetValue(fish, out targetFishDifficulty);
        return fish;
    }
    private void EndMiniGame(bool success)
    {
        if (success && !string.IsNullOrEmpty(targetFish))
        {
            Player.Instance.AddToInventory(targetFish, 1);
            Debug.Log("Bắt được cá: " + targetFish);
        }
        isGameOver = true;
        onGameComplete?.Invoke(success);
        OnMiniGameEnd?.Invoke(success);
        gameObject.SetActive(false);
        HideMiniGame();
    }
    public void HideMiniGame()
    {
        gameObject.SetActive(false);
        uiPanel.SetActive(false);
    }
}
