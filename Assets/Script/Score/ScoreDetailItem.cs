using UnityEngine;
public class ScoreDetailItem : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text ScoreNameText;
    [SerializeField] GameObject ScoreValueText;


    public void SetScoreDetailText(string name,int value,bool isNoValue)
    {
        ScoreNameText.SetText(name);
        ScoreValueText.GetComponent<TMPro.TMP_Text>().SetText(value.ToString());
        if (isNoValue)
        {
            ScoreValueText.SetActive(false);
        }
    }
}
