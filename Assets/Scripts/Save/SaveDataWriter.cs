using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveDataWriter : SingletonMonoBehaviour<SaveDataWriter>
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
        string filePath =  m_fileNames[saveFileIndex];
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
        var moneyScr = FindObjectOfType<MoneyScript>();
        if (moneyScr == null)
        {
            Debug.LogError("money script not found");
            ans.money = 0;
        }
        else
        {
            ans.money = moneyScr.Money ;
        }
        ans.header.chapterCount = 0;
        ans.header.loopCount = 0;
        ans.header.fileName = "???";
        ans.roomName =SceneManager.GetActiveScene().name;
        var playerScr = FindObjectOfType<Player>();
        if (playerScr==null)
        {
            Debug.LogError("player script not found");
            ans.playerPosition = Vector3.zero;
        }
        else
        {
            ans.playerPosition = playerScr.transform.position;
        }
        var inventryScr = FindObjectOfType<Kyoichi.Inventry>();
        if (inventryScr == null)
        {
            Debug.LogError("inventry script not found");
        }
        else
        {
            foreach (var i in inventryScr.Data)
            {
                ans.inventry.Add(i);
            }
        }
        var questScr = FindObjectOfType<QuestHolder>();
        foreach(var i in questScr.Data)
        {
            ans.quests.Add(i.GetData());
        }

        return ans;
    }

}
