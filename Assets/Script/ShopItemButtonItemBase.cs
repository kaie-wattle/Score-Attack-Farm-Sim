using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ShopItemButtonItemBase : MonoBehaviour
{
    [SerializeField] protected TMPro.TMP_Text itemNameText;
    [SerializeField] protected TMPro.TMP_Text buyCountText;
    [SerializeField] protected TMPro.TMP_Text stockText;
    [SerializeField] protected Button buyButton;

    protected int buyCount;
    protected int stock;
    protected int sellPrice;
    protected string itemName;

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

    protected void AddBuyCount(int amount)
    {
        buyCount += amount;
        int cost = sellPrice * buyCount;
        int MaxbuyCount = ResourceManager.Instance.Money / sellPrice;
        // ç›å…è„å¿
        buyCount = Mathf.Min(buyCount, stock);
        // éëã‡è„å¿
        buyCount = Mathf.Min(buyCount, MaxbuyCount);
        // â∫å¿
        buyCount = Mathf.Max(buyCount, 0);
        buyCountText.SetText(buyCount.ToString());
        UpdateInteractable();
    }

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
}
