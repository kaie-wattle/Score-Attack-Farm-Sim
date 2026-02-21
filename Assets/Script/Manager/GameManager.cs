using System.Collections.Generic;
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
    [SerializeField] ShopUIManager shopUIManager;
    [SerializeField] int endYear;
    [SerializeField] int initializeMoney = 100000;
    [SerializeField] List<SO_CropDefinition> cropDefinitionList;

    private Dictionary<SO_CropDefinition, int> seedInventory = new Dictionary<SO_CropDefinition, int>();

    private SO_CropDefinition selectCropDefinition;

    public SO_CropDefinition SelectCropDefinition => selectCropDefinition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        // イベント設定
        dateManager.OnDateChenged += uiManager.UpdateDate;
        tileMapManager.OnPlanted += Planted;
        tileMapManager.OnHarvested += Harvested;
        moneyManager.OnMoneyChanged += uiManager.UpdateMoney;

        // 各Manager初期化
        uiManager.Initialize(this, cropDefinitionList);
        dateManager.Initialize();
        tileMapManager.Initialize();
        moneyManager.Initialize(initializeMoney);
        shopUIManager.Initialize(cropDefinitionList);

        foreach (var cropDef in cropDefinitionList)
        {
            // TODO:デバッグ用
            seedInventory[cropDef] = 5;
            uiManager.UpdateSeedCount(cropDef, seedInventory[cropDef]);
        }
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
    /// 選択中作物設定
    /// </summary>
    /// <param name="_cropDef"></param>
    public void SetSelectedCrop(SO_CropDefinition _cropDef)
    {
        selectCropDefinition = _cropDef;
        uiManager.UpdateSelectedCrop(selectCropDefinition);
        Debug.Log("選択した作物：" + selectCropDefinition.cropName);
    }

    /// <summary>
    /// 種子追加
    /// </summary>
    /// <param name="cropDef">作物情報</param>
    /// <param name="count">追加数</param>
    public void AddSeed(SO_CropDefinition cropDef,int count)
    {
        seedInventory[cropDef] += count;
        uiManager.UpdateSeedCount(cropDef, seedInventory[cropDef]);
    }

    /// <summary>
    /// 種子消費
    /// </summary>
    /// <param name="cropDef">作物情報</param>
    public void UseSeed(SO_CropDefinition cropDef)
    {
        if(seedInventory[cropDef] <= 0)
        {
            // 念のためここでも種子の残量確認をする
            SetSelectedCrop(null);
            return;
        }
        seedInventory[cropDef]--;
        uiManager.UpdateSeedCount(cropDef, seedInventory[cropDef]);
        if (seedInventory[cropDef] <= 0)
        {
            selectCropDefinition = null;
            SetSelectedCrop(null);
        }
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

        if(tileMapManager.Plant(cellPos, selectCropDefinition))
        {
            UseSeed(selectCropDefinition);
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
