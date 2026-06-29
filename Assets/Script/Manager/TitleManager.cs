using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    public void OnOneYearStartButton() => OnGameStartButton(1);
    public void OnThreeYearStartButton() => OnGameStartButton(3);
    public void OnFiveYearStartButton() => OnGameStartButton(5);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGameStartButton(int playYear)
    {
        PlaySettings.PlayYears = playYear;
        SceneManager.LoadScene("MainScene");
    }

    public void OnGameEndButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
