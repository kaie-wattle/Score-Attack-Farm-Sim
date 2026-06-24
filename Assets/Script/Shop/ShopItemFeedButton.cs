using UnityEngine;

public class ShopItemFeedButton : ShopItemButtonItemBase
{
    [SerializeField] int feedPrice;
    public void SetShopItemButton(int _stock)
    {
        buyCount = 0;
        stock = _stock;
        sellPrice = feedPrice;
        itemName = "â∆í{ópéîóø";
        itemNameText.SetText(itemName);
        itemPriceText.SetText(sellPrice.ToString() + "â~");
        buyCountText.SetText(buyCount.ToString());
        stockText.SetText("ç›å…:" + stock.ToString());
        UpdateInteractable();
    }

    /// <summary>
    /// çwì¸É{É^Éìâüâ∫èàóù
    /// </summary>
    public void OnBuyButton()
    {
        int cost = sellPrice * buyCount;
        if (cost > ResourceManager.Instance.Money)
        {
            Debug.Log("çwì¸Ç≈Ç´Ç‹ÇπÇÒÇ≈ÇµÇΩÅB");
            AddBuyCount(0);
        }
        else
        {
            InvokeOnExpensed(cost, ExpenseType.Shop);
            ResourceManager.Instance.AddFeed(buyCount);
            stock -= buyCount;
            buyCount = 0;
            buyCountText.SetText(buyCount.ToString());
            stockText.SetText("ç›å…:" + stock.ToString());
            UpdateInteractable();
            Debug.Log("çwì¸ÇµÇ‹ÇµÇΩÅB");
        }
    }

    public void OnPlusFiftyButton()
    {
        AddBuyCount(50);
    }

    public void OnMinusFiftyButton()
    {
        AddBuyCount(-50);
    }
}
