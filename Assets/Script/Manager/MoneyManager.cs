using UnityEngine;
using UnityEngine.Events;

public class MoneyManager : MonoBehaviour
{
    int money;
    bool isNeverDebt;

    public int CurrentMoney => money;
    public bool IsNeverDebt => isNeverDebt;

    public event UnityAction<int> OnMoneyChanged;

    public void Initialize(int startMoney)
    {
        money = startMoney;
        isNeverDebt = true;
        OnMoneyChanged?.Invoke(money);
    }

    public void AddMoney(int value)
    {
        money += value;

        if(money < 0)
            isNeverDebt = false;

        OnMoneyChanged?.Invoke(money);
    }
}
