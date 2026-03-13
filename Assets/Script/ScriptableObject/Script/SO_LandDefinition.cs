using UnityEngine;
using UnityEngine.Tilemaps;

public enum LandType
{
    None,
    /// <summary> ”_’n </summary>
    Farmland,
    /// <summary> ’{ŽYƒGƒŠƒA </summary>
    LivestockArea,
}

/// <summary>
/// “y’n’è‹`
/// </summary>
[CreateAssetMenu(menuName = "Farm/Land")]
public class SO_LandDefinition : ScriptableObject
{
    /// <summary> “y’nƒ^ƒCƒv </summary>
    public LandType landType;
    /// <summary> “y’n–¼ </summary>
    public string landName;
    /// <summary> “y’n‰¿Ši </summary>
    public int sellPrice;
    /// <summary> ƒ^ƒCƒ‹ </summary>
    public TileBase landTile;
}
