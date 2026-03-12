using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTileMap : MonoBehaviour
{
    [SerializeField] Tilemap fieldMap;
    [SerializeField] Tile groundTile;
    [SerializeField] int ColMax;
    [SerializeField] int RowMax;

    private Vector3Int StartPos = new Vector3Int(-7, -3, 0);
    private int currentTile = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetGroundTile()
    {
        int currentCol = currentTile % ColMax;
        int currentRow = currentTile / ColMax;
        Vector3Int setPos = new Vector3Int(StartPos.x + currentCol, StartPos.y + currentRow, 0);
        fieldMap.SetTile(setPos, groundTile);
    }

    public void OnSetTileButton()
    {
        if (currentTile >= ColMax * RowMax)
        {
            Debug.Log("‚à‚¤’u‚¯‚Ü‚¹‚ñ");
        }
        else
        {
            SetGroundTile();
            currentTile++;
        }
    }
}
