using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] GameObject shopUI;
    [SerializeField] Transform shopButtonParent;
    [SerializeField] ShopItemButtonItem shopButtonItemPrefabs;
    [SerializeField] TMPro.TMP_Text moneyText;

    private List<ShopItemButtonItem> shopButtons = new List<ShopItemButtonItem>();
    private int currentMoney;
    public int CurrentMoney => currentMoney;

    public void Initialize(List<SO_CropDefinition> cropDefinitionList)
    {
        shopButtons.Clear();

        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(shopButtonItemPrefabs, shopButtonParent);
            button.SetShopItemButton(25,this,crop);
            shopButtons.Add(button);
        }
    }

    /// <summary>
    /// èäéùã‡çXêV
    /// </summary>
    public void UpdateMoney()
    {
        moneyText.SetText(ResourceManager.Instance.Money.ToString());
    }

    public void OnShopCloseButton()
    {
        shopUI.SetActive(false);
    }

    public void ShopActive()
    {
        shopUI.SetActive(true);
    }
}
