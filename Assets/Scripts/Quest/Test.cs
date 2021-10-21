using UnityEngine;
using Fungus;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;

class Test : MonoBehaviour
{
    public int value1;
    public string questName;
    public Flowchart flowchart;
    public FlowchartData flowchartData;
    [SerializeField, ReadOnly] QuestDataSO m_questDataSO;

    [Button]
    public void EncodeFlowchart()
    {
        flowchartData = FlowchartData.Encode(flowchart);
    }

    [Button]
    public void DecodeFlowchart()
    {
        FlowchartData.Decode(flowchartData);
    }

    [Button]
    public void AddQuest()
    {
        QuestHolder questHolder = FindObjectOfType<QuestHolder>();
        QuestDataSO questDataSO = QuestsManager.Instance.GetQuest(questName);
        m_questDataSO = questDataSO;
        if (questDataSO == null)
        {
            Debug.Log("nullllll");
        }
        else
        {
            questHolder.AddQuest(questDataSO);
        }
    }

    public int GetMoney()
    {
        return 10;
    }

    public string GetQuestName()
    {
        return questName;
    }

}

