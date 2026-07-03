
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameEventManager
{
    public struct RandomEventData
    {
        public int minRange;
        public UnityAction action;
    }

    public event UnityAction<int, IncomeType> OnIncomeAdded;
    public event UnityAction OnBumperCrop;
    public event UnityAction OnPlague;

    List<RandomEventData> eventDataList;
    bool hasLivestock = false;
    int dirtinessRate = 100;
    int basePlagueRate = 10;

    public GameEventManager()
    {
        eventDataList = new List<RandomEventData>
        {
            new RandomEventData{ minRange = 98,action = () => OnGameEventGetLottery() },
            new RandomEventData{ minRange = 70,action = () => OnGameEventBumperCrop() },
            new RandomEventData{ minRange = 10,action = () => OnGameEventPlague() }
        };
    }

    public void EventCheck(bool _hsLivestock)
    {
        hasLivestock = _hsLivestock;
        int rand = Random.Range(0, 100);
        Debug.Log("イベントチェック");
        foreach (var eventData in eventDataList)
        {
            if (rand >= eventData.minRange)
            {
                Debug.Log("イベント開始");
                eventData.action?.Invoke();
                return;
            }
        }
    }

    void OnGameEventGetLottery()
    {
        string message;
        message = "宝くじが当たった";
        NoticePopup.instance.Show(message);
        OnIncomeAdded?.Invoke(50000, IncomeType.Bonus);
    }

    void OnGameEventBumperCrop()
    {
        string message;
        message = "今期は豊作の予感";
        NoticePopup.instance.Show(message);
        OnBumperCrop?.Invoke();
    }

    void OnGameEventPlague()
    {
        // 家畜保有時のみイベント発生
        if (hasLivestock)
        {
            int dirtinessPoint = ResourceManager.Instance.Dirtiness / dirtinessRate;
            int rand = Random.Range(0, 100);
            int rate = dirtinessPoint + basePlagueRate;
            if(rate >= rand)
            {
                string message;
                message = "疫病が流行し家畜が死んでしまった";
                NoticePopup.instance.Show(message);
                OnPlague?.Invoke();
            }
        }
    }
}
