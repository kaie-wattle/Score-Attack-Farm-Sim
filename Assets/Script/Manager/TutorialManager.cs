using UnityEngine;

public class TutorialManager : MonoBehaviour
{

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
    }

    public void OnIntroductionButton()
    {
        TutorialPopup.instance.ShowCategory(TutorialType.Introduction);
    }

    public void OnFarmingButton()
    {
        TutorialPopup.instance.ShowCategory(TutorialType.Farming);
    }

    public void OnLivestockButton()
    {
        TutorialPopup.instance.ShowCategory(TutorialType.Livestock);
    }

    public void OnShopButton()
    {
        TutorialPopup.instance.ShowCategory(TutorialType.Shop);
    }

    public void OnGameEventButton()
    {
        TutorialPopup.instance.ShowCategory(TutorialType.GameEvent);
    }
}
