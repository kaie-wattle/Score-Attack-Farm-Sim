using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// 作物定義
/// </summary>
[CreateAssetMenu(menuName = "Farm/Crop")]
public class SO_CropDefinition : ScriptableObject
{
    /// <summary> 作物タイプ </summary>
    public CropType cropType;
    /// <summary> 成長期間 </summary>
    public int growMonths;
    /// <summary> 売却額 </summary>
    public int sellPrice;
    /// <summary> タイル </summary>
    public TileBase cropTile;
}
