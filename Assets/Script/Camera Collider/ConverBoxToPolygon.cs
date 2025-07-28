using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class ConvertBoxToPolygon : MonoBehaviour
{
    void Awake()
    {
        BoxCollider2D box = GetComponent<BoxCollider2D>();

        Vector2 size = box.size;
        Vector2 offset = box.offset;

        Vector2[] points = new Vector2[4];
        points[0] = offset + new Vector2(-size.x / 2, -size.y / 2);
        points[1] = offset + new Vector2(-size.x / 2, size.y / 2);
        points[2] = offset + new Vector2(size.x / 2, size.y / 2);
        points[3] = offset + new Vector2(size.x / 2, -size.y / 2);

        PolygonCollider2D poly = gameObject.AddComponent<PolygonCollider2D>();
        poly.isTrigger = true;
        poly.SetPath(0, points);

        //DestroyImmediate(box); 
    }
}
