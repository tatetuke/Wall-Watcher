using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.Save;

/// <summary>
/// SaveDataHolderなどをテストするためのオブジェクト
/// </summary>
public class SaveTest : MonoBehaviour
{
    public SaveData data;


    public void Start()
    {
        //List<ListElement> list = new List<ListElement>();

        //ListElement<int> a = new ListElement<int>(20);
        //ListElement<double> b = new ListElement<double>();
        //ListElement<string> c = new ListElement<string>();
        //ListElement<MoneyScript> d;

        //a.Value = 20;
        //b.Value = 3.5;
        //c.Value = "aaa";

        //Debug.Log(a.Value);
        //Debug.Log(b.Value);
        //Debug.Log(c.Value);

        //list.Add(a);
        //list.Add(b);
        //list.Add(c);

        //foreach(ListElement el in list)
        //{
        //    int p = el.GetValue<int>();


        //    if (el.CheckType(typeof(int)))
        //        x.Add((int)el.obj);
        //    if (el.CheckType(typeof(double)))
        //        y.Add((double)el.obj);
        //    if (el.CheckType(typeof(string)))
        //        z.Add((string)el.obj);
        //}

        DataBank bank = DataBank.Open();
        Debug.Log("DataBank.Open()");
        Debug.Log($"save path of bank is { bank.SavePath }");

        SaveData saveData = new SaveData()
        {
            header = new SaveDataHeader { loopCount=3,chapterCount=1},
            playerPosition = new Vector3(10, 0, 0),
            roomName = "room1",
            money = 999,
            inventry = new List<Kyoichi.ItemStack>(),
            quests = new List<QuestSaveData> { 
                new QuestSaveData { questName="quest1",cuestChapter=1,state=QuestChecker.QuestState.not_yet} 
            },
        };
        Debug.Log(saveData);

        bank.Store("player", saveData);
        Debug.Log("bank.Store()");

        bank.SaveAll();
        Debug.Log("bank.SaveAll()");

        SaveData storedData = new SaveData();
        Debug.Log("default savedata");
        Debug.Log(storedData);

        storedData = bank.Get<SaveData>("player");
        Debug.Log("bank.Get<SaveData>(\"player\")");
        Debug.Log(storedData);

        bank.Clear();
        Debug.Log("bank.Clear()");

        storedData = bank.Get<SaveData>("player");
        Debug.Log(storedData);

        bank.Load<SaveData>("player");
        Debug.Log("bank.Load()");

        storedData = bank.Get<SaveData>("player");
        Debug.Log(storedData);

        data = storedData;
    }

}

