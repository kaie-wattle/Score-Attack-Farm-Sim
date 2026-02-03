using UnityEngine;

public class DateManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text yearText;
    [SerializeField] TMPro.TMP_Text monthText;

    int year;
    int month;
    int endYear;


    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="endYear">ゲーム終了年</param>
    public void Initialize(int endYear)
    {
        year = 1;
        month = 4;
        this.endYear = endYear;
        yearText.SetText(year + "年目");
        monthText.SetText(month + "月");
    }

    /// <summary>
    /// 日付を次の月に進める
    /// </summary>
    /// <returns>終了年に到達したか</returns>
    public bool AdvanceToNextTime()
    {
        bool ret = false;
        month++;
        if(month == 13)
        {
            month = 1;
            year++;
        }
        if(year >= endYear && month >= 4)
        {
            ret = true;
        }
        yearText.SetText(year + "年目");
        monthText.SetText(month + "月");
        return ret;
    }
}
