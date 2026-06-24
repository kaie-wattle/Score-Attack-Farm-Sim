using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] GameObject shopSeedButtonParent;
    [SerializeField] GameObject shopLivestockButtonParent;
    [SerializeField] GameObject shopExpansionButtonParent;
    [SerializeField] ShopItemSeedButton shopButtonSeedItemPrefabs;
    [SerializeField] ShopItemLivestockButton shopButtonLivestockItemPrefabs;
    [SerializeField] ShopItemFeedButton shopButtonFeedItemPrefabs;
    [SerializeField] ShopItemExpansionButton shopButtonExpansionItemPrefabs;
    [SerializeField] TMPro.TMP_Text moneyText;

    public event UnityAction<int, ExpenseType> OnExpensed;
    public int CurrentMoney => currentMoney;

    private List<ShopItemSeedButton> shopSeedButtons = new List<ShopItemSeedButton>();
    private List<ShopItemLivestockButton> shopLivestockButtons = new List<ShopItemLivestockButton>();
    private List<ShopItemFeedButton> shopFeedButtons = new List<ShopItemFeedButton>();
    private List<ShopItemExpansionButton> shopExpansionButtons = new List<ShopItemExpansionButton>();
    private int currentMoney;
    

    public void Initialize(List<SO_CropDefinition> cropDefinitionList, List<SO_LivestockDefinition> livestockDefinitionList, List<SO_LandDefinition> landDefinitionList,int livestockStock,UnityAction<int,ExpenseType> _onExpensed)
    {
        shopSeedButtons.Clear();
        shopExpansionButtons.Clear();
        OnExpensed = _onExpensed;

        // 種子商品リスト
        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(shopButtonSeedItemPrefabs, shopSeedButtonParent.transform);
            int stock = Random.Range(5, 30);
            button.SetShopItemButton(stock, crop);
            button.OnExpensed += OnExpensed;
            shopSeedButtons.Add(button);
        }

        // 家畜商品リスト
        var feedButton = Instantiate(shopButtonFeedItemPrefabs, shopLivestockButtonParent.transform);
        int feedStock = Random.Range(1, 6) * 500;
        feedButton.SetShopItemButton(feedStock);
        feedButton.OnExpensed += OnExpensed;
        shopFeedButtons.Add(feedButton);


        foreach (var livestock in livestockDefinitionList)
        {
            var button = Instantiate(shopButtonLivestockItemPrefabs, shopLivestockButtonParent.transform);
            button.SetShopItemButton(livestockStock, livestock);
            button.OnExpensed += OnExpensed;
            shopLivestockButtons.Add(button);
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
            button.OnExpensed += OnExpensed;
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

        foreach (var button in shopLivestockButtons)
        {
            button.UpdateInteractable();
        }

        foreach (var button in shopFeedButtons)
        {
            button.UpdateInteractable();
        }

        foreach (var button in shopExpansionButtons)
        {
            button.UpdateInteractable();
        }
    }

    /// <summary>
    /// 種子在庫更新
    /// </summary>
    public void UpdateSeedStock()
    {
        foreach (var button in shopSeedButtons)
        {
            int stock = Random.Range(5, 30);
            button.UpdateStock(stock);
            button.ResetBuyCount();
        }
    }

    /// <summary>
    /// 家畜在庫更新
    /// </summary>
    public void UpdateLivestockStock(int stock)
    {
        foreach (var button in shopLivestockButtons)
        {
            button.UpdateStock(stock);
            button.ResetBuyCount();
        }
    }

    /// <summary>
    /// 家畜飼料在庫更新
    /// </summary>
    public void UpdateFeedStock()
    {
        foreach (var button in shopFeedButtons)
        {
            int stock = Random.Range(1, 6) * 500;
            button.UpdateStock(stock);
            button.ResetBuyCount();
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

        foreach (var button in shopLivestockButtons)
        {
            button.ResetBuyCount();
        }

        foreach (var button in shopFeedButtons)
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

    private void OnDestroy()
    {
        foreach (var button in shopSeedButtons)
        {
            button.OnExpensed -= OnExpensed;
        }
        foreach (var button in shopLivestockButtons)
        {
            button.OnExpensed -= OnExpensed;
        }
        foreach (var button in shopFeedButtons)
        {
            button.OnExpensed -= OnExpensed;
        }
        foreach (var button in shopExpansionButtons)
        {
            button.OnExpensed -= OnExpensed;
        }
        shopSeedButtons.Clear();
        shopLivestockButtons.Clear();
        shopFeedButtons.Clear();
        shopExpansionButtons.Clear();
    }
}
