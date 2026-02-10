using UnityEngine;

[CreateAssetMenu(menuName = "Farm/ScoreDefinition")]
public class SO_ScoreDefinition : ScriptableObject
{
    /// <summary> 耕地面積当たりの点数 </summary>
    public int fieldValue;
    /// <summary> 畜産面積当たりの点数 </summary>
    public int livestockValue;

    // 特別ボーナス
    /// <summary> 一度も赤字になっていない </summary>
    public int noDebtBonus;
    /// <summary> 耕作のみ </summary>
    public int cropOnlyBonus;
}
