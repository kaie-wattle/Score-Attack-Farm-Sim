using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopSeedButtonParent;
    [SerializeField] GameObject shopLivestockButtonParent;
    [SerializeField] GameObject shopExpansionButtonParent;
    [SerializeField] ShopItemSeedButton shopButtonSeedItemPrefabs;
    [SerializeField] ShopItemExpansionButton shopButtonExpansionItemPrefabs;
    [SerializeField] TMPro.TMP_Text moneyText;

    private List<ShopItemSeedButton> shopSeedButtons = new List<ShopItemSeedButton>();
    private List<ShopItemExpansionButton> shopExpansionButtons = new List<ShopItemExpansionButton>();
    private int currentMoney;
    public int CurrentMoney => currentMoney;

    public void Initialize(List<SO_CropDefinition> cropDefinitionList, List<SO_LandDefinition> landDefinitionList)
    {
        shopSeedButtons.Clear();
        shopExpansionButtons.Clear();

        // 種子商品リスト
        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(shopButtonSeedItemPrefabs, shopSeedButtonParent.transform);
            button.SetShopItemButton(25,crop);
            shopSeedButtons.Add(button);
        }

        // 土地拡張リスト
        foreach (var land in landDefinitionList)
        {
            int stock = 0;
            switch (land.landType)
            {
                case LandType.Farmland:
                    stock = ResourceManager.Instance.GetAreaMax() - ResourceManager.Instance.FieldCount;
                    break;
                case LandType.LivestockArea:
                    stock = ResourceManager.Instance.GetAreaMax() - ResourceManager.Instance.LivestockAreaCount;
                    break;
                case LandType.None:
                    Debug.Log("商品情報が正しく設定されていません");
                    break;
            }
            var button = Instantiate(shopButtonExpansionItemPrefabs, shopExpansionButtonParent.transform);
            button.SetShopItemButton(stock, land);
            shopExpansionButtons.Add(button);
        }
    }

    /// <summary>
    /// 所持金更新
    /// </summary>
    public void UpdateMoney()
    {
        moneyText.SetText(ResourceManager.Instance.Money.ToString());
        foreach (var button in shopSeedButtons)
        {
            button.UpdateInteractable();
        }

        foreach (var button in shopExpansionButtons)
        {
            button.UpdateInteractable();
        }
    }

    public void ShopActive()
    {
        shopUI.SetActive(true);
        OnSeedTabButton();
    }

    public void OnShopCloseButton()
    {
        shopUI.SetActive(false);
        foreach (var button in shopSeedButtons)
        {
            button.ResetBuyCount();
        }

        foreach (var button in shopExpansionButtons)
        {
            button.ResetBuyCount();
        }
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
