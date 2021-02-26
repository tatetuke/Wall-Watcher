using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    [System.Serializable]
    public class QuestCondition
    {
        public string dataKey;
        public string value;
        public bool MatchCondition()
        {
            return false;//SaveLoadManager.Instance.
        }
    }


    [System.Serializable]
    public class QuestPhase
    {
        public List<QuestCondition> startConditions = new List<QuestCondition>();
        public List<QuestCondition> finishConditions = new List<QuestCondition>();
        public UnityEvent OnPhaseStart = new UnityEvent();
        public UnityEvent OnPhaseFinish = new UnityEvent();
        public void CheckStart() { }
        public void CheckFinish() { }
    }

    public enum QuestState
    {
        not_yet,
        doing,
        finish,
        error
    }
    QuestState state = QuestState.not_yet;
    public List<QuestCondition> startConditions = new List<QuestCondition>();
    public List<QuestCondition> finishConditions = new List<QuestCondition>();

    public UnityEvent OnQuestStart = new UnityEvent();
    public UnityEvent OnQuestFinish = new UnityEvent();
    public void CheckStart() { }
    public void CheckFinish() { }
    List<QuestPhase> phases = new List<QuestPhase>();
    int currentPhase;

}
