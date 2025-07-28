using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WeatherManager : MonoBehaviour
{
    public static WeatherManager Instance;
    [Header("Season Prefabs")]
    public GameObject springPrefab;
    public GameObject summerPrefab;
    public GameObject fallPrefab;
    public GameObject winterPrefab;
    [Header("Rain System")]
    public ParticleSystem rainEffect;
    public AudioSource rainSound;
    private GameObject currentSeason;
    private bool isRaining = false;
    public bool IsRaining
    {
        get { return isRaining; }
        private set
        {
            if (isRaining != value)
            {
                isRaining = value;
                SetRain(isRaining);
                Debug.Log("Rain status changed to: " + isRaining);
                if (isRaining)
                {
                    OnRainStarted?.Invoke();
                }
            }
        }
    }
    public event Action OnRainStarted;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        if (GameTimeManager.Instance != null && GameTimeManager.Instance.Day == 1)
        {
            isRaining = false;
            SetRain(isRaining);
        }
        GameTimeManager.Instance.OnNewDayStarted += UpdateWeather;
        UpdateWeather();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject rainObj = GameObject.Find("RainSystem");
        if (rainObj != null)
        {
            rainEffect = rainObj.GetComponentInChildren<ParticleSystem>();
        }
        rainSound = GameObject.FindFirstObjectByType<AudioSource>(); 
        SetRain(isRaining);
    }
    void UpdateWeather()
    {
        SetSeason(GameTimeManager.Instance.Season);
        float rainChance = GetRainChance(GameTimeManager.Instance.Season);
        bool shouldRain = UnityEngine.Random.value < rainChance;
        if (shouldRain != isRaining)
        {
            IsRaining = shouldRain;
        }
    }
    void SetSeason(int seasonIndex)
    {
        GameObject oldSeason = GameObject.FindWithTag("SeasonMap");
        if (oldSeason != null)
        {
            Destroy(oldSeason);
        }
        GameObject newSeasonPrefab = null;
        switch (seasonIndex)
        {
            case 0: newSeasonPrefab = springPrefab; break;
            case 1: newSeasonPrefab = summerPrefab; break;
            case 2: newSeasonPrefab = fallPrefab; break;
            case 3: newSeasonPrefab = winterPrefab; break;
        }
        if (newSeasonPrefab != null)
        {
            currentSeason = Instantiate(newSeasonPrefab);
            currentSeason.tag = "SeasonMap";
        }
    }
    float GetRainChance(int season)
    {
        switch (season)
        {
            case 0: return 0.3f; 
            case 1: return 0.3f; 
            case 2: return 0.2f; 
            case 3: return 0.2f; 
            default: return 0.2f;
        }
    }
    void SetRain(bool enableRain)
    {
        if (rainEffect != null)
        {
            if (enableRain && !rainEffect.isPlaying)
            {
                rainEffect.Play();
            }
            else if (!enableRain && rainEffect.isPlaying)
            {
                rainEffect.Stop();
            }
        }

        if (rainSound != null)
        {
            if (enableRain && !rainSound.isPlaying)
            {
                rainSound.Play();
            }
            else if (!enableRain && rainSound.isPlaying)
            {
                rainSound.Stop();
            }
        }
    }
    [SerializeField]
    private bool testRainToggle;
    private bool lastTestRainToggle;
    void Update()
    {
        if (testRainToggle != lastTestRainToggle)
        {
            lastTestRainToggle = testRainToggle;
            IsRaining = testRainToggle;
            Debug.Log("Test Rain toggled via Inspector. Now raining: " + IsRaining);
        }
    }

}
