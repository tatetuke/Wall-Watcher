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
    QuestState m_state = QuestState.not_yet;
    [SerializeField] QuestDataSO questData;//対象となるクエストデータ
    public UnityEvent OnQuestStart = new UnityEvent();
    public UnityEvent OnQuestFinish = new UnityEvent();
    int m_currentPhase = 0;
    private void Awake()
    {
        m_currentPhase = 0;
    }

    public bool IsQuestFinished()
    {
        return m_state == QuestState.finish;
    }
    public bool CheckFinish()
    {
        return questData.MeetEndCondition();
    }
    /// <summary>
    /// 強制的にイベントを発生させる
    /// プレイヤーを所定の場所に移動させ、会話を強制再生?
    /// </summary>
    public void ForceStart()
    {

    }
    private void Start()
    {
    }
    private void Update()
    {
        switch (m_state)
        {
            case QuestState.not_yet://受注できるかチェック
                m_state = QuestState.working;
                OnQuestStart.Invoke();
                break;
            case QuestState.working://受注している状態。終了できるかチェック
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
}
