using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using RPGM.Core;
using RPGM.Gameplay;
using TMPro;
using KoganeUnityLib;
using System.Threading;
using UnityEngine.Playables;
/// <summary>
/// 文章を表示します。
/// スペースキーが押されたときに文章を送ります。
/// </summary>
public class ConversationDataManager : SingletonMonoBehaviour<ConversationDataManager>/*,ILoadableAsync*/
{
    public enum State
    {
        Normal,
        TryStop,
        SettingPosition,
        Talking,
    }
    State m_State = State.Normal;

    private bool IsCompleteAnimation;


    [SerializeField] private GameObject TalkCanvas;
    [SerializeField] private TextMeshProUGUI TextBox;
    [SerializeField] private PlayableDirector playableDirector;
    public TMP_Typewriter m_typewriter;
    public GameObject[] Options;
    public TextMeshProUGUI[] OptionTexts;
    public GameObject[] Borders;
    private GameObject m_Player;
    private GameObject m_PlayerSprite;
    private Player PlayerScript;
    private GameObject TargetNPC;
    private Animator OptionAnimator;

    DialogController m_dialogController;
    SelectManager m_selectManager;

    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;

    Color Red = Color.red;
    private string FileId;
    private string Id;



    private QuestHolder m_QuestHolder;  //クエストを追加するときに使う。
    
    /// <summary>
    /// プレイヤーがスペースキーを押し、会話が始められる状態になったとき実行されるイベント
    /// </summary>
    public UnityEvent OnTalkAccepted { get; } = new UnityEvent();
    /// <summary>
    /// プレイヤーの移動が止まり、会話が始まったとき実行されるイベント
    /// </summary>
    public UnityEvent OnTalkStart { get; } = new UnityEvent();
    /// <summary>
    /// 会話が終わったとき実行されるイベント
    /// </summary>
    public UnityEvent OnTalkEnd { get; } = new UnityEvent();

    [System.Obsolete]
    private void Start()
    {
        IsCompleteAnimation = true;
        OptionAnimator = this.transform.GetChild(0).GetChild(0).GetComponent<Animator>();
        m_Player = GameObject.Find("Player");
        m_PlayerSprite = m_Player.transform.Find("PlayerSprite").gameObject;
        PlayerScript = m_Player.GetComponent<Player>();
        m_QuestHolder = m_Player.GetComponent<QuestHolder>();
        m_dialogController = new DialogController();
        m_selectManager = new SelectManager(OptionTexts, Color.yellow, Color.white);
    }

    public bool IsTalking()
    {
        return m_State == State.Talking;
    }

    [System.Obsolete]
    private void Update()
    {
        UpdateState();

        if (m_State == State.Normal)
            UpdateGlowImage();
        else if (m_State == State.TryStop)
            return;
        else if (m_State == State.SettingPosition)
            return;
        else if (m_State == State.Talking)
        {
            // MEMO : 
            // 文字が全て出力し終わったら、自動的に選択肢を表示させる
            // 文字全て出力 -> 選択肢のアニメーション -> 右左のSetBorder
            //選択肢を選択し終わった後にSelectKeyをfalseにする(初期値に戻す)
            if (OptionAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartState"))
            {   
                OptionAnimator.SetBool("SelectKey", false);

            }


            //選択肢があるとき
            if (ExistOptions(CurrentConversation))
            {

                
                // 選択肢を出すアニメーションが終わったら
                //選択肢を選択切り替えできる,スペースで現在の選択を決定できる。
                if (OptionAnimator.GetCurrentAnimatorStateInfo(0).IsName("CompleteBorderAnimation"))
                {
                    //選択肢の選択切り替え
                    SetBorder(CurrentConversation);
                    //選択されたら.
                    if (Input.GetKeyDown("space"))
                    {
                        //アニメーションを初期状態に戻す。
                OptionAnimator.SetBool("IsPlayAnimation", false);
                        OptionAnimator.SetBool("SelectKey", true);
                        //選択した会話に進める。
                        ProceedTalk();

                    }

                }
            }
            else
            {
                if (IsFirstTalk())
                    ProceedTalk();
                else
                {
                    if (Input.GetKeyDown("space"))
                        ProceedTalk();
                }

                //下キーが押されたら文字送りをスキップして本文を出力する。
                if (Input.GetKeyDown("space"))
                    m_typewriter.Skip();
            }

            ////アニメーションが終了したら
            //if (OptionAnimator.GetCurrentAnimatorStateInfo(0).IsName("CompleteBorderAnimation"))
            //{
            //    //会話の遷移を行えるようにする。
            //    IsCompleteAnimation = true;
            //    //繰り返しアニメーションがされないようにする。
            //    OptionAnimator.SetBool("IsPlayAnimation", false);
            //}

            ////アニメーションが終了していたら会話ができる。
            //if (IsCompleteAnimation)
            //{

            //    SetBorder(CurrentConversation);
            //    if (IsFirstTalk())
            //        ProceedTalk();
            //    else
            //    {
            //        if (Input.GetKeyDown("space"))
            //            ProceedTalk();
            //    }

            //    //下キーが押されたら文字送りをスキップして本文を出力する。
            //    if (Input.GetKeyDown("down"))
            //        m_typewriter.Skip();
            //}
        }
    }

    void UpdateState()
    {
        if (m_State == State.Normal)
        {
            if (Input.GetKeyDown("space"))
            {
                OnTalkAccepted.Invoke();
                PlayerScript.ChangeState(Player.State.FREEZE);
                TargetNPC = SearchNearNPC.Instance.GetNearNPC();
                SetGlowLine(TargetNPC, Color.yellow);

                if (PlayerScript.IsWalking)
                    ChangeState(State.TryStop);
                else
                {
                    if (IsClosePosition(m_Player, TargetNPC))
                    {
                        ChangeState(State.Talking);
                    }
                    else
                        ChangeState(State.SettingPosition);
                }
            }
        }
        else if (m_State == State.TryStop)
        {
            if (!PlayerScript.IsWalking)
            {
                if (IsClosePosition(m_Player, TargetNPC))
                    ChangeState(State.Talking);
                else
                    ChangeState(State.SettingPosition);
            }
        }
        else if (m_State == State.SettingPosition)
        {
            if (playableDirector.state != PlayState.Playing)
                ChangeState(State.Talking);
        }
        else if (m_State == State.Talking)
        {

            if (IsFinishTalk())
            {
                CurrentConversation = null;  // 初期化
                PlayerScript.ChangeState(Player.State.IDLE);
                ChangeState(State.Normal);
                OnTalkEnd.Invoke();
            }
        }
    }

    void ChangeState(State state)
    {
        m_State = state;
        switch (state)
        {
            case State.Normal:
                break;
            case State.TryStop:
                break;
            case State.SettingPosition:
                playableDirector.Play();
                break;
            case State.Talking:
                OnTalkStart.Invoke();
                LookNPC();
                break;
            default:
                break;
        }
    }

    private void SetBorder(Conversations conversations)
    {
        // セレクトに関する更新
        if (ExistOptions(CurrentConversation))
        {
            if (Input.GetKeyDown("left"))
            {
                m_dialogController.Hide(Borders[m_selectManager.GetSelectNum()]);
                m_selectManager.UpdateLeft();   // 左押したときに関する更新
                m_dialogController.Display(Borders[m_selectManager.GetSelectNum()]);
            }
            if (Input.GetKeyDown("right"))
            {
                m_dialogController.Hide(Borders[m_selectManager.GetSelectNum()]);
                m_selectManager.UpdateRight();  // 右押したときに関する更新
                m_dialogController.Display(Borders[m_selectManager.GetSelectNum()]);
            }
        }
    }

    private void ProceedTalk()
    {
        UpdateCurrentConversation();

        // 会話の内容の更新
        TextBox.text = CurrentConversation.text;

        //テキストを一文字一文字出力する。
        //Play(テキスト本文,一秒間に送る文字数,文字送り終了時に呼び出されるもの)
        m_typewriter.Play(text: CurrentConversation.text, speed: 15, onComplete: () => SetBoolOptionAnimation(CurrentConversation));

        //クエストがあればクエストを追加する。
        AddQuest();

        // 選択肢に関する更新
        if (ExistOptions(CurrentConversation))
        {
            // 選択肢を表示する
            m_dialogController.Display(Options[0]);
            m_dialogController.Display(Options[1]);
            int itr = 0;
            // 選択肢の内容の更新
            foreach (var option in CurrentConversation.options)
            {
                m_dialogController.SetText(OptionTexts[itr], option.text);
                itr++;
            }
            // 初期化 : 左を選択している状態にする
            m_selectManager.ChangeSelectNum(0);
            // selectManager.ChangeColorUp(selectManager.GetSelectNum());

            //UIについて左の選択肢を選択した状態にする.（左を太枠にする）
            m_dialogController.Display(Borders[m_selectManager.GetSelectNum()]);
        }
        else
        {
            // 選択肢を隠す
            m_dialogController.Hide(Options[0]);
            m_dialogController.Hide(Options[1]);
            //選択肢の太い枠線を隠す
            m_dialogController.Hide(Borders[0]);
            m_dialogController.Hide(Borders[1]);
        }
    }

    private void SetBoolOptionAnimation(Conversations conversation)
    {
        if (conversation == null) return;

        if (ExistOptions(conversation))
        {
            OptionAnimator.SetBool("IsPlayAnimation", true);
        }
    }

    private void UpdateCurrentConversation()
    {
        if (ExistOptions(CurrentConversation))
        {
            // 選ばれた選択肢の色を元に戻す
            m_selectManager.ChangeColorDown(m_selectManager.GetSelectNum());
            // ConversationsのConversationOption型リストのtargetIdをIdとして指定
            Id = CurrentConversation.options[m_selectManager.GetSelectNum()].targetId;
            CurrentConversation = CurrentConversationData.Get(Id);
        }
        else
        {
            if (IsFirstTalk())  // 一番最初だけ例外
            {
                // FirstConversationをIdとして指定
                //int index = TargetNPC.GetComponent<NPCController>().GetConversationIndex();
                int index = 0;
                FileId = TargetNPC.GetComponent<NPCController>().ConversationDataList[index];
                CurrentConversationData = TargetNPC.GetComponent<NPCController>().GetConversation(FileId);
                Id = CurrentConversationData.GetFirst();
                CurrentConversation = CurrentConversationData.Get(Id);

                // 会話の位置の更新
                Vector3 TalkCanvasPosition;
                TalkCanvasPosition = TargetNPC.transform.position;
                TalkCanvas.transform.position = TalkCanvasPosition;
            }
            else
            {
                // ConversationsのtargetIdをIdとして指定
                Id = CurrentConversation.targetID;
                CurrentConversation = CurrentConversationData.Get(Id);
            }
        }
    }

    /// <summary>
    /// CurrenConversation.questがnullでない場合クエストを追加する関数
    /// </summary>
    private void AddQuest()
    {
        if (CurrentConversation == null || CurrentConversation.quest == null) return;

        //クエストの追加
        m_QuestHolder.AddQuest(CurrentConversation.quest);
        Debug.Log("クエスト追加");
    }


    void UpdateGlowImage()
    {
        // 前回の対象を光らせなくする
        if (TargetNPC != null)
            SetGlowLine(TargetNPC, Color.blue);

        // TargetNPCの更新
        TargetNPC = SearchNearNPC.Instance.GetNearNPC();

        // 今回の対象を光らせる
        if (TargetNPC != null)
            SetGlowLine(TargetNPC, Color.yellow);
    }

    void SetGlowLine(GameObject gameObject, Color color)
    {
        
        if (gameObject == null) return;
         OutLineSetter scr = gameObject.GetComponentInChildren<OutLineSetter>();
        //Material material= image.GetComponent<Renderer>().material;
        //material.SetFloat("_Thick", num);
        
        //やろうとしたこと
        //Material m_material = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().GetComponent<Material>();
        //m_material.SetFloat("Vector1_C1366B5E", 1);
        //m_material.SetColor("Color_7C7012AB", color);
        scr.SetWidth(10);
    }

    public bool IsFirstTalk()
    {
        return CurrentConversation == null;
    }

    public bool IsFinishTalk()
    {
        if (CurrentConversation == null) return false;
        return CurrentConversation.id == "FINISH";
    }

    private bool ExistOptions(Conversations conversations)
    {
        if (conversations == null)return false;
        return conversations.options.Count != 0;
    }

    bool IsClosePosition(GameObject gameObjectA, GameObject gameObjectB)
    {
        float diff = Mathf.Abs(gameObjectA.transform.position.x - gameObjectB.transform.position.x);
        return (diff > 2.0 - 0.5 && diff < 2.0 + 0.5);
    }

    void LookNPC()
    {
        if (m_Player.transform.position.x < TargetNPC.transform.position.x)
            m_PlayerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
        else
            m_PlayerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public GameObject GetTargetNPC()
    {
        return TargetNPC;
    }

    public State GetState()
    {
        return m_State;
    }
}
