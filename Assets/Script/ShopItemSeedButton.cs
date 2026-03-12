using UnityEngine;

public class ShopItemSeedButton : ShopItemButtonItemBase
{
    private SO_CropDefinition cropDef;

    public void SetShopItemButton(int _stock, SO_CropDefinition _cropDef)
    {
        buyCount = 0;
        stock = _stock;
        cropDef = _cropDef;
        sellPrice = cropDef.sellPrice;
        itemName = cropDef.cropName;
        itemNameText.SetText(cropDef.cropName);
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
            ResourceManager.Instance.AddSeed(cropDef, buyCount);
            stock -= buyCount;
            buyCount = 0;
            buyCountText.SetText(buyCount.ToString());
            stockText.SetText("ç›å…:" + stock.ToString());
            UpdateInteractable();
            Debug.Log("çwì¸ÇµÇ‹ÇµÇΩÅB");
        }
    }
}
