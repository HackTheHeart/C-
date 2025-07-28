using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Threading.Tasks;

public class SceneFadeUI : MonoBehaviour
{
    public static SceneFadeUI Instance;
    [SerializeField] private Animator fadeAnimator;
    [SerializeField] private float fadeDuration = 1f;
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
    public async Task FadeOut()
    {
        fadeAnimator.SetTrigger("FadeOut");
        await Task.Delay(Mathf.RoundToInt(fadeDuration * 1000));
    }
    public async Task FadeIn()
    {
        fadeAnimator.SetTrigger("FadeIn");
        await Task.Delay(Mathf.RoundToInt(fadeDuration * 1000));
    }
    public async Task FadeTransition(System.Func<Task> duringFadeAction)
    {
        await FadeOut();
        if (duringFadeAction != null)
            await duringFadeAction();
        await FadeIn();
    }
}
