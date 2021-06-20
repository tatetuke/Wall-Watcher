using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// セーブ・ロードを一括で行う
/// keyの重複検知も行う
/// </summary>
public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
    [ReadOnly, SerializeField] private string path;
    private Dictionary<string, bool> usedKeyDictionary = new Dictionary<string, bool>();

    private UnityEvent SaveCallBack = new UnityEvent();
    private UnityEvent LoadCallBack = new UnityEvent();

    List<UniTask> saveAsync = new List<UniTask>();
    List<UniTask> loadAsync = new List<UniTask>();

    private void Awake()
    {
        if (SaveLoadManager.Instance != this)
            Destroy(this);

        path = DataBank.Instance.SavePath;
    }

    public async UniTask SaveAllAsync()
    {
        SaveCallBack.Invoke();
        foreach(var save_ in saveAsync)
        {
           await save_;
        }
    }

    public async UniTask LoadAllAsync()
    {
        LoadCallBack.Invoke();
        foreach (var load_ in loadAsync)
        {
            await load_;
        }
    }

    public void AddSaveCallBack(UniTask save)=>saveAsync.Add(save);
    public void AddLoadCallBack(UniTask load) => loadAsync.Add(load);
    public void AddSaveCallBack(UnityAction save)=>LoadCallBack.AddListener(save);
    public void AddLoadCallBack(UnityAction load)=> LoadCallBack.AddListener(load);

    /// <summary>指定したkeyが新しく使用可能(重複していない)であればtrueを返す</summary>
    public bool CheckKeyAvailable(string key)
    {
        return !usedKeyDictionary.ContainsKey(key);
    }

    /// <summary>指定したkeyが全て新しく使用可能(重複していない)であればtrueを返す</summary>
    public bool CheckKeyListAvailable(List<string> keys)
    {
        if (keys == null) return true;
        foreach(string key in keys)
        {
            if (!CheckKeyAvailable(key))
                return false;
        }
        //全て重複なし
        return true;
    }

    public void AddKey(string key)
    {
        usedKeyDictionary.Add(key, true);
    }

    public void AddKeyList(List<string> keys)
    {
        if (keys == null) return;
        foreach (string key in keys)
            AddKey(key);
    }

    public void Clear()
    {
        usedKeyDictionary.Clear();
        SaveCallBack.RemoveAllListeners();
        LoadCallBack.RemoveAllListeners();
    }

    public void ShowAllKeys()
    {
        foreach(var pair in usedKeyDictionary)
        {
            Debug.Log(pair.Key);
        }
    }
}
