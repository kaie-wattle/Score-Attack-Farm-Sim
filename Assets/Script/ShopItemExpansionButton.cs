using UnityEngine;

public class ShopItemExpansionButton : ShopItemButtonItemBase
{
    private SO_LandDefinition landDef;

    public void SetShopItemButton(int _stock, SO_LandDefinition _landDef)
    {
        buyCount = 0;
        stock = _stock;
        landDef = _landDef;
        sellPrice = landDef.sellPrice;
        itemName = landDef.landName;
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
        
    }
}
