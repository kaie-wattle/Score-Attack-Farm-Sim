using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class TileMapManager : MonoBehaviour
{
    [SerializeField] Tilemap fieldMap;
    [SerializeField] Tile glassTile;
    [SerializeField] Tile groundTile;
    [SerializeField] Tile cropsTile;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

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
            return;
        }
        var pos = Input.mousePosition;
        pos.z = 0;
        var cellPos = fieldMap.WorldToCell(Camera.main.ScreenToWorldPoint(pos));
        if (fieldMap.HasTile(cellPos))
        {
            var tile = fieldMap.GetTile(cellPos);
            if (tile == groundTile)
            {
                fieldMap.SetTile(cellPos, cropsTile);
                Debug.Log("植えました");
            }
            else
            {
                Debug.Log("既に植えている");
            }
        }
        else
        {
            Debug.Log("タイルがない");
        }
    }

    /// <summary>
    /// タイル状況を更新
    /// </summary>
    public void UpdateTile()
    {
        var bounds = fieldMap.cellBounds;
        for (int x = bounds.xMin; x < bounds.xMax; x++)
        {
            for (int y = bounds.yMin; y < bounds.yMax; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                if (fieldMap.HasTile(cellPos))
                {
                    var tile = fieldMap.GetTile(cellPos);
                    if (tile == cropsTile)
                    {
                        fieldMap.SetTile(cellPos, groundTile);
                        Debug.Log("収穫しました");
                    }
                    else
                    {
                        Debug.Log("変化なし");
                    }
                }
            }
        }
    }
}
