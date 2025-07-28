using UnityEngine;

public class SeasonalTree : MonoBehaviour
{
    [SerializeField] private Sprite springSprite;
    [SerializeField] private Sprite summerSprite;
    [SerializeField] private Sprite fallSprite;
    [SerializeField] private Sprite winterSprite;
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null) { Debug.Log("spriteRenderer is null"); }
        spriteRenderer = GetComponent<SpriteRenderer>();
        ApplySeason();
    }
    private void ApplySeason()
    {
        if (spriteRenderer == null) return;
        int season = GameTimeManager.Instance.Season;
        switch (season)
        {
            case 0: spriteRenderer.sprite = springSprite; break;
            case 1: spriteRenderer.sprite = summerSprite; break;
            case 2: spriteRenderer.sprite = fallSprite; break;
            case 3: spriteRenderer.sprite = winterSprite; break;
        }
    }
}
