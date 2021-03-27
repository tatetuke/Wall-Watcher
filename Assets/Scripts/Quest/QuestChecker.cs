using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// クエストのゲーム内での進行を制御するクラス
/// ゲームの条件をチェックし、条件に満たせばクエスト受注可能になったり、終了したり
/// クエストを受注できるSceneにおく
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
    [SerializeField] QuestDataSO questData;//対象となるクエストデータ
    List<QuestChecker> m_subQuests = new List<QuestChecker>();
    public UnityEvent OnQuestStart = new UnityEvent();
    public UnityEvent OnQuestFinish = new UnityEvent();
    int m_currentPhase=0;
    private void Awake()
    {
        m_currentPhase = 0;
    }

    public bool IsQuestFinished()
    {
        return m_state==QuestState.finish;
    }
    public bool CheckStart()
    {
        foreach(var i in questData.startConditions)
        {
            if (!i.MeetCondition(GetValueFromProperty(i))) return false;
        }
        return true;
    }
    public bool CheckFinish()
    {
        foreach (var i in questData.endConditions)
        {
            if (!i.MeetCondition(GetValueFromProperty(i))) return false;
        }
        return true;
    }

    /// <summary>
    /// 条件からプロパティに変換
    /// </summary>
    /// <param name="condition"></param>
    /// <returns></returns>
    object GetValueFromProperty(QuestConditions condition)
    {
        switch (condition.valueType)
        {
            case QuestConditions.ValueType.Int:
                return GamePropertyManager.Instance.GetIntProperty(condition.parameterKey);
            case QuestConditions.ValueType.Float:
                return GamePropertyManager.Instance.GetFloatProperty(condition.parameterKey);
            case QuestConditions.ValueType.String:
                return GamePropertyManager.Instance.GetStringProperty(condition.parameterKey);
            case QuestConditions.ValueType.Boolean:
                return GamePropertyManager.Instance.GetBoolProperty(condition.parameterKey);
        }
        return null;
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
            case QuestState.not_yet://受注できるかチェック
                if (CheckStart())
                {
                    m_state = QuestState.working;
                    OnQuestStart.Invoke();
                }
                break;
            case QuestState.working://受注している状態。終了できるかチェック
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
