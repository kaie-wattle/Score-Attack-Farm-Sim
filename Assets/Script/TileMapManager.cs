using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] Tilemap fieldMap;
    [SerializeField] Tile glassTile; // 未使用 拡張可能エリアとして使用予定
    [SerializeField] Tile groundTile;
    [SerializeField] Tile cropsTile;

    Camera mainCamera;
    Dictionary<Vector3Int, FieldCellData> fieldCells;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCamera = Camera.main;
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClickTile();
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
        
        if(!fieldCells.TryGetValue(cellPos,out var cell))
        {
            // 存在しないタイルをクリックした場合は処理しない
            Debug.Log("タイルがない");
            return;
        }

        if(cell.cropData != null)
        {
            Debug.Log("既に植えている");
            return;
        }

        // 植える
        cell.cropData = new CropData(CropType.Carrot);
        fieldMap.SetTile(cellPos, cropsTile);
        Debug.Log("植えました");
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

            cell.cropData.growthStage++;
            if(cell.cropData.growthStage >= 1)
            {
                cell.cropData = null;
                fieldMap.SetTile(cell.cellPos, groundTile);
                Debug.Log("収穫しました。");
            }
        }
    }
}
