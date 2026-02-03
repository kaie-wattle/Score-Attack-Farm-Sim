using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] UIManager uiManager;
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

    public void OnNextDateClick()
    {
        int money = Random.Range(-100, 100);
        uiManager.UpdateUI(money);
    }

    void GameClear()
    {
        clearUI.SetActive(true);
        uiManager.UpdateClearUI(Random.Range(100, 1000));
    }
}
