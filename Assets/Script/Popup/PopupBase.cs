using UnityEngine;

public abstract class PopupBase<T> : MonoBehaviour where T:MonoBehaviour
{
    public static T instance { get; private set; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this as T;
    }

    public void OnCloseButton()
    {
        gameObject.SetActive(false);
    }
}
