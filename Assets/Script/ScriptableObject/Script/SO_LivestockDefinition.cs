using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ‰Æ’{’è‹`
/// </summary>
[CreateAssetMenu(menuName = "Farm/Livestock")]
public class SO_LivestockDefinition : ScriptableObject
{
    /// <summary> ‰Æ’{ƒ^ƒCƒv </summary>
    public LivestockType cropType;
    /// <summary> ‰Æ’{–¼ </summary>
    public string cropName;
    /// <summary> ‰Æ’{‰¿Ši </summary>
    public string cropPrice;
    /// <summary> ¬’·ŠúŠÔ </summary>
    public int growMonths;
    /// <summary> ”„‹pŠz </summary>
    public int sellPrice;
    /// <summary> ƒ^ƒCƒ‹ </summary>
    public TileBase livestockTile;
}
