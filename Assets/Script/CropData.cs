using UnityEngine;

public enum CropType
{
    None,
    /// <summary> •Ä </summary>
    Rice,
    /// <summary> ¬” </summary>
    Wheat
}

/// <summary>
/// ì•¨î•ñ
/// </summary>
[System.Serializable]
public class CropData
{
    public SO_CropDefinition so_CropDefinition;
    /// <summary> ¬’·’iŠK </summary>
    public int growthStage;

    public CropData(SO_CropDefinition cropDefinition)
    {
        so_CropDefinition = cropDefinition;
        growthStage = 0;
    }
}
