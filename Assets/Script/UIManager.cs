using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text yearText;
    [SerializeField] TMPro.TMP_Text monthText;
    [SerializeField] TMPro.TMP_Text moneyText;
    [SerializeField] TMPro.TMP_Text scoreText;
    [SerializeField] GameObject clearUI;

    public void Initialize()
    {
        scoreText.SetText("Score:" + 0);
        clearUI.SetActive(false);
    }

    /// <summary>
    /// 日付更新
    /// </summary>
    public void UpdateDate(int year,int month)
    {
        yearText.SetText(year + "年目");
        monthText.SetText(month + "月");
    }

    /// <summary>
    /// 所持金更新
    /// </summary>
    /// <param name="money">お金</param>
    public void UpdateMoney(int money)
    {
        moneyText.SetText(money.ToString());
    }

    /// <summary>
    /// クリア画面表示
    /// </summary>
    /// <param name="score">スコア</param>
    public void UpdateClearUI(int score)
    {
        clearUI.SetActive(true);
        scoreText.SetText("Score:" + score.ToString());
    }
}
