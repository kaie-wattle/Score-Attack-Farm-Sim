using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] Tilemap fieldMap;
    [SerializeField] Tilemap livestockMap;
    [SerializeField] Tile glassTile;
    [SerializeField] Tile groundTile;
    [SerializeField] int cleaningCost;
    [SerializeField] TMPro.TMP_Text sellFlagText;

    Camera mainCamera;
    Dictionary<Vector3Int, FieldCellData> fieldCells;
    Dictionary<Vector3Int, LivestockAreaCellData> livestockCells;
    Vector3Int StartPos = new Vector3Int(-7, -4, 0);
    int MinCleaningCost = 50;
    bool sellFlag = false;

    /// <summary> 耕作イベント </summary>
    public event UnityAction<Vector3Int> OnPlanted;
    /// <summary> 収入獲得イベント </summary>
    public event UnityAction<int, IncomeType> OnIncomeAdded;
    /// <summary> 支出イベント </summary>
    public event UnityAction<int, ExpenseType> OnExpensed;
    /// <summary> タイル切り替えイベント </summary>
    public event UnityAction<LandType> OnTileChanged;

    public void OnChangeFieldTileButton() => OnChangeTileMap(LandType.Farmland);
    public void OnChangeLivestockTileButton() => OnChangeTileMap(LandType.LivestockArea);

    public void Initialize()
    {
        mainCamera = Camera.main;
        InitField();
        InitLivestockArea();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickTile();
        }
    }

    /// <summary>
    /// 作物情報初期化
    /// </summary>
    void InitField()
    {
        // 各セルの作物情報を初期化する。
        fieldCells = new Dictionary<Vector3Int, FieldCellData>();
        var bounds = fieldMap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (fieldMap.HasTile(cellPos))
                {
                    fieldCells[cellPos] = new FieldCellData(cellPos);
                }
            }
        }
    }

    /// <summary>
    /// 畜産情報初期化
    /// </summary>
    void InitLivestockArea()
    {
        // 各セルの作物情報を初期化する。
        livestockCells = new Dictionary<Vector3Int, LivestockAreaCellData>();
        var bounds = livestockMap.cellBounds;

        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (livestockMap.HasTile(cellPos))
                {
                    livestockCells[cellPos] = new LivestockAreaCellData(cellPos);
                }
            }
        }
    }

    /// <summary>
    /// タイルクリック処理
    /// </summary>
    void ClickTile()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            // UIをクリックした場合は処理しない
            return;
        }
        var pos = Input.mousePosition;
        pos.z = 0;
        if (fieldMap.gameObject.activeInHierarchy)
        {
            ClickField(pos);
        }
        else if (livestockMap.gameObject.activeInHierarchy)
        {
            ClickLivestockArea(pos);
        }
    }

    /// <summary>
    /// 畑クリック処理
    /// </summary>
    /// <param name="pos"></param>
    void ClickField(Vector3 pos)
    {
        var cellPos = fieldMap.WorldToCell(mainCamera.ScreenToWorldPoint(pos));

        OnPlanted?.Invoke(cellPos);
    }

    /// <summary>
    /// 畜産エリアクリック処理
    /// </summary>
    /// <param name="pos"></param>
    void ClickLivestockArea(Vector3 pos)
    {
        var cellPos = livestockMap.WorldToCell(mainCamera.ScreenToWorldPoint(pos));
        if (!livestockCells.TryGetValue(cellPos, out var cell))
        {
            // 存在しないタイルをクリックした場合は処理しない
            Debug.Log("タイルがない");
            return;
        }
        if (cell.livestockData == null)
        {
            Debug.Log("タイル未設定");
            return;
        }

        SO_LivestockDefinition livestockDefinition = cell.livestockData.so_LivestockDefinition;
        int sellPrice;
        Debug.Log(livestockDefinition.livestockName);

        if (sellFlag)
        {
            if (livestockDefinition.growMonths != 0)
            {
                sellPrice = livestockDefinition.livestockPrice;
            }
            else
            {
                sellPrice = livestockDefinition.sellPrice;
            }
            string message = string.Format("{0}を売却します。よろしいですか？\n値段:{1}", livestockDefinition.livestockName,sellPrice);
            ConfirmPopup.instance.Show(message, () =>
            {
                OnIncomeAdded?.Invoke(sellPrice, IncomeType.Livestock);
                cell.livestockData = null;
                livestockMap.SetTile(cell.cellPos, glassTile);
                ResourceManager.Instance.AddLivestock(livestockDefinition, -1);
            });
        }
    }

    /// <summary>
    /// タイルマップ切り替え
    /// </summary>
    /// <param name="landType">タイルマップ種別</param>
    void OnChangeTileMap(LandType landType)
    {
        switch (landType)
        {
            case LandType.Farmland:
                fieldMap.gameObject.SetActive(true);
                livestockMap.gameObject.SetActive(false);
                OnTileChanged?.Invoke(landType);
                break;
            case LandType.LivestockArea:
                fieldMap.gameObject.SetActive(false);
                livestockMap.gameObject.SetActive(true);
                OnTileChanged?.Invoke(landType);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// 農地タイル更新
    /// </summary>
    void UpdateFieldTile()
    {
        foreach (var cell in fieldCells.Values)
        {
            if (cell.cropData == null)
            {
                continue;
            }

            // 全ての作物を1段階成長させる
            cell.cropData.growthStage++;
            if (cell.cropData.growthStage >= cell.cropData.so_CropDefinition.growMonths)
            {
                // 成長しきったら収穫する
                Debug.Log(cell.cropData.so_CropDefinition.sellPrice);
                OnIncomeAdded?.Invoke(cell.cropData.so_CropDefinition.sellPrice, IncomeType.Crop);
                cell.cropData = null;
                fieldMap.SetTile(cell.cellPos, groundTile);
                Debug.Log("収穫しました。");
            }
        }
    }

    /// <summary>
    /// 畜産タイル更新
    /// </summary>
    void UpdateLivestockTile()
    {
        int totalDirtiness = 0;
        foreach (var cell in livestockCells.Values)
        {
            if (cell.livestockData == null)
            {
                continue;
            }
            SO_LivestockDefinition definition = cell.livestockData.so_LivestockDefinition;
            int dirtiness = Random.Range(1, 5);
            totalDirtiness += dirtiness;

            // 餌がない場合は家畜が餓死する
            if (ResourceManager.Instance.Feed <= 0)
            {
                Debug.Log("餌が無かったので家畜が餓死しました。");
                cell.livestockData = null;
                livestockMap.SetTile(cell.cellPos, glassTile);
                ResourceManager.Instance.AddLivestock(definition, -1);
                totalDirtiness += 1000;
                continue;
            }

            int feed = definition.feedConsumption;
            if(ResourceManager.Instance.Feed >= feed)
            {
                ResourceManager.Instance.AddFeed(-definition.feedConsumption);
            }
            else
            {
                ResourceManager.Instance.AddFeed(-ResourceManager.Instance.Feed);
                Debug.Log("餌が足りませんでした。");
                continue;
            }

            // 成長する家畜の場合、成長させる。
            if (cell.livestockData.so_LivestockDefinition.growMonths != 0)
            {
                cell.livestockData.growthStage++;
                if (cell.livestockData.growthStage >= cell.livestockData.so_LivestockDefinition.growMonths)
                {
                    // 成長しきったら売却する
                    OnIncomeAdded?.Invoke(cell.livestockData.so_LivestockDefinition.sellPrice, IncomeType.Livestock);
                    cell.livestockData = null;
                    livestockMap.SetTile(cell.cellPos, glassTile);
                    ResourceManager.Instance.AddLivestock(definition, -1);
                    Debug.Log("家畜を売却しました。:" + definition.sellPrice);
                }
            }
            else
            {
                // 収入計算
                int rand = Random.Range(-20, 50);
                int dirtinessPenalty = (ResourceManager.Instance.Dirtiness / 50);
                float incomeRate = (100 + rand - dirtinessPenalty) / 100f;
                incomeRate = Mathf.Max(incomeRate, 0.1f);
                int income = (int)(cell.livestockData.so_LivestockDefinition.animalProductPrice * incomeRate);
                Debug.Log("今回の割合:" + incomeRate);
                // 家畜の生成物獲得
                OnIncomeAdded?.Invoke(income, IncomeType.Livestock);
                Debug.Log("家畜の生成物を獲得しました。:" + income);
            }

        }
        Debug.Log("汚れ合計:" + totalDirtiness);
        ResourceManager.Instance.AddDirtiness(totalDirtiness);
    }

    /// <summary>
    /// タイル状況を更新
    /// </summary>
    public void UpdateTile()
    {
        UpdateFieldTile();
        UpdateLivestockTile();
    }

    /// <summary>
    /// 作物を植える
    /// </summary>
    /// <param name="cellPos">タイルの座標</param>
    /// <param name="SelectCrop">選択されている作物</param>
    /// <returns>植えることができたか</returns>
    public bool Plant(Vector3Int cellPos, SO_CropDefinition SelectCrop)
    {
        if (!fieldCells.TryGetValue(cellPos, out var cell))
        {
            // 存在しないタイルをクリックした場合は処理しない
            Debug.Log("タイルがない");
            return false;
        }

        if (cell.cropData != null)
        {
            Debug.Log("既に植えている");
            return false;
        }

        // 植える
        cell.cropData = new CropData(SelectCrop);
        fieldMap.SetTile(cellPos, SelectCrop.cropTile);
        Debug.Log("植えました");
        return true;
    }

    /// <summary>
    /// 家畜を配置する
    /// </summary>
    /// <param name="livestockDef">タイルの座標</param>
    /// <param name="value">選択されている作物</param>
    public void SetLivestock(SO_LivestockDefinition livestockDef, int value)
    {
        int count = 0;
        foreach (var cell in livestockCells.Values)
        {
            if (cell.livestockData == null)
            {
                cell.livestockData = new LivestockData(livestockDef);
                livestockMap.SetTile(cell.cellPos, livestockDef.livestockTile);
                count++;
                if (value <= count)
                {
                    Debug.Log("配置完了");
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 農地拡張
    /// </summary>
    public void SetGroundTile()
    {
        int ColMax = 7; // デバッグ
        int currentCol = ResourceManager.Instance.FieldCount % ColMax;
        int currentRow = ResourceManager.Instance.FieldCount / ColMax;
        Vector3Int setPos = new Vector3Int(StartPos.x + currentCol, StartPos.y + currentRow, 0);
        fieldMap.SetTile(setPos, groundTile);
        fieldCells[setPos] = new FieldCellData(setPos);
    }

    /// <summary>
    /// 畜産面積拡張
    /// </summary>
    public void SetGlassTile()
    {
        int ColMax = 7; // デバッグ
        int currentCol = ResourceManager.Instance.LivestockAreaCount % ColMax;
        int currentRow = ResourceManager.Instance.LivestockAreaCount / ColMax;
        Vector3Int setPos = new Vector3Int(StartPos.x + currentCol, StartPos.y + currentRow, 0);
        livestockMap.SetTile(setPos, glassTile);
        livestockCells[setPos] = new LivestockAreaCellData(setPos);
    }

    /// <summary>
    /// 空き畜産面積取得
    /// </summary>
    public int GetFreeLivestockTile()
    {
        int count = 0;
        foreach (var cell in livestockCells.Values)
        {
            if (cell.livestockData == null)
            {
                count++;
            }
        }

        return count;
    }


    /// <summary>
    /// 作物が植えられている耕地面積を取得
    /// </summary>
    /// <returns>作物が植えられている耕地面積</returns>
    public int GetPlantedCount()
    {
        int ret = 0;
        foreach (var cell in fieldCells.Values)
        {
            if (cell.cropData != null)
                ret++;
        }
        return ret;
    }

    public void OnSellLivestock()
    {
        sellFlag = !sellFlag;
        if (sellFlag)
        {
            Debug.Log("売却フラグON");
            sellFlagText.SetText("売却選択中");
        }
        else
        {
            Debug.Log("売却フラグOFF");
            sellFlagText.SetText("");
        }
    }

    public void OnCleaning()
    {
        int cost = (cleaningCost * ResourceManager.Instance.Dirtiness) + MinCleaningCost;
        OnExpensed?.Invoke(cost, ExpenseType.Land);
        ResourceManager.Instance.AddDirtiness(-ResourceManager.Instance.Dirtiness);
    }
}
