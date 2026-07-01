using System.Collections.Generic;
using UnityEngine;

public enum TutorialType
{
    Introduction,
    Farming,
    Livestock,
    Shop,
    GameEvent
}


/// <summary>
/// チュートリアル定義
/// </summary>
[CreateAssetMenu(menuName = "Tutorial")]
public class SO_TutorialCategory : ScriptableObject
{
    public TutorialType tutorialType;
    public string CategoryTitle;
    public List<Sprite> pages;
}
