using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "New Diggable Tile", menuName = "Tiles/Diggable Tile")]
public class DiggableTileData : ScriptableObject
{
    public TileBase tile;
    public bool isDiggable;
    public TileBase afterDigTile;
}
