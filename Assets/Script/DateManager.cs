using System;
using UnityEngine;
using UnityEngine.Events;

public class DateManager : MonoBehaviour
{
    int year;
    int month;
    int endYear;

    public event UnityAction<int, int> OnDateChenged;
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="endYear">ゲーム終了年</param>
    public void Initialize(int endYear)
    {
        year = 1;
        month = 4;
        this.endYear = endYear;
        OnDateChenged?.Invoke(year, month);
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
        OnDateChenged?.Invoke(year, month);
        return ret;
    }
}
