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
        int cost = sellPrice * buyCount;
        if (cost > ResourceManager.Instance.Money)
        {
            Debug.Log("çwì¸Ç≈Ç´Ç‹ÇπÇÒÇ≈ÇµÇΩÅB");
            AddBuyCount(0);
        }
        else
        {
            switch(landDef.landType)
            {
                case LandType.Farmland:
                    ResourceManager.Instance.AddMoney(-cost);
                    ResourceManager.Instance.AddField(buyCount);
                    stock -= buyCount;
                    buyCount = 0;
                    buyCountText.SetText(buyCount.ToString());
                    stockText.SetText("ç›å…:" + stock.ToString());
                    UpdateInteractable();
                    Debug.Log("î_ínÇägí£ÇµÇ‹ÇµÇΩÅB");
                    break;
                case LandType.LivestockArea:
                    Debug.Log("í{éYÉGÉäÉAÇägí£ÇµÇ‹ÇµÇΩÅB");
                    ResourceManager.Instance.AddMoney(-cost);
                    ResourceManager.Instance.AddLivestockArea(buyCount);
                    stock -= buyCount;
                    buyCount = 0;
                    buyCountText.SetText(buyCount.ToString());
                    stockText.SetText("ç›å…:" + stock.ToString());
                    UpdateInteractable();
                    break;
                case LandType.None:
                    Debug.Log("è§ïièÓïÒÇ™ê≥ÇµÇ≠ê›íËÇ≥ÇÍÇƒÇ¢Ç‹ÇπÇÒ");
                    break;
            }
            
        }
    }
}
