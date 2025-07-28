using UnityEngine;

[System.Serializable]
public class WarpTile
{
    public string fromScene;            
    public Vector2Int tilePosition;     
    public string toScene;              
    public Vector2 targetPosition;      
}
