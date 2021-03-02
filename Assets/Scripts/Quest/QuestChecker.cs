using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// クエストクラス
/// </summary>
public class QuestChecker : MonoBehaviour
{
    /// <summary>
    /// クエストの状態
    /// not_yet→working→
    /// subQuestRun→subQuestFinish→working→
    /// subQuestRun→subQuestFinish→working→...subQuestのぶんだけ繰り返し
    /// →finish
    /// となる
    /// </summary>
    public enum QuestState
    {
        not_yet,
        working,
        subQuestRun,
        subQuestFinish,
        finish,
        error
    }
    QuestState m_state = QuestState.not_yet;
    [SerializeField] QuestDataSO questData;
    List<QuestChecker> m_subQuests = new List<QuestChecker>();
    public UnityEvent OnQuestStart = new UnityEvent();
    public UnityEvent OnQuestFinish = new UnityEvent();
    int m_currentPhase=0;
    public bool IsQuestFinished()
    {
        return m_state==QuestState.finish;
    }
    public bool CheckStart()
    {
        foreach(var i in questData.startConditions)
        {
            switch (i.valueType)
            {
                case QuestConditions.ValueType.Int:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetIntProperty(i.parameterKey))) return false;
                    break;
                case QuestConditions.ValueType.Float:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetFloatProperty(i.parameterKey))) return false;
                    break;
                case QuestConditions.ValueType.String:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetStringProperty(i.parameterKey))) return false;
                    break;
                case QuestConditions.ValueType.Boolean:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetBoolProperty(i.parameterKey))) return false;
                    break;
            }
        }
        return true;
    }
    public bool CheckFinish()
    {
        foreach (var i in questData.endConditions)
        {
            switch (i.valueType)
            {
                case QuestConditions.ValueType.Int:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetIntProperty(i.parameterKey))) return false;
                    break;
                case QuestConditions.ValueType.Float:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetFloatProperty(i.parameterKey))) return false;
                    break;
                case QuestConditions.ValueType.String:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetStringProperty(i.parameterKey))) return false;
                    break;
                case QuestConditions.ValueType.Boolean:
                    if (!i.MeetCondition(GamePropertyManager.Instance.GetBoolProperty(i.parameterKey))) return false;
                    break;
            }
        }
        return true;
    }
    /// <summary>
    /// 強制的にイベントを発生させる
    /// プレイヤーを所定の場所に移動させ、会話を強制再生
    /// </summary>
    public void ForceStart()
    {

    }
    private void Start()
    {
        foreach (var i in questData.subQuests)
        {
            var obj = new GameObject(i.name);
            var scr = obj.AddComponent<QuestChecker>();
            scr.questData = i;
            obj.transform.parent = transform;
            m_subQuests.Add(scr);
        }
    }
    private void Update()
    {
        switch (m_state)
        {
            case QuestState.not_yet:
                if (CheckStart())
                {
                    m_state = QuestState.working;
                    OnQuestStart.Invoke();
                }
                break;
            case QuestState.working:
                if (m_subQuests.Count == 0 )
                {
                    if (CheckFinish())
                    {
                        OnQuestFinish.Invoke();
                        m_state = QuestState.finish;
                    }
                }
                else
                {
                    if (m_currentPhase < m_subQuests.Count && m_subQuests[m_currentPhase].CheckStart())
                    {
                        m_state = QuestState.subQuestRun;
                    }
                }
                break;
            case QuestState.subQuestRun:
                if (m_subQuests[m_currentPhase].IsQuestFinished())
                {
                    m_currentPhase++;
                    m_state = QuestState.subQuestFinish;
                }
                break;
            case QuestState.subQuestFinish:
                    m_state = QuestState.working;
                break;
            case QuestState.finish:
                break;
            case QuestState.error:
                break;
            default:
                break;
        }
    }
}
