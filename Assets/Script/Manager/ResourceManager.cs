using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceManager : MonoBehaviour
{
    [SerializeField] int ColMax;
    [SerializeField] int RowMax;
    public static ResourceManager Instance { get; private set; }

    public bool IsNeverDebt => isNeverDebt;
    public bool IsFieldMax => isFieldMax;
    public event UnityAction OnMoneyChanged;
    public event UnityAction<SO_CropDefinition> OnSeedInventoryChanged;


    /// <summary> 所持金 </summary>
    public int Money { get; private set; }
    /// <summary> 種子保有数 </summary>
    private Dictionary<SO_CropDefinition, int> seedInventory = new Dictionary<SO_CropDefinition, int>();
    /// <summary> 耕地面積 </summary>
    public int FiledCount { get; private set; }

    private bool isNeverDebt;
    private bool isFieldMax;

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(int value)
    {
        Money += value;

        if (Money < 0)
            isNeverDebt = false;

        OnMoneyChanged?.Invoke();
    }

    public void AddSeed(SO_CropDefinition cropDef, int value)
    {
        if (!seedInventory.ContainsKey(cropDef))
        {
            seedInventory[cropDef] = 0;
        }
        seedInventory[cropDef] += value;
        OnSeedInventoryChanged?.Invoke(cropDef);
    }

    public int GetSeedCount(SO_CropDefinition cropDef)
    {
        return seedInventory.ContainsKey(cropDef) ? seedInventory[cropDef] : 0;
    }

    public void AddField()
    {
        if (FiledCount >= ColMax * RowMax)
        {
            Debug.Log("もう置けません");
        }
        else
        {
            FiledCount++;
            if (FiledCount >= ColMax * RowMax)
            {
                isFieldMax = true;
            }
        }
    }

    /// <summary>
    /// TODO:デバッグ用
    /// </summary>
    private void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.label);
        myStyle.fontSize = 30; // フォントサイズを指定
        myStyle.normal.textColor = Color.white; // 色も変更可能

        GUILayout.Label("Money:" + Money.ToString(), myStyle);
        foreach (var pair in seedInventory)
        {
            GUILayout.Label(pair.Key.name + " : " + pair.Value, myStyle);
        }
    }
}
