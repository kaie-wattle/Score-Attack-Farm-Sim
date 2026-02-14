using UnityEngine;
using UnityEngine.Events;

public class CropButtonItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text cropNameText;

    private SO_CropDefinition cropDef;

    public event UnityAction<SO_CropDefinition> OnClikedEvent;

    public void SetCropButton(SO_CropDefinition _cropDef)
    {
        cropDef = _cropDef;
        cropNameText.SetText(cropDef.cropName);
    }

    public void OnClick()
    {
        OnClikedEvent?.Invoke(cropDef);
    }
}
