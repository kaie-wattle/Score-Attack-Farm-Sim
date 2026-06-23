using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CropButtonItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text cropNameText;
    [SerializeField] TMPro.TMP_Text seedCountText;
    [SerializeField] Button cropButton;

    private SO_CropDefinition cropDef;
    public SO_CropDefinition CropDef => cropDef;

    public event UnityAction<SO_CropDefinition> OnClikedEvent;

    public void SetCropButton(SO_CropDefinition _cropDef,int seedCount)
    {
        cropDef = _cropDef;
        cropNameText.SetText(cropDef.cropName);
        seedCountText.SetText(seedCount.ToString());

        UpdateInteractable(seedCount);
    }

    public void OnClick()
    {
        OnClikedEvent?.Invoke(cropDef);
    }

    public void UpdateSeedCount(int count)
    {
        seedCountText.SetText(count.ToString());
        UpdateInteractable(count);
    }

    void UpdateInteractable(int seedCount)
    {
        cropButton.interactable = seedCount > 0;
    }
}
