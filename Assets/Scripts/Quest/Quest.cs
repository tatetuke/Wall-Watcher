using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Quest : MonoBehaviour
{
    public enum QuestState
    {
        not_yet,
        doing,
        finish,
        error
    }
    QuestState m_state = QuestState.not_yet;
    [SerializeField] QuestDataSO questData;
    List<Quest> m_subQuests = new List<Quest>();
    public UnityEvent OnQuestStart = new UnityEvent();
    public UnityEvent OnQuestFinish = new UnityEvent();
    int m_currentPhase=0;
    public bool IsCurrentSubQuestFinished()
    {
        return false;
    }
    public bool IsQuestFinished()
    {
        return false;
    }
    public bool CheckStart()
    {
        foreach(var i in questData.startConditions)
        {
            if (GamePropertyManager.Instance.GetBoolProperty(i.parameterKey))
            {

            }
        }
        return false;
    }
    public bool CheckFinish()
    {
        return false;
    }
    public void ForceStart()
    {

    }
    private void Start()
    {
        foreach (var i in questData.subQuests)
        {
            var obj = new GameObject(i.name);
            var scr = obj.AddComponent<Quest>();
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
                    m_state = QuestState.doing;
                    OnQuestStart.Invoke();
                }
                break;
            case QuestState.doing:
                if (CheckFinish())
                {
                    m_state = QuestState.finish;
                    OnQuestFinish.Invoke();
                }
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
