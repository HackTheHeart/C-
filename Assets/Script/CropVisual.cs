using UnityEngine;
using System.Collections;

public class CropVisual : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Vector3 originalPosition;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalPosition = transform.localPosition;
        spriteRenderer.transform.localScale = Vector3.one * 1f;
    }
    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
        spriteRenderer.transform.localScale = Vector3.one * 1f;
    }
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }
    private IEnumerator ShakeCoroutine()
    {
        float duration = 0.1f; 
        float elapsedTime = 0f;
        float shakeAmount = 0.05f;
        while (elapsedTime < duration)
        {
            float offsetX = Random.Range(-shakeAmount, shakeAmount);
            transform.localPosition = originalPosition + new Vector3(offsetX, 0f, 0f);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPosition;
    }
}
