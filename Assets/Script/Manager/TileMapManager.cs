using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] Tilemap fieldMap;
    [SerializeField] Tile glassTile; // 未使用 拡張可能エリアとして使用予定
    [SerializeField] Tile groundTile;

    Camera mainCamera;
    Dictionary<Vector3Int, FieldCellData> fieldCells;
    Vector3Int StartPos = new Vector3Int(-7, -3, 0);

    /// <summary> 耕作イベント </summary>
    public event UnityAction<Vector3Int> OnPlanted;
    /// <summary> 収穫イベント </summary>
    public event UnityAction<int> OnHarvested;

    public void Initialize()
    {
        mainCamera = Camera.main;
        InitField();
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
        var cellPos = fieldMap.WorldToCell(mainCamera.ScreenToWorldPoint(pos));

        OnPlanted?.Invoke(cellPos);
    }

    /// <summary>
    /// タイル状況を更新
    /// </summary>
    public void UpdateTile()
    {
        foreach(var cell in fieldCells.Values)
        {
            if(cell.cropData == null)
            {
                continue;
            }

            // 全ての作物を1段階成長させる
            cell.cropData.growthStage++;
            if(cell.cropData.growthStage >= cell.cropData.so_CropDefinition.growMonths)
            {
                // 成長しきったら収穫する
                Debug.Log(cell.cropData.so_CropDefinition.sellPrice);
                OnHarvested?.Invoke(cell.cropData.so_CropDefinition.sellPrice);
                cell.cropData = null;
                fieldMap.SetTile(cell.cellPos, groundTile);
                Debug.Log("収穫しました。");
            }
        }
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

    void SetGroundTile()
    {
        int ColMax = 7; // デバッグ
        int currentCol = ResourceManager.Instance.FiledCount % ColMax;
        int currentRow = ResourceManager.Instance.FiledCount / ColMax;
        Vector3Int setPos = new Vector3Int(StartPos.x + currentCol, StartPos.y + currentRow, 0);
        fieldMap.SetTile(setPos, groundTile);
    }

    /// <summary>
    /// 耕地面積を取得
    /// </summary>
    /// <returns>耕地面積</returns>
    public int GetFieldCount()
    {
        return fieldCells.Count;
    }

    /// <summary>
    /// 作物が植えられている耕地面積を取得
    /// </summary>
    /// <returns>作物が植えられている耕地面積</returns>
    public int GetPlantedCount()
    {
        int ret = 0;
        foreach(var cell in fieldCells.Values)
        {
            if (cell.cropData != null)
                ret++;
        }
        return ret;
    }
}
