using UnityEngine;

public enum LivestockType
{
    None,
    /// <summary> ãç </summary>
    Cow,
    /// <summary> ìÿ </summary>
    Pig,
    /// <summary> å{ </summary>
    Chicken,
    /// <summary> ór </summary>
    Sheep,
}

/// <summary>
/// â∆í{èÓïÒ
/// </summary>
[System.Serializable]
public class LivestockData
{
    public SO_LivestockDefinition so_LivestockDefinition;

    public LivestockData(SO_LivestockDefinition livestockDefinition)
    {
        so_LivestockDefinition = livestockDefinition;
    }
}
