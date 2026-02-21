using UnityEngine;
using UnityEngine.UI;

public class ShopItemButtonItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text itemNameText;
    [SerializeField] TMPro.TMP_Text buyCountText;
    [SerializeField] TMPro.TMP_Text stockText;
    [SerializeField] Button buyButton;

    private int currentMoney;
    private int buyCount;
    private int stock;
    private SO_CropDefinition cropDef;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    //void Start()
    //{
    //    // 以下はデバッグ
    //    buyCount = 0;
    //    stock = 25;
    //    currentMoney = 1000;
    //    buyCountText.SetText(buyCount.ToString());
    //    stockText.SetText("在庫:" + stock.ToString());
    //}

    public void SetShopItemButton(int _stock,int _currentMoney,SO_CropDefinition _cropDef)
    {
        buyCount = 0;
        stock = _stock;
        currentMoney = _currentMoney;
        cropDef = _cropDef;
        itemNameText.SetText(cropDef.cropName);
        buyCountText.SetText(buyCount.ToString());
        stockText.SetText("在庫:" + stock.ToString());
    }

    void UpdateInteractable()
    {
        buyButton.interactable = stock > 0;
    }

    public void OnPlusButton()
    {
        buyCount++;
        if (buyCount > stock)
        {
            buyCount = stock;
        }
        buyCountText.SetText(buyCount.ToString());
    }

    public void OnPlusTenButton()
    {
        buyCount += 10;
        if (buyCount > stock)
        {
            buyCount = stock;
        }
        buyCountText.SetText(buyCount.ToString());
    }

    public void OnMinusButton()
    {
        buyCount--;
        if (buyCount < 0)
        {
            buyCount = 0;
        }
        buyCountText.SetText(buyCount.ToString());
    }

    public void OnMinusTenButton()
    {
        buyCount -= 10;
        if (buyCount < 0)
        {
            buyCount = 0;
        }
        buyCountText.SetText(buyCount.ToString());
    }

    public void OnBuyButton()
    {
        stock -= buyCount;
        buyCount = 0;
        buyCountText.SetText(buyCount.ToString());
        stockText.SetText("在庫:" + stock.ToString());
        UpdateInteractable();
        Debug.Log("購入しました。");
    }
}
