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
        clearUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Ÿ‚ÌŒƒ{ƒ^ƒ“‰Ÿ‰ºˆ—
    /// </summary>
    public void OnNextDateClick()
    {
        int money = Random.Range(-100, 100);
        uiManager.UpdateUI(money);
        tileMapManager.UpdateTile();
    }

    void GameClear()
    {
        clearUI.SetActive(true);
        uiManager.UpdateClearUI(Random.Range(100, 1000));
    }
}
