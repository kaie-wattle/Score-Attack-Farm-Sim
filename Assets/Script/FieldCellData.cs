using UnityEngine;

[System.Serializable]
public class FieldCellData
{
    public bool isPlowed; // çkÇ≥ÇÍÇƒÇ¢ÇÈÇ©
    public CropData cropData;
    public Vector3Int cellPos;

    public FieldCellData(Vector3Int pos)
    {
        cellPos = pos;
        isPlowed = true;
        cropData = null;
    }
}
