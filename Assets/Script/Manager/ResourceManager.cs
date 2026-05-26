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
    public bool IsLivestockAreaMax => isLivestockAreaMax;
    public event UnityAction OnMoneyChanged;
    public event UnityAction<SO_CropDefinition> OnSeedInventoryChanged;
    public event UnityAction<SO_LivestockDefinition,int> OnLivestockInventoryChanged;
    public event UnityAction OnFieldCountChanged;
    public event UnityAction OnLivestockAreaCountChanged;

    /// <summary> 所持金 </summary>
    public int Money { get; private set; }
    /// <summary> 種子保有数 </summary>
    private Dictionary<SO_CropDefinition, int> seedInventory = new Dictionary<SO_CropDefinition, int>();
    /// <summary> 家畜保有数 </summary>
    private Dictionary<SO_LivestockDefinition, int> livestockInventory = new Dictionary<SO_LivestockDefinition, int>();
    /// <summary> 耕地面積 </summary>
    public int FieldCount { get; private set; }
    /// <summary> 畜産面積 </summary>
    public int LivestockAreaCount { get; private set; }

    private bool isNeverDebt;
    private bool isFieldMax;
    private bool isLivestockAreaMax;

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

    /// <summary>
    /// 所持金更新
    /// </summary>
    /// <param name="value"></param>
    public void AddMoney(int value)
    {
        Money += value;

        if (Money < 0)
            isNeverDebt = false;

        OnMoneyChanged?.Invoke();
    }

    /// <summary>
    /// 保有種子更新
    /// </summary>
    /// <param name="cropDef"></param>
    /// <param name="value"></param>
    public void AddSeed(SO_CropDefinition cropDef, int value)
    {
        if (!seedInventory.ContainsKey(cropDef))
        {
            seedInventory[cropDef] = 0;
        }
        seedInventory[cropDef] += value;
        OnSeedInventoryChanged?.Invoke(cropDef);
    }

    /// <summary>
    /// 保有種子数取得
    /// </summary>
    /// <param name="cropDef"></param>
    /// <returns></returns>
    public int GetSeedCount(SO_CropDefinition cropDef)
    {
        return seedInventory.ContainsKey(cropDef) ? seedInventory[cropDef] : 0;
    }

    /// <summary>
    /// 総保有種子数取得
    /// </summary>
    /// <returns></returns>
    public int GetAllSeedCount()
    {
        int count = 0;
        foreach(var value in seedInventory.Values)
        {
            count += value;
        }
        return count;
    }

    /// <summary>
    /// 保有家畜更新
    /// </summary>
    /// <param name="livestockDef"></param>
    /// <param name="value"></param>
    public void AddLivestock(SO_LivestockDefinition livestockDef, int value)
    {
        if (!livestockInventory.ContainsKey(livestockDef))
        {
            livestockInventory[livestockDef] = 0;
        }
        livestockInventory[livestockDef] += value;
        OnLivestockInventoryChanged?.Invoke(livestockDef, value);
    }

    /// <summary>
    /// 保有家畜数取得
    /// </summary>
    /// <param name="livestockDef"></param>
    /// <returns></returns>
    public int GetLivestockCount(SO_LivestockDefinition livestockDef)
    {
        return livestockInventory.ContainsKey(livestockDef) ? livestockInventory[livestockDef] : 0;
    }

    /// <summary>
    /// 総保有家畜数取得
    /// </summary>
    /// <returns></returns>
    public int GetAllLivestockCount()
    {
        int count = 0;
        foreach (var value in livestockInventory.Values)
        {
            count += value;
        }
        return count;
    }

    /// <summary>
    /// 農地拡張
    /// </summary>
    /// <param name="value"></param>
    public void AddField(int value)
    {
        if (FieldCount >= ColMax * RowMax)
        {
            Debug.Log("もう置けません");
        }
        else
        {
            for (int i = 0; i < value; i++)
            {
                OnFieldCountChanged?.Invoke();
                FieldCount++;
                if (FieldCount >= ColMax * RowMax)
                {
                    isFieldMax = true;
                }
            }
        }
    }

    /// <summary>
    /// 畜産面積拡張
    /// </summary>
    /// <param name="value"></param>
    public void AddLivestockArea(int value)
    {
        if (LivestockAreaCount >= ColMax * RowMax)
        {
            Debug.Log("もう置けません");
        }
        else
        {
            for (int i = 0; i < value; i++)
            {
                OnLivestockAreaCountChanged?.Invoke();
                LivestockAreaCount++;
                if (LivestockAreaCount >= ColMax * RowMax)
                {
                    isLivestockAreaMax = true;
                }
            }
        }
    }

    /// <summary>
    /// エリア最大数取得
    /// </summary>
    /// <returns></returns>
    public int GetAreaMax()
    {
        return ColMax * RowMax;
    }





    /// <summary> 空畜産面積 </summary>
    public int FreeLivestockAreaCount { get; set; }
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
        GUILayout.Label("AllSeedCount:" + GetAllSeedCount().ToString(), myStyle);

        foreach (var pair in livestockInventory)
        {
            GUILayout.Label(pair.Key.name + " : " + pair.Value, myStyle);
        }
        GUILayout.Label("AllLivestockCount:" + GetAllLivestockCount().ToString(), myStyle);
        GUILayout.Label("FreeLivestockAreaCount:" + FreeLivestockAreaCount.ToString(), myStyle);
    }
}
