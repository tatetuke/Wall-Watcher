//https://qiita.com/tocoteron/items/b865edaa0e3018cb5e55
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

// 保管されるものを object型から SaveDataBase にした
// ToString()のoverrideし忘れを防ぐため

/// <summary>
/// ローカルへの書き出し・読みだしを管理する
/// </summary>
public class DataBank
{
    static DataBank instance = new DataBank();
    static Dictionary<string, SaveDataBaseClass> bank = new Dictionary<string, SaveDataBaseClass>();

    static readonly string path = "SaveData";
    //static readonly string fullPath = $"{ Application.persistentDataPath }/{ path }";
    static readonly string fullPath = $"{ Application.persistentDataPath }/{ path }";
    static readonly string extension = "dat";

    public string SavePath
    {
        get
        {
            return fullPath;
        }
    }

    DataBank() { }

    public static DataBank Open()
    {
        return instance;
    }

    public static DataBank Instance
    {
        get { return instance; }
    }

    public bool IsEmpty()
    {
        return bank.Count == 0;
    }

    public bool ExistsKey(string key)
    {
        return bank.ContainsKey(key);
    }

    /// <summary>データを一時保存する</summary>
    public void Store(string key, SaveDataBaseClass obj)
    {
        bank[key] = obj;
    }

    /// <summary>一時保存されているデータを全て削除する</summary>
    public void Clear()
    {
        bank.Clear();
    }

    /// <summary>一時保存されているデータを削除する</summary>
    public void Remove(string key)
    {
        bank.Remove(key);
    }

    /// <summary>一時保存されているデータを取得する</summary>
    public DataType Get<DataType>(string key)
        where DataType: SaveDataBaseClass
    {
        if (ExistsKey(key))
        {
            return (DataType)bank[key];
        }
        else
        {
            return default(DataType);
        }
    }

    /// <summary>一時保存されているデータを全てファイルへ書き出す</summary>
    public void SaveAll()
    {
        foreach (string key in bank.Keys)
        {
            Save(key);
        }
    }

    /// <summary>一時保存されているデータをファイルへ書き出す</summary>
    public bool Save(string key)
    {
        if (!ExistsKey(key))
        {
            return false;
        }

        string filePath = $"{ fullPath }/{ key }.{ extension }";

        string json = JsonUtility.ToJson(bank[key]);

        byte[] data = Encoding.UTF8.GetBytes(json);
        //data = Compressor.Compress(data);
        //data = Cryptor.Encrypt(data);

        if (!Directory.Exists(fullPath))
        {
            Directory.CreateDirectory(fullPath);
        }

        using (FileStream fileStream = File.Create(filePath))
        {
            fileStream.Write(data, 0, data.Length);
        }

        return true;
    }

    /// <summary>ファイルからデータを読み込み一時保存する</summary>
    public bool Load<DataType>(string key)
        where DataType : SaveDataBaseClass
    {
        string filePath = $"{ fullPath }/{ key }.{ extension }";

        if (!File.Exists(filePath))
        {
            return false;
        }

        byte[] data = null;
        using (FileStream fileStream = File.OpenRead(filePath))
        {
            data = new byte[fileStream.Length];
            fileStream.Read(data, 0, data.Length);
        }

        //data = Cryptor.Decrypt(data);
        //data = Compressor.Decompress(data);

        string json = Encoding.UTF8.GetString(data);

        bank[key] = JsonUtility.FromJson<DataType>(json);

        return true;
    }
}