using UnityEngine;

public enum CropType
{
    None,
    Carrot,
}

[System.Serializable]
public class CropData
{
    public CropType cropType;
    public int growthStage;

    public CropData(CropType cropType)
    {
        this.cropType = cropType;
        growthStage = 0;
    }
}
