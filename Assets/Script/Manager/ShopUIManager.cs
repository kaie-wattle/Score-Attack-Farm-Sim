using System.Collections.Generic;
using UnityEngine;

public class ShopUIManager : MonoBehaviour
{
    [SerializeField] Transform shopButtonParent;
    [SerializeField] ShopItemButtonItem shopButtonItemPrefabs;

    private List<ShopItemButtonItem> shopButtons = new List<ShopItemButtonItem>();

    public void Initialize(List<SO_CropDefinition> cropDefinitionList)
    {
        shopButtons.Clear();

        foreach (var crop in cropDefinitionList)
        {
            var button = Instantiate(shopButtonItemPrefabs, shopButtonParent);
            button.SetShopItemButton(25,10000,crop);
            shopButtons.Add(button);
        }
    }
}
