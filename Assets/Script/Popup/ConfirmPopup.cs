using System;
using UnityEngine;

public class ConfirmPopup : PopupBase<ConfirmPopup>
{
    [SerializeField] TMPro.TMP_Text messageText;

    Action yesAction;

    public void Show(string message,Action action)
    {
        messageText.SetText(message);
        yesAction = action;
        gameObject.SetActive(true);
    }

    public void OnYesButton()
    {
        yesAction?.Invoke();
        gameObject.SetActive(false);
    }
}
