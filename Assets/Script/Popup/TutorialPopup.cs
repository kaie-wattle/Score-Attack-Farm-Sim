using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static IncomeAndExpensesManager;

public class TutorialPopup : PopupBase<TutorialPopup>
{
    [SerializeField] List<SO_TutorialCategory> tutorialCategoryList;
    [SerializeField] Image tutorialImage;
    [SerializeField] Button PrevButton;
    [SerializeField] Button NextButton;

    SO_TutorialCategory currentTutorialDef;
    int currentPage = 0;
    public void ShowCategory(TutorialType tutorialType)
    {
        var data = tutorialCategoryList.Find(x =>x.tutorialType == tutorialType);
        if (data != null)
        {
            ShowPopup(data);
        }
    }

    void ShowPopup(SO_TutorialCategory tutorialDef)
    {
        currentPage = 0;
        currentTutorialDef = tutorialDef;
        
        UpdateView();
        window.SetActive(true);
    }

    void UpdateView()
    {
        messageText.SetText(currentTutorialDef.CategoryTitle);
        if (currentPage <= 0)
        {
            currentPage = 0;
            PrevButton.interactable = false;
        }
        else
        {
            PrevButton.interactable = true;
        }

        if (currentPage >= currentTutorialDef?.pages.Count - 1)
        {
            currentPage = currentTutorialDef.pages.Count - 1;
            NextButton.interactable = false;
        }
        else
        {
            NextButton.interactable = true;
        }
        tutorialImage.sprite = currentTutorialDef.pages[currentPage];
    }

    #region ボタンイベント
    public void OnNextButton()
    {
        currentPage++;
        UpdateView();
    }

    public void OnPrevButton()
    {
        currentPage--;
        UpdateView();
    }
    #endregion
}
