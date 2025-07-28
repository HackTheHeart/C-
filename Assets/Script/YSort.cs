using UnityEngine;

public class YSort : MonoBehaviour
{
    public SpriteRenderer targetRenderer;
    void LateUpdate()
    {
        if (targetRenderer != null)
        {
            targetRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -100);
        }
    }
}
