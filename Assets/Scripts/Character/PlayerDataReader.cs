using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// プレイヤーについてのセーブデータを読み書き
/// </summary>
public class PlayerDataReader : MonoBehaviour
{
    [SerializeField] string directoryPath;
    [SerializeField] string fileName="playerData.csv";

    public void Load()
    {
        var dat=CSVReader.Read(directoryPath, fileName);
        foreach(var i in dat)
        {
            switch (i[0])
            {
                case "quest":
                    break;
                case "item":
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// プレイヤーが請け負っているクエストを返す
    /// </summary>
    /// <returns></returns>
    public List<QuestDataSO> GetQuests()
    {
        return null;
    }

}
