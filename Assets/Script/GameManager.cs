using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
    [SerializeField] TileMapManager tileMapManager;
    [SerializeField] GameObject clearUI;
    [SerializeField] int endYear;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        uiManager.Initialize(endYear);
        uiManager.gameClear += GameClear;
        tileMapManager.onHarvested += OnHarvested;
        clearUI.SetActive(false);
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
        uiManager.UpdateDate();
        tileMapManager.UpdateTile();
    }

    /// <summary>
    /// 収穫された
    /// </summary>
    /// <param name="income"></param>
    void OnHarvested(int income)
    {
        uiManager.UpdateMoney(income);
    }

    /// <summary>
    /// ゲームクリア
    /// </summary>
    void GameClear()
    {
        clearUI.SetActive(true);
        uiManager.UpdateClearUI(Random.Range(100, 1000));
    }

    private void OnDestroy()
    {
        uiManager.gameClear -= GameClear;
        tileMapManager.onHarvested -= OnHarvested;
    }
}
