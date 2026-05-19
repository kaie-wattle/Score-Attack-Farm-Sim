using UnityEngine;
using UnityEngine.Events;

public class LivestockButtonItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text livestockNameText;
    [SerializeField] TMPro.TMP_Text stockCountText;

    private SO_LivestockDefinition livestockDef;
    public SO_LivestockDefinition LivestockDef => livestockDef;

    public void SetLivestockDefButton(SO_LivestockDefinition _livestockDef, int stockCount)
    {
        livestockDef = _livestockDef;
        livestockNameText.SetText(livestockDef.livestockName);
        stockCountText.SetText(stockCount.ToString());
    }

    public void UpdateStockCount(int count)
    {
        stockCountText.SetText(count.ToString());
    }
}
