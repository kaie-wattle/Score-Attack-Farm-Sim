using UnityEngine;

public class ShopItemLivestockButton : ShopItemButtonItemBase
{
    private SO_LivestockDefinition livestockDef;

    public void SetShopItemButton(int _stock, SO_LivestockDefinition _livestockDef)
    {
        buyCount = 0;
        stock = _stock;
        livestockDef = _livestockDef;
        sellPrice = livestockDef.livestockPrice;
        itemName = livestockDef.livestockName;
        itemNameText.SetText(itemName);
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
            ResourceManager.Instance.AddMoney(-cost);
            ResourceManager.Instance.AddLivestock(livestockDef, buyCount);
            stock -= buyCount;
            buyCount = 0;
            buyCountText.SetText(buyCount.ToString());
            stockText.SetText("ç›å…:" + stock.ToString());
            UpdateInteractable();
            Debug.Log("çwì¸ÇµÇ‹ÇµÇΩÅB");
        }
    }
}
