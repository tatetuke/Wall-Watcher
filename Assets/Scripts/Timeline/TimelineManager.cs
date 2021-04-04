using KoganeUnityLib;
using RPGM.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    // MEMO : 会話タイムラインに選択肢の追加

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Player player;

    [SerializeField] Animator OptionAnimator;

    private bool IsStarted;
    private bool IsEnd;
    public TMP_Typewriter m_typewriter;

    public State m_State = State.Normal;
    public ConversationData conversationData;
    private Conversations CurrentConversation = null;
    private string FileId;
    private string Id;
    bool IsFinished = false;


    public enum State
    {
        Normal,
        FirstFrameCurrentConversation,
        Talking,
        FinishCurrentConversation
    }

    private void ProceedTalk()
    {
        if (m_State == State.Normal)
        {
            IsFinished = false;
        }
        else if (m_State == State.FirstFrameCurrentConversation)
        {
            if (IsFinished) return;  // タイムラインによって上書きされた時のためにある。もう処理が終わっていたらなにもしない
            playableDirector.Pause();
            UpdateCurrentConversation();
            m_typewriter.Play(text: CurrentConversation.text, speed: 15, onComplete: () => ChangeState(State.FinishCurrentConversation));
            ChangeState(State.Talking);
        }
        else if (m_State == State.Talking)
        {
            OptionAnimator.SetBool("SelectKey", true);
            //下キーが押されたら文字送りをスキップして本文を出力する。
            if (Input.GetKeyDown("space"))
                m_typewriter.Skip();
        }
        else if (m_State == State.FinishCurrentConversation)
        {
            if (Input.GetKeyDown("space"))
            {
                ClearTypewriter();
                playableDirector.Resume();
                ChangeState(State.Normal);
                IsFinished = true;
            }
        }
    }


    void Start()
    {
        IsStarted = false;
        IsEnd = false;
    }

    void Update()
    {
        ProceedTalk();
    }

    private void ClearTypewriter()
    {
        m_typewriter.Play(text: "", speed: 0);
    }

    public void ChangeState(State state)
    {
        m_State = state;
    }

    void UpdateCurrentConversation()
    {
        // CurrentConversationの更新
        if (IsFirstTalk())  // 一番最初だけ例外
        {
            // FirstConversationをIdとして指定
            Id = conversationData.GetFirst();
            CurrentConversation = conversationData.Get(Id);
        }
        else
        {
            // ConversationsのtargetIdをIdとして指定
            Id = CurrentConversation.targetID;
            CurrentConversation = conversationData.Get(Id);
        }
    }

    public bool IsFirstTalk()
    {
        return CurrentConversation == null;
    }
}