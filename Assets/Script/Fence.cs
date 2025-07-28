using UnityEngine;

public class Fence : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Sprite single, horizontal, vertical, cross, corner;
    public LayerMask fenceLayer;
    public void UpdateFenceConnections()
    {
        bool left = IsFenceAt(transform.position + Vector3.left);
        bool right = IsFenceAt(transform.position + Vector3.right);
        bool up = IsFenceAt(transform.position + Vector3.up);
        bool down = IsFenceAt(transform.position + Vector3.down);
        if (left && right && up && down) spriteRenderer.sprite = cross;
        else if (left && right) spriteRenderer.sprite = horizontal;
        else if (up && down) spriteRenderer.sprite = vertical;
        else if ((left && up) || (right && down) || (left && down) || (right && up)) spriteRenderer.sprite = corner;
        else spriteRenderer.sprite = single;
    }
    bool IsFenceAt(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position, fenceLayer);
        return hit != null;
    }
}
