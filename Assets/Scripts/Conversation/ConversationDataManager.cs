using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
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

    DialogController dialogController;
    SelectManager selectManager;

    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;

    private float LineThickness = 1;  // 光らせる際の線の太さ
    private string FileId;
    private string Id;
    
    private QuestHolder m_QuestHolder;  //クエストを追加するときに使う。

    [System.Obsolete]
    private void Start()
    {
        m_Player = GameObject.Find("Player");
        m_PlayerSprite = m_Player.transform.Find("PlayerSprite").gameObject;
        PlayerScript = m_Player.GetComponent<Player>();
        m_QuestHolder = m_Player.GetComponent<QuestHolder>();
        dialogController = new DialogController();
        selectManager = new SelectManager(OptionTexts, Color.yellow, Color.white);
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
            UpdateSelect(CurrentConversation);
            if (IsFirstTalk())
                ProceedTalk();
            else
            {
                if (Input.GetKeyDown("space"))
                    ProceedTalk();
            }

            //下キーが押されたら文字送りをスキップして本文を出力する。
            if (Input.GetKeyDown("down"))
                m_typewriter.Skip();
        }
    }

    void UpdateState()
    {
        if (m_State == State.Normal)
        {
            if (Input.GetKeyDown("space"))
            {
                PlayerScript.ChangeState(Player.State.FREEZE);
                TargetNPC = SearchNearNPC.Instance.GetNearNPC();
                SetGlowLine(TargetNPC, 0);

                if (PlayerScript.IsWalking)
                    ChangeState(State.TryStop);
                else
                {
                    if (IsClosePosition(m_Player, TargetNPC))
                        ChangeState(State.Talking);
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
            }
        }
    }

    void ChangeState(State state)
    {
        m_State = state;
        if (state == State.SettingPosition)
            playableDirector.Play();
        else if (state == State.Talking)
            LookNPC();
    }

    private void UpdateSelect(Conversations conversations)
    {
        // セレクトに関する更新
        if (ExistOptions(CurrentConversation))
        {
            if (Input.GetKeyDown("left"))
            {
                dialogController.Hide(Borders[selectManager.GetSelectNum()]);
                selectManager.UpdateLeft();   // 左押したときに関する更新
                dialogController.Display(Borders[selectManager.GetSelectNum()]);
            }
            if (Input.GetKeyDown("right"))
            {
                dialogController.Hide(Borders[selectManager.GetSelectNum()]);
                selectManager.UpdateRight();  // 右押したときに関する更新
                dialogController.Display(Borders[selectManager.GetSelectNum()]);
            }
        }
    }

    private void ProceedTalk()
    {
        // CurrentConversationの更新
        if (ExistOptions(CurrentConversation))
        {
            // 選ばれた選択肢の色を元に戻す
            selectManager.ChangeColorDown(selectManager.GetSelectNum());
            // ConversationsのConversationOption型リストのtargetIdをIdとして指定
            Id = CurrentConversation.options[selectManager.GetSelectNum()].targetId;
            CurrentConversation = CurrentConversationData.Get(Id);
        }
        else
        {
            if (IsFirstTalk())  // 一番最初だけ例外
            {
                // FirstConversationをIdとして指定
                int index = TargetNPC.GetComponent<NPCController>().GetConversationIndex();
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

        // 会話の内容の更新
        TextBox.text = CurrentConversation.text;

        //テキストを一文字一文字出力する。
        //Play(テキスト本文,一秒間に送る文字数,文字送り終了時に呼び出されるもの)
        m_typewriter.Play(text: CurrentConversation.text, speed: 15, onComplete: () => Debug.Log("完了"));

        //クエストがあればクエストを追加する。
        AddQuest();

        // 選択肢に関する更新
        if (ExistOptions(CurrentConversation))
        {
            // 選択肢を表示する
            dialogController.Display(Options[0]);
            dialogController.Display(Options[1]);
            int itr = 0;
            // 選択肢の内容の更新
            foreach (var option in CurrentConversation.options)
            {
                dialogController.SetText(OptionTexts[itr], option.text);
                itr++;
            }
            // 初期化 : 左を選択している状態にする
            selectManager.ChangeSelectNum(0);
           // selectManager.ChangeColorUp(selectManager.GetSelectNum());
            //come
            dialogController.Display(Borders[selectManager.GetSelectNum()]);
        }
        else
        {
            // 選択肢を隠す
            dialogController.Hide(Options[0]);
            dialogController.Hide(Options[1]);
            //come
            dialogController.Hide(Borders[0]);
            dialogController.Hide(Borders[1]);
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
            SetGlowLine(TargetNPC, 0);

        // TargetNPCの更新
        TargetNPC = SearchNearNPC.Instance.GetNearNPC();

        // 今回の対象を光らせる
        if (TargetNPC != null)
            SetGlowLine(TargetNPC, LineThickness);
    }

    void SetGlowLine(GameObject gameObject,float num)
    {
        if (gameObject == null) return;
        GameObject image = gameObject.transform.GetChild(0).gameObject;
        Material material= image.GetComponent<Renderer>().material;
        material.SetFloat("_Thick", num);
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
        if (conversations == null)
            return false;
        else
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
