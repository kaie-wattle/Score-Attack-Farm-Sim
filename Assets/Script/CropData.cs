using UnityEngine;

public enum CropType
{
    None,
    /// <summary> ïƒ </summary>
    Rice,
    /// <summary> è¨îû </summary>
    Wheat,
    /// <summary> Ç…ÇÒÇ∂ÇÒ </summary>
    Carrot,
    /// <summary> ÇΩÇ‹ÇÀÇ¨ </summary>
    Onion,
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
