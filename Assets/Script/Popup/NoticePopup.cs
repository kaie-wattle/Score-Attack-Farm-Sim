using UnityEngine;

public class NoticePopup : PopupBase<NoticePopup>
{
    [SerializeField] TMPro.TMP_Text messageText;

    public void Show(string message)
    {
        messageText.SetText(message);
        gameObject.SetActive(true);
    }
}
