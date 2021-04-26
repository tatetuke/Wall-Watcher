//https://qiita.com/tocoteron/items/b865edaa0e3018cb5e55
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;
using System.Text;

public class DataBank
{
    static DataBank instance = new DataBank();
    static Dictionary<string, object> bank = new Dictionary<string, object>();

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

    public bool IsEmpty()
    {
        return bank.Count == 0;
    }

    public bool ExistsKey(string key)
    {
        return bank.ContainsKey(key);
    }

    public void Store(string key, object obj)
    {
        bank[key] = obj;
    }

    public void Clear()
    {
        bank.Clear();
    }

    public void Remove(string key)
    {
        bank.Remove(key);
    }

    public DataType Get<DataType>(string key)
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

    public void SaveAll()
    {
        foreach (string key in bank.Keys)
        {
            Save(key);
        }
    }

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

    public bool Load<DataType>(string key)
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