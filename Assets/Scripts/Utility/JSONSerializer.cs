using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

//http://kan-kikuchi.hatenablog.com/entry/Dictionary-JSON
//https://qiita.com/tetr4lab/items/134fc7295a07076d1625

/// <summary>
/// DictionaryデータとJsonデータ(string)の変換を行う
/// 暗号化と複合化も同時に行う
/// </summary>
public static class JsonSerializer
{

    //保存するディレクトリ名
    private const string DIRECTORY_NAME = "Data";

    //ファイルのパスを取得
    private static string GetFilePath(string fileName)
    {

        string directoryPath = Application.dataPath + "/" + DIRECTORY_NAME;

        //ディレクトリが無ければ作成
        if (!Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        //ファイル名は暗号化する
        string filePath = directoryPath + "/" + fileName;

        return filePath;
    }

    /// <summary>
    /// Dictionaryデータをjson形式に変換して保存する
    /// </summary>
    /// <param name="dic">保存するDictionary<string, object>データ</param>
    /// <param name="fileName">保存ファイル名</param>
    public static void Save(Dictionary<string, int> dic, string fileName)
    {
        string jsonStr = JsonUtility.ToJson(new JSONSerialization(dic), true);
        Debug.Log("serialized text = " + jsonStr);

        string filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError(fileName + "はありません！");
            return;
        }

        File.WriteAllText(filePath, jsonStr);

        Debug.Log("saveFilePath = " + filePath);
    }

    /// <summary>
    /// jsonデータを読み込みDictionaryデータに変換して返す
    /// </summary>
    /// <param name="fileName">取得するファイルの名前</param>
    public static Dictionary<string, int> Load(string fileName)
    {

        string filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError(fileName + "はありません！");
            return null;
        }

        string jsonStr = File.ReadAllText(filePath);
        var data = JsonUtility.FromJson<JSONSerialization>(jsonStr);
        if (data == null)
        {
            Debug.LogError("Data is broken");
            return new Dictionary<string, int>();
        }
        Dictionary<string, int> dic = data.ToDictionary();
        return dic;
    }

    public static void ClearData(string fileName)
    {
        string filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError(fileName + "はありません！");
            return;
        }
        File.WriteAllText(filePath, "");
    }

    [System.Serializable]
    public class JSONTest
    {
        public List<string> keys;
        public List<int> values;

        public JSONTest(Dictionary<string,int> dic)
        {
            keys = new List<string>();
            values = new List<int>();
            foreach(var i in dic)
            {
                keys.Add(i.Key);
                values.Add(i.Value);
            }
        }
    }

   /* public static void Save(Dictionary<string, Dictionary<string, int>> dic, string fileName)
    {
        var d = new Dictionary<string, JSONTest>();
        foreach(var i in dic)
        {
            d.Add(i.Key, new JSONTest(i.Value));
        }
        string jsonStr = JsonUtility.ToJson(new JSONSerialization<string, JSONTest>(d));
        Debug.Log("serialized text = " + jsonStr);

        string filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError(fileName + "はありません！");
            return;
        }

        File.WriteAllText(filePath, jsonStr);

        Debug.Log("saveFilePath = " + filePath);
    }*/

   /* public static Dictionary<string, Dictionary<string, int>> Load2(string fileName)
    {

        string filePath = GetFilePath(fileName);
        if (!File.Exists(filePath))
        {
            Debug.LogError(fileName + "はありません！");
            return null;
        }

        string jsonStr = File.ReadAllText(filePath);
        var data = JsonUtility.FromJson<JSONSerialization<string, JSONTest>>(jsonStr);
        if (data == null)
        {
            Debug.LogError("Data is broken");
            return new Dictionary<string, Dictionary<string, int>>();
        }
        var a = data.ToDictionary();

        Dictionary<string, Dictionary<string, int>> dic=new Dictionary<string, Dictionary<string, int>>();
        foreach(var i in a)
        {
            Dictionary<string, int> b=new Dictionary<string, int>();
            int count = i.Value.keys.Count;
            for(int j = 0; j < count; j++)
            {
                b.Add(i.Value.keys[j],i.Value.values[j]);
            }
            dic.Add(i.Key, b);
        }
        return dic;
    }*/
}