using UnityEngine;

/// <summary>
/// ボーナス：土地を最大まで取得
/// </summary>
[CreateAssetMenu(menuName = "Farm/ScoreBonus/LandMax")]
public class SO_LandMaxBonus : SO_ScoreBonus
{
    public override int CalcBonus(ScoreContext context)
    {
        return context.IsLandMax ? bonusValue : 0;
    }
}
