using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("----------Mamager----------")]
    [SerializeField] UIManager uiManager;
    [SerializeField] DateManager dateManager;
    [SerializeField] TileMapManager tileMapManager;
    [SerializeField] IncomeAndExpensesManager incomeAndExpensesManager;
    [SerializeField] MaintenanceManager maintenanceManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] ShopUIManager shopUIManager;
    [Header("---------------------------")]
    [SerializeField] int endYear;
    [SerializeField] int initializeMoney = 500;
    [SerializeField] int initializeFeed = 500;
    [SerializeField] List<SO_CropDefinition> cropDefinitionList;
    [SerializeField] List<SO_LivestockDefinition> livestockDefinitionList;
    [SerializeField] List<SO_LandDefinition> landDefinitionList;

    private SO_CropDefinition selectCropDefinition;

    public SO_CropDefinition SelectCropDefinition => selectCropDefinition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // イベント設定
        dateManager.OnDateChenged += uiManager.UpdateDate;
        tileMapManager.OnPlanted += Planted;
        tileMapManager.OnIncomeAdded += IncomeAdded;
        tileMapManager.OnTileChanged += TileMapChanged;
        maintenanceManager.OnExpensed += ExpenseAdded;
        ResourceManager.Instance.OnMoneyChanged += MoneyChanged;
        ResourceManager.Instance.OnFeedChanged += FeedChanged;
        ResourceManager.Instance.OnDirtinessChanged += DirtinessChanged;
        ResourceManager.Instance.OnSeedInventoryChanged += SeedChanged;
        ResourceManager.Instance.OnLivestockInventoryChanged += LivestockChanged;
        ResourceManager.Instance.OnFieldCountChanged += FieldChanged;
        ResourceManager.Instance.OnLivestockAreaCountChanged += LivestockAreaChanged;

        ResourceManager.Instance.AddMoney(initializeMoney);
        ResourceManager.Instance.AddFeed(initializeFeed);
        ResourceManager.Instance.AddDirtiness(0);

        // 各Manager初期化
        uiManager.Initialize(cropDefinitionList, livestockDefinitionList, SetSelectedCrop);
        dateManager.Initialize();
        tileMapManager.Initialize();
        shopUIManager.Initialize(cropDefinitionList, livestockDefinitionList, landDefinitionList, tileMapManager.GetFreeLivestockTile(), ExpenseAdded);
        incomeAndExpensesManager.Initialize();

        foreach (var cropDef in cropDefinitionList)
        {
            // TODO:デバッグ用
            ResourceManager.Instance.AddSeed(cropDef, 5);
            uiManager.UpdateSeedCount(cropDef);
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
        int allLandCount = ResourceManager.Instance.FieldCount+ResourceManager.Instance.LivestockAreaCount;
        int seedCount = ResourceManager.Instance.GetAllSeedCount();
        int cost = maintenanceManager.CalcCost(allLandCount, tileMapManager.GetPlantedCount(), seedCount);

        // 支払い
        ResourceManager.Instance.AddMoney(-cost);

        incomeAndExpensesManager.SaveReportData(dateManager.Year, dateManager.Month);

        if (IsGameClear())
        {
            GameClear();
        }
    }

    /// <summary>
    /// 収支画面表示ボタン押下処理
    /// </summary>
    public void OnIncomeAndExpensesViewButton()
    {
        incomeAndExpensesManager.ReportActive();
    }

    /// <summary>
    /// ショップ表示ボタン押下処理
    /// </summary>
    public void OnShopViewButton()
    {
        shopUIManager.ShopActive();
    }

    /// <summary>
    /// 選択中作物設定
    /// </summary>
    /// <param name="_cropDef"></param>
    public void SetSelectedCrop(SO_CropDefinition _cropDef)
    {
        selectCropDefinition = _cropDef;
        uiManager.UpdateSelectedCrop(selectCropDefinition);
        Debug.Log("選択した作物：" + (selectCropDefinition != null ? selectCropDefinition.cropName : "未選択"));
    }

    /// <summary>
    /// 種子消費
    /// </summary>
    /// <param name="cropDef">作物情報</param>
    public void UseSeed(SO_CropDefinition cropDef)
    {
        if (ResourceManager.Instance.GetSeedCount(cropDef) <= 0)
        {
            // 念のためここでも種子の残量確認をする
            SetSelectedCrop(null);
            return;
        }

        ResourceManager.Instance.AddSeed(cropDef, -1);
        uiManager.UpdateSeedCount(cropDef);
        if (ResourceManager.Instance.GetSeedCount(cropDef) <= 0)
        {
            selectCropDefinition = null;
            SetSelectedCrop(null);
        }
    }

    #region イベント
    /// <summary>
    /// タイル切り替え
    /// </summary>
    /// <param name="cellPos">タイルの座標</param>
    void TileMapChanged(LandType landType)
    {
        uiManager.ChangeItemSelectList(landType);
    }


    /// <summary>
    /// 作物を植える
    /// </summary>
    /// <param name="cellPos">タイルの座標</param>
    void Planted(Vector3Int cellPos)
    {
        if (selectCropDefinition == null)
        {
            Debug.Log("作物未選択");
            return;
        }

        if (tileMapManager.Plant(cellPos, selectCropDefinition))
        {
            UseSeed(selectCropDefinition);
        }
    }

    /// <summary>
    /// 収入獲得
    /// </summary>
    /// <param name="income">収入</param>
    void IncomeAdded(int income, IncomeType incomeType)
    {
        ResourceManager.Instance.AddMoney(income);
        incomeAndExpensesManager.SetIncomeData(income, incomeType);
    }

    /// <summary>
    /// 費用計上
    /// </summary>
    /// <param name="expense"></param>
    /// <param name="expenseType"></param>
    void ExpenseAdded(int expense, ExpenseType expenseType)
    {
        ResourceManager.Instance.AddMoney(-expense);
        incomeAndExpensesManager.SetExpenseData(expense, expenseType);
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
    /// 所持金更新
    /// </summary>
    void MoneyChanged()
    {
        uiManager.UpdateMoney();
        shopUIManager.UpdateMoney();
        //Debug.Log("お金変更");
    }

    /// <summary>
    /// 所持餌更新
    /// </summary>
    void FeedChanged()
    {
        uiManager.UpdateFeed();
        Debug.Log("餌変更");
    }

    /// <summary>
    /// 汚れ更新
    /// </summary>
    void DirtinessChanged()
    {
        uiManager.UpdateDirtiness();
        Debug.Log("汚れ変更");
    }

    /// <summary>
    /// 種子保有量更新
    /// </summary>
    /// <param name="cropDef">作物情報</param>
    void SeedChanged(SO_CropDefinition cropDef)
    {
        uiManager.UpdateSeedCount(cropDef);
    }

    /// <summary>
    /// 家畜保有量更新
    /// </summary>
    /// <param name="livestockDef">家畜情報</param>
    void LivestockChanged(SO_LivestockDefinition livestockDef,int value)
    {
        uiManager.UpdateLivestockCount(livestockDef);
        if(value > 0)
        {
            tileMapManager.SetLivestock(livestockDef, value);
        }
        shopUIManager.UpdateStock(tileMapManager.GetFreeLivestockTile());
        //デバッグ用
        ResourceManager.Instance.FreeLivestockAreaCount = tileMapManager.GetFreeLivestockTile();
    }

    /// <summary>
    /// 耕地面積更新
    /// </summary>
    void FieldChanged()
    {
        tileMapManager.SetGroundTile();
    }

    /// <summary>
    /// 畜産面積更新
    /// </summary>
    void LivestockAreaChanged()
    {
        tileMapManager.SetGlassTile();
        shopUIManager.UpdateStock(tileMapManager.GetFreeLivestockTile());
        //デバッグ用
        ResourceManager.Instance.FreeLivestockAreaCount = tileMapManager.GetFreeLivestockTile();
    }
    #endregion

    /// <summary>
    /// ゲームクリア
    /// </summary>
    void GameClear()
    {
        ScoreContext context = new ScoreContext
        {
            Money = ResourceManager.Instance.Money,
            FieldCount = ResourceManager.Instance.FieldCount,
            LivestockArea = ResourceManager.Instance.LivestockAreaCount,
            IsNeverDebt = ResourceManager.Instance.IsNeverDebt,
            IsCropOnly = true // 仮
        };
        ScoreResult score = scoreManager.CalcScore(context);
        uiManager.UpdateClearUI(score);
    }

    private void OnDestroy()
    {
        dateManager.OnDateChenged -= uiManager.UpdateDate;
        tileMapManager.OnPlanted -= Planted;
        tileMapManager.OnIncomeAdded -= IncomeAdded;
        tileMapManager.OnTileChanged -= TileMapChanged;
        maintenanceManager.OnExpensed -= ExpenseAdded;
        ResourceManager.Instance.OnMoneyChanged -= MoneyChanged;
        ResourceManager.Instance.OnFeedChanged -= FeedChanged;
        ResourceManager.Instance.OnDirtinessChanged -= DirtinessChanged;
        ResourceManager.Instance.OnSeedInventoryChanged -= SeedChanged;
        ResourceManager.Instance.OnLivestockInventoryChanged -= LivestockChanged;
        ResourceManager.Instance.OnFieldCountChanged -= FieldChanged;
        ResourceManager.Instance.OnLivestockAreaCountChanged -= LivestockAreaChanged;
    }
}
