using UnityEngine;

public class FenceRemover : MonoBehaviour
{
    public LayerMask fenceLayer;
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 clickPosition = GetMouseWorldPosition();
            clickPosition = SnapToGrid(clickPosition);

            Collider2D fence = Physics2D.OverlapPoint(clickPosition, fenceLayer);
            if (fence != null)
            {
                Destroy(fence.gameObject);
            }
        }
    }
    Vector3 GetMouseWorldPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        return Camera.main.ScreenToWorldPoint(mousePos);
    }
    Vector3 SnapToGrid(Vector3 pos)
    {
        return new Vector3(Mathf.Round(pos.x), Mathf.Round(pos.y), 0);
    }
}
