using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// 最小限の情報
/// </summary>
public class SaveDataHeader
{
    public string fileName;
    public int loopCount;//周回数
    public int chapterCount;
}

/// <summary>
/// セーブデータの情報
/// </summary>
public class SaveData
{
    public SaveDataHeader header = new SaveDataHeader();
    public Vector3 playerPosition;
    public string roomName;
    public int money;
    public List<Kyoichi.ItemStack> inventry = new List<Kyoichi.ItemStack>();
}

/// <summary>
/// プレイヤーのセーブデータを読み込む
/// </summary>
public class SaveDataReader : SingletonMonoBehaviour<SaveDataReader>
{
    [SerializeField] string saveDirectoryPath="Data/Saves";
    List<string> m_fileNames = new List<string>();
    private void Awake()
    {
        try
        {
            string[] names = Directory.GetFiles(Application.dataPath +"/"+saveDirectoryPath, "*.csv",SearchOption.TopDirectoryOnly);
            m_fileNames.AddRange(names);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public int GetFileCount() { return m_fileNames.Count; }

    public SaveDataHeader GetFileHeader(int count)
    {
        if (count >= m_fileNames.Count)
        {
            Debug.LogError("file not found");
            return null;
        }
        var dat = new SaveDataHeader();
        int readCount = 0;
        string filePath =  m_fileNames[count];
        foreach (var line in File.ReadLines(filePath))
        {
            if (readCount >= 3) break;//すべてのヘッダー情報が読み取れたので抜ける
            var i = line.Split(',');
            switch (i[0])
            {
                case "fileName":
                    dat.fileName = i[1];
                    readCount++;
                    break;
                case "loopCount":
                    dat.loopCount = int.Parse(i[1]);
                    readCount++;
                    break;
                case "chapterNum":
                    dat.chapterCount = int.Parse(i[1]);
                    readCount++;
                    break;
            }
        }
        return dat;
    }
    public SaveData GetFileData(int count)
    {
        if (count >= m_fileNames.Count)
        {
            Debug.LogError("file not found");
            return null;
        }
        var dat = new SaveData();
        dat.header = GetFileHeader(count);

        string filePath = Application.dataPath + "/" + saveDirectoryPath + "/" + m_fileNames[count];
        foreach (var line in File.ReadLines(filePath))
        {
            var i = line.Split(',');
            switch (i[0])
            {
                case "playerPos":
                    float x = float.Parse(i[1]);
                    float y = float.Parse(i[2]);
                    float z = float.Parse(i[3]);
                    dat.playerPosition = new Vector3(x, y, z);
                    break;
                case "roomName":
                    dat.roomName = i[1];
                    break;
                case "money":
                    dat.money = int.Parse(i[1]);
                    break;
                case "inventryItem":
                    var item = Kyoichi.ItemManager.Instance.GetItem(i[1]);
                    dat.inventry.Add(new Kyoichi.ItemStack(item, int.Parse(i[2])));
                    break;
            }
        }
        return dat;
    }
    /// <summary>
    /// セーブデータがディレクトリ内に存在しているかどうか
    /// </summary>
    /// <returns></returns>
    public bool ExistsSaveFiles()
    {
        return m_fileNames.Count != 0;
    }
}
