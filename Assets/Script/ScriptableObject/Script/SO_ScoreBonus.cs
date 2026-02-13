using UnityEngine;

public abstract class SO_ScoreBonus : ScriptableObject
{
    public string bonusName;
    public int bonusValue;

    public abstract int CalcBonus(ScoreContext context);
}
