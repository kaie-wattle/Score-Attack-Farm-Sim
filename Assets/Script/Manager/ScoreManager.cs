using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// スコア管理クラス
/// スコアは以下で考える
/// 所持金+耕地面積+畜産面積+ボーナス
/// </summary>
public class ScoreManager : MonoBehaviour
{
    [SerializeField] SO_ScoreDefinition scoreDefinition;
    [SerializeField] List<SO_ScoreBonus> scoreBonusList;
    public ScoreResult CalcScore(ScoreContext context)
    {
        ScoreResult result = new ScoreResult();

        // 基本スコア
        result.BaseScore += context.Money;
        result.BaseScoreDetails.Add(new ScoreDetail("資金:", context.Money));
        result.BaseScore += context.FieldCount * scoreDefinition.fieldValue;
        result.BaseScoreDetails.Add(new ScoreDetail("耕地面積:", context.FieldCount * scoreDefinition.fieldValue));
        result.BaseScore += context.LivestockArea * scoreDefinition.livestockValue;
        result.BaseScoreDetails.Add(new ScoreDetail("畜産面積:", context.LivestockArea * scoreDefinition.livestockValue));

        foreach (var scoreBonus in scoreBonusList)
        {
            int bonus = scoreBonus.CalcBonus(context);
            if(bonus > 0)
            {
                result.BonusScoreDetails.Add(new ScoreDetail(scoreBonus.bonusName, bonus));
                result.BonusScore += bonus;
            }
        }

        result.TotalScore = result.BaseScore + result.BonusScore;

        return result;
    }
}
