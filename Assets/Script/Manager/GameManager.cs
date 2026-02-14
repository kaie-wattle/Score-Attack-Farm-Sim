using UnityEditor.EditorTools;
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

    private SO_CropDefinition selectCropDefinition;

    public SO_CropDefinition SelectCropDefinition => selectCropDefinition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        dateManager.OnDateChenged += uiManager.UpdateDate;
        tileMapManager.OnPlanted += Planted;
        tileMapManager.OnHarvested += Harvested;
        moneyManager.OnMoneyChanged += uiManager.UpdateMoney;

        uiManager.Initialize(this);
        dateManager.Initialize();
        tileMapManager.Initialize();
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

    public void SetSelectedCrop(SO_CropDefinition _cropDef)
    {
        selectCropDefinition = _cropDef;
        Debug.Log("選択した作物：" + _cropDef.cropName);
    }

    /// <summary>
    /// 作物を植える
    /// </summary>
    /// <param name="cellPos">タイルの座標</param>
    void Planted(Vector3Int cellPos)
    {
        if(selectCropDefinition == null)
        {
            Debug.Log("作物未選択");
            return;
        }

        tileMapManager.Plant(cellPos, selectCropDefinition);
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
        ScoreContext context = new ScoreContext
        {
            Money = moneyManager.CurrentMoney,
            FieldCount = tileMapManager.GetFieldCount(),
            LivestockArea = 0, // 未実装
            IsNeverDebt = moneyManager.IsNeverDebt,
            IsCropOnly = true // 仮
        };
        ScoreResult score = scoreManager.CalcScore(context);
        uiManager.UpdateClearUI(score);
    }

    private void OnDestroy()
    {
        dateManager.OnDateChenged -= uiManager.UpdateDate;
        tileMapManager.OnPlanted -= Planted;
        tileMapManager.OnHarvested -= Harvested;
        moneyManager.OnMoneyChanged += uiManager.UpdateMoney;
    }
}
