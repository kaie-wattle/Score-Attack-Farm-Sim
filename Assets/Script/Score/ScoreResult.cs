using System.Collections.Generic;

public class ScoreResult
{
    public int TotalScore;
    public int BaseScore;
    public int BonusScore;

    public List<ScoreDetail> BaseScoreDetails = new List<ScoreDetail>();
    public List<ScoreDetail> BonusScoreDetails = new List<ScoreDetail>();
}

public class ScoreDetail
{
    public string Name;
    public int Score;

    public ScoreDetail(string name, int score)
    {
        Name = name;
        Score = score;
    }
}