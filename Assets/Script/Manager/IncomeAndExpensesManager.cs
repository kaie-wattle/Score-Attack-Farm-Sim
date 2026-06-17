using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// 収入タイプ
/// </summary>
public enum IncomeType
{
    Crop,
    Livestock,
}

/// <summary>
/// 支出タイプ
/// </summary>
public enum ExpenseType
{
    Shop,
    Seed,
    Land,
    FieldWater,
}

public class IncomeAndExpensesManager : MonoBehaviour
{
    [SerializeField] GameObject ViewButon;
    [SerializeField] Button NextButon;
    [SerializeField] Button PrevButon;
    [Header("-----------ViewItem-----------")]
    [SerializeField] TMPro.TMP_Text yearText;
    [SerializeField] TMPro.TMP_Text monthText;
    [SerializeField] TMPro.TMP_Text IncomeTitleText;
    [SerializeField] TMPro.TMP_Text CropIncomeText;
    [SerializeField] TMPro.TMP_Text LivestockIncomeText;
    [SerializeField] TMPro.TMP_Text ShopExpenseText;
    [SerializeField] TMPro.TMP_Text SeedstockExpenseText;
    [SerializeField] TMPro.TMP_Text LandExpenseText;
    [SerializeField] TMPro.TMP_Text FieldWaterExpenseText;

    public struct MonthlyReport
    {
        public int year;
        public int month;
        public int cropIncome;
        public int livestockIncome;
        public int shopExpense;
        public int seedStockExpense;
        public int landExpense;
        public int fieldWaterExpense;
    }

    List<MonthlyReport> monthlyReports = new List<MonthlyReport>();
    
    int currentReportPage;
    int currentCropIncome;
    int currentLivestockIncome;

    int currentShopExpense;
    int currentSeedStockExpense;
    int currentLandExpense;
    int currentFieldWaterExpense;

    public void Initialize()
    {
        currentReportPage = 0;
        IsViewReportData();
    }

    public void SetIncomeData(int value, IncomeType incomeType)
    {
        switch (incomeType)
        {
            case IncomeType.Crop:
                currentCropIncome = value;
                break;
            case IncomeType.Livestock:
                currentLivestockIncome = value;
                break;
            default:
                break;
        }
    }

    public void SetExpenseData(int value, ExpenseType expenseType)
    {
        switch (expenseType)
        {
            case ExpenseType.Shop:
                currentShopExpense += value;
                break;
            case ExpenseType.Seed:
                currentSeedStockExpense += value;
                break;
            case ExpenseType.Land:
                currentLandExpense += value;
                break;
            case ExpenseType.FieldWater:
                currentFieldWaterExpense += value;
                break;
            default:
                break;
        }
    }

    public void SaveReportData(int year,int month)
    {
        SetReportData(year, month);
        IsViewReportData();
    }

    public void ReportActive()
    {
        currentReportPage = 0;
        gameObject.SetActive(true);
        ChangeButtonActive();
        ViewReportData();
    }

    void IsViewReportData()
    {
        if(monthlyReports?.Count > 0)
        {
            ViewButon.SetActive(true);
        }
        else
        {
            ViewButon.SetActive(false);
        }
    }

    void SetReportData(int year, int month)
    {
        MonthlyReport currentReport = new MonthlyReport();
        currentReport.year = year;
        currentReport.month = month;
        currentReport.cropIncome = currentCropIncome;
        currentReport.livestockIncome = currentLivestockIncome;
        currentReport.shopExpense = currentShopExpense;
        currentReport.seedStockExpense = currentSeedStockExpense;
        currentReport.landExpense = currentLandExpense;
        currentReport.fieldWaterExpense = currentFieldWaterExpense;

        monthlyReports.Insert(0,currentReport);

        currentCropIncome = 0;
        currentLivestockIncome = 0;
        currentShopExpense = 0;
        currentSeedStockExpense = 0;
        currentLandExpense = 0;
        currentShopExpense = 0;
    }

    void ViewReportData()
    {
        MonthlyReport currentReport;
        if (currentReportPage >= 0 && currentReportPage < monthlyReports.Count)
        {
            currentReport = monthlyReports[currentReportPage];
        }
        else
        {
            Debug.Log("リスト範囲外");
            return;
        }
        yearText.SetText(currentReport.year.ToString());
        monthText.SetText(currentReport.month.ToString());

        CropIncomeText.SetText(currentReport.cropIncome.ToString());
        LivestockIncomeText.SetText(currentReport.livestockIncome.ToString());
        ShopExpenseText.SetText(currentReport.shopExpense.ToString());
        SeedstockExpenseText.SetText(currentReport.seedStockExpense.ToString());
        LandExpenseText.SetText(currentReport.landExpense.ToString());
        FieldWaterExpenseText.SetText(currentReport.fieldWaterExpense.ToString());
    }

    void ChangeButtonActive()
    {
        if (currentReportPage <= 0)
        {
            currentReportPage = 0;
            NextButon.interactable = false;
        }
        else
        {
            NextButon.interactable = true;
        }

        if (currentReportPage >= monthlyReports?.Count - 1)
        {
            currentReportPage = monthlyReports.Count - 1;
            PrevButon.interactable = false;
        }
        else
        {
            PrevButon.interactable = true;
        }
    }

    #region ボタンイベント
    public void OnCloseButton()
    {
        gameObject.SetActive(false);
    }

    public void OnNextButton()
    {
        currentReportPage--;
        ChangeButtonActive();
        ViewReportData();
    }

    public void OnPrevButton()
    {
        currentReportPage++;
        ChangeButtonActive();
        ViewReportData();
    }
    #endregion
}
