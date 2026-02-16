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
    [SerializeField] TMPro.TMP_Text selectedCropText;

    [SerializeField] Transform scoreDetailParent;
    [SerializeField] ScoreDetailItem scoreDetailItemPrefab;

    private GameManager gameManager;
    private List<CropButtonItem> cropButtons = new List<CropButtonItem>();

    public void Initialize(GameManager _gameManager, List<SO_CropDefinition> cropDefinitionList)
    {
        gameManager = _gameManager;
        ClearScoreDetails();
        clearUI.SetActive(false);
        cropButtons.Clear();

        selectedCropText.SetText("未選択");
        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(cropButtonItemPrefabs, cropButtonParent);
            button.SetCropButton(crop,0);
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
    /// 種子保有状況更新
    /// </summary>
    /// <param name="_cropDef"></param>
    /// <param name="count"></param>
    public void UpdateSeedCount(SO_CropDefinition _cropDef,int count)
    {
        foreach (var button in cropButtons)
        {
            if(button.CropDef == _cropDef)
            {
                button.UpdateSeedCount(count);
                break;
            }
        }
    }

    /// <summary>
    /// 選択中作物名更新
    /// </summary>
    /// <param name="_cropDef">作物情報</param>
    public void UpdateSelectedCrop(SO_CropDefinition _cropDef)
    {
        if (_cropDef == null)
        {
            selectedCropText.SetText("未選択");
        }
        else
        {
            selectedCropText.SetText(_cropDef.cropName + "選択中");
        }
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
