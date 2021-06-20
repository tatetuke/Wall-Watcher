using KoganeUnityLib;
using RPGM.Gameplay;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineManager : MonoBehaviour
{
    // MEMO : 会話タイムラインに選択肢の追加

    [SerializeField]
    private PlayableDirector playableDirector;
    [SerializeField]
    private Player player;
    private bool IsStarted;
    private bool IsEnd;
    public TMP_Typewriter m_typewriter;

    public State m_State = State.Idle;
    public ConversationData conversationData;
    private Conversations CurrentConversation = null;
    private string FileId;
    private string Id;


    public enum State
    {
        Idle,
        Talk
    }

    public void ChangeState(State state)
    {
        m_State = state;
    }

    // Start is called before the first frame update
    void Start()
    {
        IsStarted = false;
        IsEnd = false;
    }

    bool IsFirstFrame = true;
    bool IsFinishCurrentConversation = false;

    // Update is called once per frame
    void Update()
    {
        if (m_State == State.Talk)
        {
            if (IsFinishCurrentConversation)
            {
                if (Input.GetKeyDown("space"))
                {
                    m_typewriter.Play(text: "", speed: 0);
                    playableDirector.Resume();
                    IsFinishCurrentConversation = false;
                }
            }
            else
            {
                if (IsFirstFrame)
                {
                    playableDirector.Pause();
                    UpdateCurrentConversation();
                    m_typewriter.Play(text: CurrentConversation.text, speed: 15, onComplete: () => IsFinishCurrentConversation = true);
                    IsFirstFrame = false;
                }
                else
                {
                    //下キーが押されたら文字送りをスキップして本文を出力する。
                    if (Input.GetKeyDown("space"))
                    {
                        m_typewriter.Skip();
                    }
                }
            }
        }
        else IsFirstFrame = true;


        //// Tキーが押され、再生中でないならば、再生する
        //if (Input.GetKeyDown(KeyCode.T) && playableDirector.state != PlayState.Playing)
        //{
        //    IsStarted = true;
        //    Debug.Log("タイムラインが再生されました");
        //    player.ChangeState(Player.State.FREEZE);
        //    // タイムラインの再生
        //    playableDirector.Play();
        //}

        //if(IsStarted && playableDirector.state != PlayState.Playing)
        //{
        //    Debug.Log("タイムライン終了");
        //    player.ChangeState(Player.State.IDLE);
        //}
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