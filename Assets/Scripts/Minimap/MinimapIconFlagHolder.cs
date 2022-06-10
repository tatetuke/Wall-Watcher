using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// ミニマップに表示される MinimapIcon の名前を保存する
/// シーンをまたいで保存したいデータを Minimap から保存
/// Minimap 以外からは呼び出さないはず
/// </summary>
public class MinimapIconFlagHolder : SingletonMonoBehaviour<MinimapIconFlagHolder>
{
    // ミニマップに表示されるミニマップアイコンの名前
    private Dictionary<string, bool> visibleMinimapIconDictionary = new Dictionary<string, bool>();

    [Header("デバッグ用")]
    // テスト用　visibleMinimapIconDictionary の初期化　
    [SerializeField] private List<string> testMinimapIconNameList = new List<string>();

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        Test();
    }

    public void AddVisibleMinimapIcon(string iconName)
    {
        OperateVisibleMinimapIcon(iconName, true);
    }
    public void RemoveVisibleMinimapIcon(string iconName)
    {
        OperateVisibleMinimapIcon(iconName, false);
    }

    public bool ContainsKey(string iconName)
    {
        return visibleMinimapIconDictionary.ContainsKey(iconName);
    }

    /// <summary>
    /// ミニマップの可視化アイコンリストに追加または削除をする
    /// </summary>
    /// <param name="iconName"></param>
    /// <param name="value">trueなら追加、falseなら削除</param>
    public void OperateVisibleMinimapIcon(string iconName, bool value)
    {
        if (value)
        {
            //visibleMinimapIconDictionary.Add(iconName, true);
            visibleMinimapIconDictionary[iconName] = true;
        }
        else
        {
            if (visibleMinimapIconDictionary.ContainsKey(iconName))
                visibleMinimapIconDictionary.Remove(iconName);
        }
    }

    public void Clear()
    {
        visibleMinimapIconDictionary.Clear();
    }

    private void Test()
    {
#if UNITY_EDITOR
        foreach(var iconName in testMinimapIconNameList)
        {
            AddVisibleMinimapIcon(iconName);
        }
#endif
    }

}