using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// 最小限の情報
/// </summary>
public class SaveDataHeader
{
    public string fileName;
    public int loopCount;//周回数
    public int chapterCount;
}

[System.Serializable]
public class QuestSaveData
{
    public string questName;
    public int cuestChapter;
    public QuestChecker.QuestState state;
}

[System.Serializable]
/// <summary>
/// セーブデータの情報
/// </summary>
public class SaveData : SaveDataBase
{
    public SaveDataHeader header = new SaveDataHeader();
    public Vector3 playerPosition;
    public string roomName;
    public int money;
    public List<Kyoichi.ItemStack> inventry = new List<Kyoichi.ItemStack>();
    public List<QuestSaveData> quests = new List<QuestSaveData>();
}

public class SaveDataBase
{
    public override string ToString()
    {
        return $"{base.ToString()} {JsonUtility.ToJson(this)}";
    }
}
