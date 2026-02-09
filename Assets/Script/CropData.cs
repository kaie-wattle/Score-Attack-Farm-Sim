using UnityEngine;

public enum CropType
{
    None,
    Rice,
    Wheat
}

/// <summary>
/// çÏï®èÓïÒ
/// </summary>
[System.Serializable]
public class CropData
{
    public SO_CropDefinition so_CropDefinition;
    /// <summary> ê¨í∑íiäK </summary>
    public int growthStage;

    public CropData(SO_CropDefinition cropDefinition)
    {
        so_CropDefinition = cropDefinition;
        growthStage = 0;
    }
}
