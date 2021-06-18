using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fungus;

[CreateAssetMenu(fileName = "NewQuest", menuName = "ScriptableObject/Quest")]
public class QuestDataSO : ScriptableObject
{
    public string title;//クエストの題名
    public string description;//クエストの詳しい説明
    public List<QuestDataSO> subQuests = new List<QuestDataSO>();
    public List<QuestConditions> endConditions = new List<QuestConditions>();
    public Flowchart flowchart;

    /// <summary>
    /// endConditionsのうち、どれか一つでも成り立っていればOK（OR条件）
    /// </summary>
    /// <returns></returns>
    public bool MeetEndCondition()
    {
        foreach(var i in endConditions)
        {
            if (i.MeetConditions()) return true;
        }
        return false;
    }
}
