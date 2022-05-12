using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuestSaveData
{
    public string questName;
    public int cuestChapter;
    public QuestChecker.QuestState state;
}

public class SaveDataBaseClass
{
    public string ToData()
    {
        return $"{base.ToString()} {JsonUtility.ToJson(this)}";
    }
}

[System.Serializable]
/// <summary>
/// セーブデータの情報
/// </summary>
public class SaveData : SaveDataBaseClass
{
    public string fileName;
    public int loopCount;//周回数
    public int chapterCount;
    public Vector3 playerPosition;
    public string roomName;
    public int money;
    public List<Kyoichi.ItemStack> inventry = new List<Kyoichi.ItemStack>();
    public List<QuestSaveData> quests = new List<QuestSaveData>();
}
