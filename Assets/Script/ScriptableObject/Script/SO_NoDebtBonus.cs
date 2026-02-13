using UnityEngine;

/// <summary>
/// ボーナス：一度も赤字になっていない
/// </summary>
[CreateAssetMenu(menuName ="Farm/ScoreBonus/NoDebt")]
public class SO_NoDebtBonus : SO_ScoreBonus
{
    public override int CalcBonus(ScoreContext context)
    {
        return context.IsNeverDebt ? bonusValue : 0;
    }
}
