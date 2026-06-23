using UnityEngine;

[System.Serializable]
public class LivestockAreaCellData
{
    public LivestockData livestockData;
    public Vector3Int cellPos;

    public LivestockAreaCellData(Vector3Int pos)
    {
        cellPos = pos;
        livestockData = null;
    }
}
