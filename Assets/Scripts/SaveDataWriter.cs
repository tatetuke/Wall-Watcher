using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataWriter : MonoBehaviour
{
    [SerializeField] string saveDirectoryPath="Data/Saves";
    List<string> m_fileNames = new List<string>();
    private void Awake()
    {
        try
        {
            string[] names = Directory.GetFiles(Application.dataPath + "/" + saveDirectoryPath, "*.csv", SearchOption.TopDirectoryOnly);
            m_fileNames.AddRange(names);
        }
        catch (System.Exception e)
        {
            Debug.Log(e.Message);
        }
    }
    /// <summary>
    /// データを指定したセーブデータに保存する
    /// </summary>
    /// <param name="saveFileIndex"></param>
    public void Save(int saveFileIndex)
    {
        string filePath = Application.dataPath + "/" + saveDirectoryPath + "/" + m_fileNames[saveFileIndex];
        var dat = Convert();
        if (!File.Exists(filePath))
        {
            using (File.Create(filePath))
            {

            }
        }
        FileInfo fi = new FileInfo(filePath);
        using (StreamWriter sw = fi.CreateText())
        {
            sw.WriteLine("fileName" + "," + dat.header.fileName);
            sw.Flush();
            sw.WriteLine("loopCount" + "," + dat.header.loopCount);
            sw.Flush();
            sw.WriteLine("chapterNum" + "," + dat.header.chapterCount);
            sw.Flush();
            sw.WriteLine("playerPos" + "," + dat.playerPosition.x + "," + dat.playerPosition.y + "," + dat.playerPosition.z);
            sw.Flush();
            sw.WriteLine("roomName" + "," + dat.roomName);
            sw.Flush();
            sw.WriteLine("money" + "," + dat.money);
            sw.Flush();

            foreach (var obj in dat.inventry)
            {
                sw.WriteLine("inventryItem" + "," + obj.item.name + "," + obj.count);
                sw.Flush();
            }
        }
    }

    /// <summary>
    /// UnityのScene内にあるデータをかき集めてセーブデータに変換
    /// </summary>
    /// <returns></returns>
    SaveData Convert()
    {
        var ans = new SaveData();
        ans.money = FindObjectOfType<MoneyScript>().Money;
        ans.header.chapterCount = 0;
        ans.header.loopCount = 0;
        ans.header.fileName = "???";
        ans.roomName =SceneManager.GetActiveScene().name;
        ans.playerPosition = FindObjectOfType<Player>().transform.position;
        var inventry = FindObjectOfType<Kyoichi.Inventry>();
        foreach(var i in inventry.Data)
        {
            ans.inventry.Add(i);
        }
        return ans;
    }

}
