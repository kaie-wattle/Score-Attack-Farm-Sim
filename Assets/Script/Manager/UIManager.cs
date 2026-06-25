using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("-----Year-----")]
    [SerializeField] TMPro.TMP_Text yearText;
    [SerializeField] TMPro.TMP_Text monthText;

    [Header("-----Money-----")]
    [SerializeField] TMPro.TMP_Text moneyText;

    [Header("-----Feed-----")]
    [SerializeField] TMPro.TMP_Text feedText;

    [Header("-----Dirtiness-----")]
    [SerializeField] TMPro.TMP_Text dirtinessText;

    [Header("-----Crop-----")]
    [SerializeField] GameObject cropInfo;
    [SerializeField] Transform cropButtonParent;
    [SerializeField] CropButtonItem cropButtonItemPrefabs;
    [SerializeField] TMPro.TMP_Text selectedCropText;

    [Header("-----Livestock-----")]
    [SerializeField] GameObject livestockInfo;
    [SerializeField] Transform livestockButtonParent;
    [SerializeField] LivestockButtonItem livestockButtonItemPrefabs;
    [SerializeField] Button sellLivestockButton;
    [SerializeField] Button cleaningButton;

    [Header("-----Score-----")]
    [SerializeField] Transform scoreDetailParent;
    [SerializeField] ScoreDetailItem scoreDetailItemPrefab;

    [Header("-----GameEnd-----")]
    [SerializeField] GameObject clearUI;
    [SerializeField] GameObject gameOverUI;

    private UnityAction<SO_CropDefinition> cropDefinitionAction;
    private List<CropButtonItem> cropButtons = new List<CropButtonItem>();
    private List<LivestockButtonItem> livestockButtons = new List<LivestockButtonItem>();

    public void Initialize(List<SO_CropDefinition> cropDefinitionList, List<SO_LivestockDefinition> livestockDefinitionList, UnityAction<SO_CropDefinition> _cropDefinitionAction)
    {
        cropDefinitionAction = _cropDefinitionAction;
        ClearScoreDetails();
        cropInfo.SetActive(true);
        livestockInfo.SetActive(false);
        clearUI.SetActive(false);
        gameOverUI.SetActive(false);
        cropButtons.Clear();
        UpdateFeed();
        UpdateDirtiness();

        selectedCropText.SetText("未選択");
        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(cropButtonItemPrefabs, cropButtonParent);
            button.SetCropButton(crop, 0);
            button.OnClikedEvent += cropDefinitionAction;
            cropButtons.Add(button);
        }

        foreach (var livestock in livestockDefinitionList)
        {
            var button = Instantiate(livestockButtonItemPrefabs, livestockButtonParent);
            button.SetLivestockDefButton(livestock, 0);
            livestockButtons.Add(button);
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
    public void UpdateMoney()
    {
        moneyText.SetText(ResourceManager.Instance.Money.ToString() + "円");
    }

    /// <summary>
    /// 所持餌更新
    /// </summary>
    public void UpdateFeed()
    {
        feedText.SetText(ResourceManager.Instance.Feed.ToString());
    }

    /// <summary>
    /// 汚れ更新
    /// </summary>
    public void UpdateDirtiness()
    {
        dirtinessText.SetText(ResourceManager.Instance.Dirtiness.ToString());
    }

    /// <summary>
    /// 種子保有状況更新
    /// </summary>
    /// <param name="_cropDef"></param>
    public void UpdateSeedCount(SO_CropDefinition _cropDef)
    {
        foreach (var button in cropButtons)
        {
            if (button.CropDef == _cropDef)
            {
                button.UpdateSeedCount(ResourceManager.Instance.GetSeedCount(_cropDef));
                break;
            }
        }
    }

    /// <summary>
    /// 家畜保有状況更新
    /// </summary>
    /// <param name="_livestockDef"></param>
    public void UpdateLivestockCount(SO_LivestockDefinition _livestockDef)
    {
        foreach (var button in livestockButtons)
        {
            if (button.LivestockDef == _livestockDef)
            {
                button.UpdateStockCount(ResourceManager.Instance.GetLivestockCount(_livestockDef));
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
    /// アイテム選択リスト更新
    /// </summary>
    /// <param name="landType">タイルマップ種別</param>
    public void ChangeItemSelectList(LandType landType)
    {
        switch (landType)
        {
            case LandType.Farmland:
                cropInfo.SetActive(true);
                livestockInfo.SetActive(false);
                sellLivestockButton.gameObject.SetActive(false);
                cleaningButton.gameObject.SetActive(false);
                Debug.Log("農場押下");
                break;
            case LandType.LivestockArea:
                cropInfo.SetActive(false);
                livestockInfo.SetActive(true);
                sellLivestockButton.gameObject.SetActive(true);
                cleaningButton.gameObject.SetActive(true);
                Debug.Log("畜産押下");
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ゲームオーバー画面表示
    /// </summary>
    public void ShowGameOverUI()
    {
        gameOverUI.SetActive(true);
    }

    /// <summary>
    /// クリア画面表示
    /// </summary>
    /// <param name="result">スコア詳細情報</param>
    public void ShowClearUI(ScoreResult result)
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
            button.OnClikedEvent -= cropDefinitionAction;
        }
        cropButtons.Clear();
        livestockButtons.Clear();
    }
}
