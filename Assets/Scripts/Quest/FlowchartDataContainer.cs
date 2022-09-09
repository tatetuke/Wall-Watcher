using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Fungus;

/// <summary>
/// FlowchartDataをSave Load用に保管する
/// クエストごとのフラグ管理庫
/// GamePropertyManagerへの登録もする
/// </summary>
public class FlowchartDataContainer : SaveLoadableSingletonMonoBehaviour<FlowchartDataContainer>
{
    private Dictionary<string, FlowchartData> m_flowchartDataDictionary = new Dictionary<string, FlowchartData>();
    [SerializeField]private string m_savekey = "FlowchartData";

    public int money = 0;
    public int pollutionDegree = 0;
    public string playerName = "ring";
    public bool getEnding = false;

    public void Store(string key, FlowchartData flowchartData)
    {
        m_flowchartDataDictionary.Add(key, flowchartData);
    }

    public FlowchartData Get(string key)
    {
        //なかった場合どうしようか
        return m_flowchartDataDictionary[key];
    }

    public void Clear()
    {
        m_flowchartDataDictionary.Clear();
    }

    /// <summary>SaveLoadManager用</summary>
    protected override void Save()
    {
        SaveData_FlowchartDataContainer savedata = new SaveData_FlowchartDataContainer();
        savedata.data = new List<KeyValuePair<string, FlowchartData>>(m_flowchartDataDictionary.Count);

        //辞書からListへ変換
        int i = 0;
        foreach(var pair in m_flowchartDataDictionary) {

            savedata.data[i] = new KeyValuePair<string, FlowchartData>(pair.Key,pair.Value);
            i++;
        }
        
        DataBank.Instance.Store(m_savekey, savedata);
        DataBank.Instance.Save(m_savekey);
    }


    /// <summary>SaveLoadManager用</summary>
    protected override void Load()
    {
        DataBank.Instance.Load<SaveData_FlowchartDataContainer>(m_savekey);
        var savedata = DataBank.Instance.Get<SaveData_FlowchartDataContainer>(m_savekey);

        //辞書に追加
        foreach(var item in savedata.data)
        {
            m_flowchartDataDictionary.Add(item.Key, item.Value);
        }
    }

    /// <summary>SaveLoadManager用</summary>
    protected override List<string> GetKeyList()
    {
        return null;
    }

    protected override void Awake()
    {
        //base.Awake()を忘れない SaveLoadManagerでの重複チェック・登録が行われる
        base.Awake();

        Debug.Log("aa");
        GamePropertyManager.Instance.RegisterParam("_Money", () => money);
        Debug.Log("bb");
        GamePropertyManager.Instance.RegisterParam("_PollutionDegree", () => pollutionDegree);
        GamePropertyManager.Instance.RegisterParam("_PlayerName", () => playerName);
        GamePropertyManager.Instance.RegisterParam("_GetEnding", () => getEnding);
        Debug.Log("_Money: " + GamePropertyManager.Instance.GetProperty<int>("_Money"));
    }

    /// <summary>SaveLoadManager用</summary>
    protected async override UniTask SaveAsync()
    {
        //throw new NotImplementedException();
    }

    /// <summary>SaveLoadManager用</summary>
    protected async override UniTask LoadAsync()
    {
        //throw new NotImplementedException();
    }



    /// <summary>
    /// Flowchartの変数をGamePropertyManagerへ登録する TODO
    /// </summary>
    public class FlowchartVariablesRegister_GameProperty
    {


    }
}

/// <summary>
/// m_flowchartDataDictionary をセーブするためのクラス
/// </summary>
public class SaveData_FlowchartDataContainer:SaveDataBaseClass
{
    public List<KeyValuePair<string, FlowchartData>> data = new List<KeyValuePair<string, FlowchartData>>();
}

