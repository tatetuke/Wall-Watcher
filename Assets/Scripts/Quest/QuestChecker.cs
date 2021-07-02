using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// クエストのゲーム内での進行を制御するクラス
/// クエストを受注したらこのクラスを生成し、終了条件を逐次チェックする
/// </summary>
public class QuestChecker : MonoBehaviour
{
    /// <summary>
    /// クエストの状態
    /// not_yet→working→
    /// →finish
    /// となる
    /// </summary>
    public enum QuestState
    {
        not_yet,
        working,
        finish,
        error
    }
    [SerializeField, ReadOnly] QuestDataSO m_quest;
    [SerializeField,ReadOnly]QuestState m_state = QuestState.not_yet;
    [SerializeField, ReadOnly] int m_currentPhase = 0;
    [SerializeField]public Fungus.Flowchart m_Flowchart;

    public QuestSaveData GetData()
    {
        var dat = new QuestSaveData();
        dat.cuestChapter = m_currentPhase;
        dat.questName = m_quest.name;
        dat.state = m_state;
        return dat;
    }

    bool m_isSubQuest = false;

    List<QuestChecker> m_subQuests = new List<QuestChecker>();
    public UnityEvent OnQuestStart { get; } = new UnityEvent();
    public UnityEvent OnQuestFinish { get; } = new UnityEvent();

    private void Awake()
    {
        m_currentPhase = 0;
        foreach(var i in m_quest.subQuests)
        {
            var checker = new QuestChecker();
            checker.Initialize(i, QuestState.not_yet, 0);
            checker.m_isSubQuest = true;
            checker.gameObject.transform.parent = transform;
            m_subQuests.Add(checker);
        }
    }

    private void Start()
    {

    }

    public bool HasSubQuest() { return m_subQuests.Count != 0; }

    public void Initialize(QuestDataSO quest, QuestState state,int chapter)
    {
        m_quest = quest;
        m_state = state;
        m_currentPhase = chapter;
    }

    private void Update()
    {
        switch (m_state)
        {
            case QuestState.not_yet://サブクエストでまだアクティブになってなかったら何もしない
                break;
            case QuestState.working://受注している状態。終了できるかチェック
                if (HasSubQuest())
                {
                    if (m_subQuests[m_currentPhase].IsQuestFinished())
                    {
                        m_currentPhase++;
                        if (m_currentPhase >= m_subQuests.Count)
                        {

                        }
                    }
                }
                else
                {

                }
                if (CheckFinish())
                {
                    OnQuestFinish.Invoke();
                    m_state = QuestState.finish;
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

    public bool IsQuestFinished()
    {
        return m_state == QuestState.finish;
    }
    bool CheckFinish()
    {
        return m_quest.MeetEndCondition();
    }
    /// <summary>
    /// 強制的にイベントを発生させる
    /// プレイヤーを所定の場所に移動させ、会話を強制再生?
    /// </summary>
    public void ForceStart()
    {

    }
}
