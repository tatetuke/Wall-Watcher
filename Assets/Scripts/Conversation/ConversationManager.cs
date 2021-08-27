using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ConversationManager : SingletonMonoBehaviour<ConversationManager>
{
    public enum State
    {
        Normal,
        TryStop,
        SettingPosition,
        Talking,
    }
    State m_State = State.Normal;

    private GameObject m_Player;
    private GameObject m_PlayerSprite;
    private Player PlayerScript;
    private GameObject TargetNPC;
    [SerializeField] private PlayableDirector playableDirector;
    public Flowchart CurrentFlowchart = null;
    public string MessageId; // メッセージID
    private QuestHolder m_QuestHolder;

    void UpdateState()
    {
        if (m_State == State.Normal)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                TargetNPC = SearchNearNPC.Instance.GetNearNPC();
                if (TargetNPC != null)
                {
                    PlayerScript.ChangeState(Player.State.FREEZE);
                    SetGlowLine(TargetNPC, Color.yellow);

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
            if (IsFinishTalk(CurrentFlowchart))
            {
                Debug.Log("会話終了を検知");
                CurrentFlowchart = null;  // 初期化
                PlayerScript.ChangeState(Player.State.IDLE);
                ChangeState(State.Normal);
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
                StartConversation();
                LookNPC();
                break;
            default:
                break;
        }
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
        Material m_material = gameObject.GetComponentInChildren<Renderer>().material;
        //Material m_material = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().GetComponent<Material>();
        //m_material.SetFloat("Vector1_C1366B5E", 1);
        m_material.SetColor("Color_7C7012AB", color);

        //OutLineSetter scr = gameObject.GetComponentInChildren<OutLineSetter>();
        //Material material= image.GetComponent<Renderer>().material;
        //material.SetFloat("_Thick", num);

        //やろうとしたこと
        //Material m_material = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>().GetComponent<Material>();
        //m_material.SetFloat("Vector1_C1366B5E", 1);
        //m_material.SetColor("Color_7C7012AB", color);
        //scr.SetWidth(10);
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

    public bool IsFinishTalk(Flowchart flowchart)
    {
        return !flowchart.GetVariable<BooleanVariable>("IsTalking").Value;
    }

    private void StartConversation()
    {
        NPCController npcController = TargetNPC.GetComponent<NPCController>();
        string npcname = TargetNPC.GetComponent<Character>()?.NameText;
        List<string> questNames = m_QuestHolder.GetQuestNames();

        Flowchart flowchart = npcController.SelectFlowchart(questNames);

        if (flowchart!=null)
        {            
            CurrentFlowchart = flowchart;
            CurrentFlowchart.SendFungusMessage("Start");
        }
        else
        {
            flowchart = npcController.GetFlowchart("NoQuest");
            CurrentFlowchart = flowchart;
            CurrentFlowchart.SendFungusMessage("Start");
        }
        //MessageReceived[] receivers = FindObjectsOfType<MessageReceived>();
        ////取得できた場合
        //if (receivers != null)
        //{
        //    //すべてのMessageReceivedに"test"イベントを送信する
        //    foreach (var receiver in receivers)
        //    {
        //        receiver.OnSendFungusMessage("Test1");
        //    }
        //}
    }

    public GameObject GetTargetNPC()
    {
        return TargetNPC;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_Player = GameObject.Find("Player");
        m_PlayerSprite = m_Player.transform.Find("PlayerSprite").gameObject;
        PlayerScript = m_Player.GetComponent<Player>();
        m_QuestHolder = FindObjectOfType<QuestHolder>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(m_State);
        UpdateState();

        if (m_State == State.Normal)
            UpdateGlowImage();
        else if (m_State == State.TryStop)
            return;
        else if (m_State == State.SettingPosition)
            return;
        else if (m_State == State.Talking)
            return;
    }


}
