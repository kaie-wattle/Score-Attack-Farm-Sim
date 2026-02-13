using UnityEngine;

/// <summary>
/// 維持費管理クラス
/// 維持費は以下で考える
/// 保有している土地の面積+作物を植えている面積+保有している種子の合計
/// ただし種子の数は一定数までなら維持費はかからない
/// </summary>
public class MaintenanceManager : MonoBehaviour
{
    /// <summary> 保有面積のコスト </summary>
    [SerializeField] int fieldCost = 5;
    /// <summary> 作物を植えている面積のコスト </summary>
    [SerializeField] int plantedCost = 10;
    /// <summary> 維持費がかからない種子の数 </summary>
    [SerializeField] int freeSeedCount = 50;
    /// <summary> 種子1つ当たりのコスト </summary>
    [SerializeField] int SeedCost = 5;

    /// <summary>
    /// 維持費計算
    /// </summary>
    /// <param name="fieldCount">保有土地数</param>
    /// <param name="plantedFieldCount">作物が植えられている土地数</param>
    /// <param name="seedCount">保有している種子数</param>
    /// <returns></returns>
    public int CalcCost(int fieldCount,int plantedFieldCount,int seedCount)
    {
        int ret = 0;
        // 保有面積のコスト
        ret += fieldCount* fieldCost;
        // 作物を植えている面積のコスト
        ret += plantedFieldCount * plantedCost;
        // 保有種子のコスト
        ret += Mathf.Max(0, seedCount - freeSeedCount) * SeedCost;
        return ret;
    }
}
