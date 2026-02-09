using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] DateManager dateManager;
    [SerializeField] TMPro.TMP_Text moneyText;
    [SerializeField] TMPro.TMP_Text scoreText;

    int currentMoney;
    int endYear;

    public event UnityAction gameClear;

    public void Initialize(int endYear)
    {
        this.endYear = endYear;
        currentMoney = 10000;
        dateManager.Initialize(endYear);
        moneyText.SetText(currentMoney.ToString());
        scoreText.SetText("Score:" + 0);
    }

    /// <summary>
    /// 日付更新
    /// </summary>
    public void UpdateDate()
    {
        if (dateManager.AdvanceToNextTime())
        {
            gameClear?.Invoke();
        }
    }

    /// <summary>
    /// 所持金更新
    /// </summary>
    /// <param name="money"></param>
    public void UpdateMoney(int money)
    {

        currentMoney += money;
        moneyText.SetText(currentMoney.ToString());
    }

    public void UpdateClearUI(int score)
    {
        scoreText.SetText("Score:" + score.ToString());
    }
}
