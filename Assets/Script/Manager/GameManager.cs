using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] DateManager dateManager;
    [SerializeField] TileMapManager tileMapManager;
    [SerializeField] MaintenanceManager maintenanceManager;
    [SerializeField] ScoreManager scoreManager;
    [SerializeField] ShopUIManager shopUIManager;
    [SerializeField] int endYear;
    [SerializeField] int initializeMoney = 500;
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
        tileMapManager.OnHarvested += Harvested;
        tileMapManager.OnTileChanged += TileMapChanged;
        ResourceManager.Instance.OnMoneyChanged += MoneyChanged;
        ResourceManager.Instance.OnSeedInventoryChanged += SeedChanged;
        ResourceManager.Instance.OnFieldCountChanged += FieldChanged;
        ResourceManager.Instance.OnLivestockAreaCountChanged += LivestockAreaChanged;

        ResourceManager.Instance.AddMoney(initializeMoney);

        // 各Manager初期化
        uiManager.Initialize(cropDefinitionList, livestockDefinitionList, SetSelectedCrop);
        dateManager.Initialize();
        tileMapManager.Initialize();
        shopUIManager.Initialize(cropDefinitionList, landDefinitionList);

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
        int seedCount = 10; // 仮
        int cost = maintenanceManager.CalcCost(tileMapManager.GetFieldCount(), tileMapManager.GetPlantedCount(), seedCount);

        // 支払い
        ResourceManager.Instance.AddMoney(-cost);

        if (IsGameClear())
        {
            GameClear();
        }
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
    /// 収穫された
    /// </summary>
    /// <param name="income">収入</param>
    void Harvested(int income)
    {
        ResourceManager.Instance.AddMoney(income);
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
        Debug.Log("お金変更");
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
            FieldCount = tileMapManager.GetFieldCount(),
            LivestockArea = 0, // 未実装
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
        tileMapManager.OnHarvested -= Harvested;
        tileMapManager.OnTileChanged -= TileMapChanged;
        ResourceManager.Instance.OnMoneyChanged -= MoneyChanged;
        ResourceManager.Instance.OnSeedInventoryChanged -= SeedChanged;
        ResourceManager.Instance.OnFieldCountChanged -= FieldChanged;
        ResourceManager.Instance.OnLivestockAreaCountChanged -= LivestockAreaChanged;
    }
}
