using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// ‰Æ’{’è‹`
/// </summary>
[CreateAssetMenu(menuName = "Farm/Livestock")]
public class SO_LivestockDefinition : ScriptableObject
{
    /// <summary> ‰Æ’{ƒ^ƒCƒv </summary>
    public LivestockType livestocklType;
    /// <summary> ‰Æ’{–¼ </summary>
    public string livestockName;
    /// <summary> ‰Æ’{‰¿Ši </summary>
    public int livestockPrice;
    /// <summary> ¬’·ŠúŠÔ 1ˆÈã‚ªİ’è‚³‚ê‚é‚ÆŠúŠÔ“’BŒã©“®”„‹p</summary>
    public int growMonths;
    /// <summary> ¶¬•¨‚Ì’l’i </summary>
    public int animalProductPrice;
    /// <summary> ”„‹pŠz </summary>
    public int sellPrice;
    /// <summary> ‰aÁ”ï—Ê </summary>
    public int feedConsumption;
    /// <summary> ©“®”„‹pƒtƒ‰ƒO ¬’·ŠúŠÔ‚ªİ’è‚³‚ê‚Ä‚¢‚é‚à‚Ì‚Ítrue‚Éİ’è‚·‚é </summary>
    public bool isAutoSold;
    /// <summary> ƒ^ƒCƒ‹ </summary>
    public TileBase livestockTile;
}
