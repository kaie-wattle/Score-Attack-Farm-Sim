using UnityEngine;

/// <summary>
/// スコア管理クラス
/// スコアは以下で考える
/// 所持金+耕地面積+畜産面積+ボーナス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] SO_ScoreDefinition scoreDefinition;
    public int CalcScore(int money,int fieldCount,int livestockArea,bool noDebtBonus,bool cropOnlyBonus)
    {
        int score = 0;

        // 基本スコア
        score += money;
        score += fieldCount * scoreDefinition.fieldValue;
        score += livestockArea * scoreDefinition.livestockValue;

        // ボーナス
        if (noDebtBonus)
            score += scoreDefinition.noDebtBonus;

        if(cropOnlyBonus)
            score += scoreDefinition.cropOnlyBonus;

        return score;
    }
}
