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
    public class QuestData
    {
        public QuestDataSO quest;
        public QuestChecker.QuestState state;
        public int chapter;
    }


    /// <summary>
    /// 請け負った、または完了したクエスト
    /// </summary>
    [SerializeField,ReadOnly] List<QuestData> m_quests = new List<QuestData>();

    public IEnumerable<QuestData> Data { get => m_quests; }
    private void Awake()
    {
        OnQuestAdd.AddListener(() => {
            EffectGenerator.Instance.Generate("quest");
        });
    }
    public void Initialize(QuestDataSO quest_, QuestChecker.QuestState state_, int chapter_)
    {
        m_quests.Add(new QuestData { quest = quest_, state = state_, chapter = chapter_ });
    }

    /// <summary>
    /// クエストを請け負う
    /// </summary>
    /// <param name="quest"></param>
    public void AddQuest(QuestDataSO quest_)
    {
        m_quests.Add(new QuestData { quest = quest_, state = QuestChecker.QuestState.not_yet, chapter = 0 });
        OnQuestAdd.Invoke();
    }
    /// <summary>
    /// クエストを請け負ったときに実行されるイベント
    /// </summary>
    public UnityEvent OnQuestAdd { get; } = new UnityEvent();

}
