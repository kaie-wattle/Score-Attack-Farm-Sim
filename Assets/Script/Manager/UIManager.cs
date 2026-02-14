using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text yearText;
    [SerializeField] TMPro.TMP_Text monthText;
    [SerializeField] TMPro.TMP_Text moneyText;
    [SerializeField] GameObject clearUI;

    [SerializeField] Transform cropButtonParent;
    [SerializeField] CropButtonItem cropButtonItemPrefabs;
    [SerializeField] List<SO_CropDefinition> cropDefinitions;

    [SerializeField] Transform scoreDetailParent;
    [SerializeField] ScoreDetailItem scoreDetailItemPrefab;

    private GameManager gameManager;
    private List<CropButtonItem> cropButtons = new List<CropButtonItem>();

    public void Initialize(GameManager _gameManager)
    {
        gameManager = _gameManager;
        ClearScoreDetails();
        clearUI.SetActive(false);
        cropButtons.Clear();

        foreach (var crop in cropDefinitions)
        {
            var button = Instantiate(cropButtonItemPrefabs, cropButtonParent);
            button.SetCropButton(crop);
            button.OnClikedEvent += _gameManager.SetSelectedCrop;
            cropButtons.Add(button);
        }
    }

    /// <summary>
    /// 日付更新
    /// </summary>
    public void UpdateDate(int year, int month)
    {
        yearText.SetText(year + "年目");
        monthText.SetText(month + "月");
    }

    /// <summary>
    /// 所持金更新
    /// </summary>
    /// <param name="money">お金</param>
    public void UpdateMoney(int money)
    {
        moneyText.SetText(money.ToString());
    }

    /// <summary>
    /// クリア画面表示
    /// </summary>
    /// <param name="result">スコア詳細情報</param>
    public void UpdateClearUI(ScoreResult result)
    {
        ClearScoreDetails();
        clearUI.SetActive(true);

        var totalScore = Instantiate(scoreDetailItemPrefab, scoreDetailParent);
        totalScore.SetScoreDetailText("Score", result.TotalScore, false);

        var baseHeader = Instantiate(scoreDetailItemPrefab, scoreDetailParent);
        baseHeader.SetScoreDetailText("---- 基本スコア ----", 0, true);

        // 基本スコア
        foreach (var detail in result.BaseScoreDetails)
        {
            var item = Instantiate(scoreDetailItemPrefab, scoreDetailParent);
            item.SetScoreDetailText(detail.Name, detail.Score, false);
        }

        // ボーナス見出し（任意）
        if (result.BonusScoreDetails.Count > 0)
        {
            var header = Instantiate(scoreDetailItemPrefab, scoreDetailParent);
            header.SetScoreDetailText("---- ボーナス ----", 0, true);
        }

        // ボーナス
        foreach (var detail in result.BonusScoreDetails)
        {
            var item = Instantiate(scoreDetailItemPrefab, scoreDetailParent);
            item.SetScoreDetailText(detail.Name, detail.Score, false);
        }
    }

    void ClearScoreDetails()
    {
        foreach (Transform child in scoreDetailParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void OnDestroy()
    {
        foreach (var button in cropButtons)
        {
            button.OnClikedEvent -= gameManager.SetSelectedCrop;
        }
        cropButtons.Clear();
    }
}
