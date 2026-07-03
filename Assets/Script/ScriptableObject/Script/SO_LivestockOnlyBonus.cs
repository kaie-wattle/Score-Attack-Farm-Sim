using UnityEngine;

/// <summary>
/// ボーナス：畜産のみでクリア
/// </summary>
[CreateAssetMenu(menuName = "Farm/ScoreBonus/LivestockOnly")]
public class SO_LivestockOnlyBonus : SO_ScoreBonus
{
    public override int CalcBonus(ScoreContext context)
    {
        return context.IsLivestockOnly ? bonusValue : 0;
    }
}
