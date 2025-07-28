using UnityEngine;

public class FencePlacer : MonoBehaviour
{
    public GameObject fencePrefab;
    public LayerMask fenceLayer;
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Vector3 placePosition = GetMouseWorldPosition();
            placePosition = SnapToGrid(placePosition);
            if (!IsFenceAtPosition(placePosition)) 
            {
                GameObject newFence = Instantiate(fencePrefab, placePosition, Quaternion.identity);
                newFence.GetComponent<Fence>().UpdateFenceConnections();
                UpdateAdjacentFences(placePosition);
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
    bool IsFenceAtPosition(Vector3 position)
    {
        Collider2D hit = Physics2D.OverlapPoint(position, fenceLayer);
        return hit != null;
    }
    void UpdateAdjacentFences(Vector3 position)
    {
        Vector3[] directions = { Vector3.left, Vector3.right, Vector3.up, Vector3.down };
        foreach (var dir in directions)
        {
            Collider2D neighbor = Physics2D.OverlapPoint(position + dir, fenceLayer);
            if (neighbor != null)
            {
                neighbor.GetComponent<Fence>().UpdateFenceConnections();
            }
        }
    }
}
