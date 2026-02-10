using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] DateManager dateManager;
    [SerializeField] TileMapManager tileMapManager;
    [SerializeField] MoneyManager moneyManager;
    [SerializeField] MaintenanceManager maintenanceManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] int endYear;
    [SerializeField] int initializeMoney = 100000;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dateManager.OnDateChenged += uiManager.UpdateDate;
        tileMapManager.OnHarvested += Harvested;
        moneyManager.OnMoneyChanged += uiManager.UpdateMoney;

        uiManager.Initialize();
        dateManager.Initialize();
        moneyManager.Initialize(initializeMoney);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 次の月ボタン押下処理
    /// </summary>
    public void OnNextDateClick()
    {
        // 時間を進める
        dateManager.AdvanceMonth();
        
        // 収穫
        tileMapManager.UpdateTile();

        // 維持費計算
        int seedCount = 10; // 仮
        int cost = maintenanceManager.CalcCost(tileMapManager.GetFieldCount(), tileMapManager.GetPlantedCount(), seedCount);

        // 支払い
        moneyManager.AddMoney(-cost);

        if(IsGameClear())
        {
            GameClear();
        }
    }

    /// <summary>
    /// 収穫された
    /// </summary>
    /// <param name="income">収入</param>
    void Harvested(int income)
    {
        moneyManager.AddMoney(income);
    }

    /// <summary>
    /// ゲームクリア判定
    /// </summary>
    /// <returns>true:ゲームクリア false:未クリア</returns>
    bool IsGameClear()
    {
        return dateManager.Year >= endYear && dateManager.Month >= 4;
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    void GameClear()
    {
        int score = scoreManager.CalcScore(
            moneyManager.CurrentMoney,
            tileMapManager.GetFieldCount(),
            0,
            moneyManager.IsNeverDebt,
            true
            );
        uiManager.UpdateClearUI(score);
    }

    private void OnDestroy()
    {
        dateManager.OnDateChenged -= uiManager.UpdateDate;
        tileMapManager.OnHarvested -= Harvested;
        moneyManager.OnMoneyChanged += uiManager.UpdateMoney;
    }
}
