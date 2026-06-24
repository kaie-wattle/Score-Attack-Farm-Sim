using UnityEngine;

public class NoticePopup : PopupBase<NoticePopup>
{
    public void Show(string message)
    {
        messageText.SetText(message);
        window.SetActive(true);
    }
}
