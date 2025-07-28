using UnityEngine;

[CreateAssetMenu(fileName = "Scriptable Objects", menuName = "Inventory")]
public class ScriptableItems : ScriptableObject
{
    [SerializeField] private string itemName;
    [SerializeField] private bool itemStackable;
    [SerializeField] private Sprite itemIcon;
    public string ItemName => itemName;
    public bool ItemStackable => itemStackable;
    public Sprite ItemIcon => itemIcon;
}
