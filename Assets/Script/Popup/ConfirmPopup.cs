using System;
using UnityEngine;

public class ConfirmPopup : PopupBase<ConfirmPopup>
{
    Action yesAction;

    public void Show(string message,Action action)
    {
        messageText.SetText(message);
        yesAction = action;
        window.SetActive(true);
    }

    public void OnYesButton()
    {
        yesAction?.Invoke();
        window.SetActive(false);
    }
}
