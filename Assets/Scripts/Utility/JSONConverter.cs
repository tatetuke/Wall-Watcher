using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Serialization.cs
using System;

//https://kou-yeung.hatenablog.com/entry/2015/12/31/014611

//2019/10/27 動作確認済
//2019/20/24 動作確認済

// List<T>をJson化したい用
[System.Serializable]
public class JSONSerialization<T>
{
    [SerializeField]  List<T> target;
    public List<T> ToList() { return target; }

    public JSONSerialization(List<T> target)
    {
        this.target = target;
    }
}

[System.Serializable]
public class JSONContainer
{
    public string key;
    public int value;
    public JSONContainer(string key_, int value_) { key = key_; value = value_; }
}
// Dictionary<TKey, TValue>
[System.Serializable]
public class JSONSerialization : ISerializationCallbackReceiver
{

    public List<JSONContainer> containers = new List<JSONContainer>();

    Dictionary<string, int> target=new Dictionary<string, int>();
    public Dictionary<string, int> ToDictionary() { return target; }
    public JSONSerialization(Dictionary<string, int> target_) { target = target_; }

    public void OnBeforeSerialize()
    {
        foreach(var obj in target)
        {
            containers.Add(new JSONContainer(obj.Key, obj.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        target = new Dictionary<string, int>();
        foreach(var obj in containers)
        {
            target.Add(obj.key, obj.value);
        }
    }
}