using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DayNightCycle2D : MonoBehaviour
{
    public static DayNightCycle2D Instance { get; private set; }
    [SerializeField] private Light2D globalLight;
    private bool isRaining;
    private void Awake()
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
    void Update()
    {
        if (GameTimeManager.Instance == null || globalLight == null)
            return;
        float timeOfDay = GameTimeManager.Instance.TimeOfDay;
        isRaining = WeatherManager.Instance.IsRaining;
        UpdateLighting(timeOfDay);
    }
    void UpdateLighting(float timeOfDay)
    {
        if (isRaining)
        {
            if (timeOfDay >= 6f && timeOfDay < 18f)
            {
                globalLight.intensity = 0.6f;
                globalLight.color = new Color(0.1f, 0.3f, 0.5f);
            }
            else
            {
                globalLight.intensity = 0.3f;
                globalLight.color = new Color(0.1f, 0.2f, 0.3f);
            }
        }
        else
        {
            if (timeOfDay >= 6f && timeOfDay < 18f)
            {
                globalLight.intensity = 1f;
                globalLight.color = Color.white;
            }
            else
            {
                float lerpTime = timeOfDay < 6f
                    ? Mathf.InverseLerp(0f, 6f, timeOfDay)
                    : Mathf.InverseLerp(18f, 24f, timeOfDay);

                globalLight.intensity = Mathf.Lerp(0.5f, 0.2f, lerpTime);
                globalLight.color = Color.Lerp(Color.white, new Color(0.1f, 0.1f, 0.3f), lerpTime);
            }
        } 
    }

}
