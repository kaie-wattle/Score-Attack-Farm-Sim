using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] DateManager dateManager;
    [SerializeField] TileMapManager tileMapManager;
    [SerializeField] int endYear;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dateManager.OnDateChenged += uiManager.UpdateDate;
        tileMapManager.OnHarvested += Harvested;
        uiManager.Initialize();
        dateManager.Initialize(endYear);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 次の月ボタン押下処理
    /// </summary>
    public void OnNextDateClick()
    {
        bool isEnd = dateManager.AdvanceToNextTime();
        
        tileMapManager.UpdateTile();
        if(isEnd)
        {
            GameClear();
        }
        
    }

    /// <summary>
    /// 収穫された
    /// </summary>
    /// <param name="income"></param>
    void Harvested(int income)
    {
        uiManager.UpdateMoney(income);
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    void GameClear()
    {
        // TODO:スコアは仮
        uiManager.UpdateClearUI(Random.Range(100, 1000));
    }

    private void OnDestroy()
    {
        dateManager.OnDateChenged -= uiManager.UpdateDate;
        tileMapManager.OnHarvested -= Harvested;
    }
}
