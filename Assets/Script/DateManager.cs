using System;
using UnityEngine;
using UnityEngine.Events;

public class DateManager : MonoBehaviour
{
    int year;
    int month;

    public int Year => year;
    public int Month => month;

    public event UnityAction<int, int> OnDateChenged;
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="endYear">ゲーム終了年</param>
    public void Initialize()
    {
        year = 1;
        month = 4;
        OnDateChenged?.Invoke(year, month);
    }

    /// <summary>
    /// 日付を次の月に進める
    /// </summary>
    public void AdvanceMonth()
    {
        month++;
        if(month > 12)
        {
            month = 1;
            year++;
        }
        OnDateChenged?.Invoke(year, month);
    }
}
