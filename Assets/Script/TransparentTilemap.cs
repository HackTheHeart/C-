using UnityEngine;
using UnityEngine.Tilemaps;

public class TransparentTilemap : MonoBehaviour
{
    [SerializeField]private Tilemap tilemap;
    void Start()
    {
        tilemap = GetComponent<Tilemap>();
        MakeTilesTransparent();
    }
    void MakeTilesTransparent()
    {
        tilemap.color = new Color(1, 1, 1, 0f);
    }
}
