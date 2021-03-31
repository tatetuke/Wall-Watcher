using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// プレイヤーが請け負ったクエストを管理するクラス
/// </summary>
[DisallowMultipleComponent]
public class QuestHolder : MonoBehaviour
{
    /// <summary>
    /// 請け負った、または完了したクエスト
    /// </summary>
    [SerializeField,ReadOnly] List<QuestChecker> m_quests = new List<QuestChecker>();

    public IEnumerable<QuestChecker> Data { get => m_quests; }
    private void Awake()
    {
        OnQuestAdd.AddListener(() => {
            EffectGenerator.Instance.Generate("quest");
        });
    }
    public void Initialize(QuestDataSO quest_, QuestChecker.QuestState state_, int chapter_)
    {
        var obj = new GameObject(quest_.name);
        var scr = obj.AddComponent<QuestChecker>();
        scr.Initialize(quest_, state_, chapter_);
        scr.gameObject.transform.parent = transform.parent;
        m_quests.Add(scr);
    }

    /// <summary>
    /// クエストを受注する
    /// </summary>
    /// <param name="quest"></param>
    public void AddQuest(QuestDataSO quest_)
    {
        var obj = new GameObject(quest_.name);
        var scr = obj.AddComponent<QuestChecker>();
        scr.Initialize(quest_, QuestChecker.QuestState.working, 0);
        scr.gameObject.transform.parent = transform.parent;
        m_quests.Add(scr);
        OnQuestAdd.Invoke();
    }
    /// <summary>
    /// クエストを請け負ったときに実行されるイベント
    /// </summary>
    public UnityEvent OnQuestAdd { get; } = new UnityEvent();

}
