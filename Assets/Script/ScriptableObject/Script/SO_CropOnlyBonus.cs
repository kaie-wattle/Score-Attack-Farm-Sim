using UnityEngine;

/// <summary>
/// ボーナス：耕作のみでクリア
/// </summary>
[CreateAssetMenu(menuName ="Farm/ScoreBonus/CropOnly")]
public class SO_CropOnlyBonus : SO_ScoreBonus
{
    public override int CalcBonus(ScoreContext context)
    {
        return context.IsCropOnly ? bonusValue : 0;
    }
}
