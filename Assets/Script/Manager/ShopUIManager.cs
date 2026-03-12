using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopSeedButtonParent;
    [SerializeField] GameObject shopLivestockButtonParent;
    [SerializeField] GameObject shopExpansionButtonParent;
    [SerializeField] ShopItemSeedButton shopButtonSeedItemPrefabs;
    [SerializeField] TMPro.TMP_Text moneyText;

    private List<ShopItemSeedButton> shopButtons = new List<ShopItemSeedButton>();
    private int currentMoney;
    public int CurrentMoney => currentMoney;

    public void Initialize(List<SO_CropDefinition> cropDefinitionList)
    {
        shopButtons.Clear();

        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(shopButtonSeedItemPrefabs, shopSeedButtonParent.transform);
            button.SetShopItemButton(25,crop);
            shopButtons.Add(button);
        }
    }

    /// <summary>
    /// èäéùã‡çXêV
    /// </summary>
    public void UpdateMoney()
    {
        moneyText.SetText(ResourceManager.Instance.Money.ToString());
        foreach (var button in shopButtons)
        {
            button.UpdateInteractable();
        }
    }

    public void ShopActive()
    {
        shopUI.SetActive(true);
    }

    public void OnShopCloseButton()
    {
        shopUI.SetActive(false);
    }

    public void OnSeedTabButton()
    {
        shopSeedButtonParent.SetActive(true);
        shopLivestockButtonParent.SetActive(false);
        shopExpansionButtonParent.SetActive(false);
    }

    public void OnLivestockTabButton()
    {
        shopSeedButtonParent.SetActive(false);
        shopLivestockButtonParent.SetActive(true);
        shopExpansionButtonParent.SetActive(false);
    }

    public void OnExpansionTabButton()
    {
        shopSeedButtonParent.SetActive(false);
        shopLivestockButtonParent.SetActive(false);
        shopExpansionButtonParent.SetActive(true);
    }
}
