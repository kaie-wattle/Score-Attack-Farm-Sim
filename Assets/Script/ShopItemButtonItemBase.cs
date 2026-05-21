using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemButtonItemBase : MonoBehaviour
{
    [SerializeField] protected TMPro.TMP_Text itemNameText;
    [SerializeField] protected TMPro.TMP_Text itemPriceText;
    [SerializeField] protected TMPro.TMP_Text buyCountText;
    [SerializeField] protected TMPro.TMP_Text stockText;
    [SerializeField] protected Button buyButton;

    protected int buyCount;
    protected int stock;
    protected int sellPrice;
    protected string itemName;


    /// <summary>
    /// 押下可否更新
    /// </summary>
    public void UpdateInteractable()
    {
        if (buyCount == 0)
        {
            buyButton.interactable = false;
        }
        else if (buyCount > ResourceManager.Instance.Money / sellPrice)
        {
            buyButton.interactable = false;
        }
        else if (stock > 0)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }
    }

    /// <summary>
    /// 在庫更新
    /// </summary>
    /// <param name="stock">在庫</param>
    public void UpdateStock(int stock)
    {
        this.stock = stock;
        stockText.SetText("在庫:" + stock.ToString());
    }

    /// <summary>
    /// 購入数リセット
    /// </summary>
    public void ResetBuyCount()
    {
        buyCount = 0;
        buyCountText.SetText(buyCount.ToString());
        UpdateInteractable();
    }

    protected void AddBuyCount(int amount)
    {
        buyCount += amount;
        int cost = sellPrice * buyCount;
        int MaxbuyCount = ResourceManager.Instance.Money / sellPrice;
        // 在庫上限
        buyCount = Mathf.Min(buyCount, stock);
        // 資金上限
        buyCount = Mathf.Min(buyCount, MaxbuyCount);
        // 下限
        buyCount = Mathf.Max(buyCount, 0);
        buyCountText.SetText(buyCount.ToString());
        UpdateInteractable();
    }

    #region ボタン押下処理
    public void OnPlusButton()
    {
        AddBuyCount(1);
    }

    public void OnPlusTenButton()
    {
        AddBuyCount(10);
    }

    public void OnMinusButton()
    {
        AddBuyCount(-1);
    }

    public void OnMinusTenButton()
    {
        AddBuyCount(-10);
    }
    #endregion

}
