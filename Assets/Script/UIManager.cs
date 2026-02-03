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

    public void UpdateUI(int money)
    {

        currentMoney += money;
        moneyText.SetText(currentMoney.ToString());
        if (dateManager.AdvanceToNextTime())
        {
            gameClear?.Invoke();
        }
    }

    public void UpdateClearUI(int score)
    {
        scoreText.SetText("Score:" + score.ToString());
    }
}
