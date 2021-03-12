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
    enum State
    {
        Normal,
        TryStop,
        SettingPosition,
        Talking,
    }
    State m_State = State.Normal;

    //comeが編集********************************************************************:
    //[SerializeField] private AssetLabelReference _labelReference;
    //********************************************************************comeが編集
    [SerializeField] GameObject TalkCanvas;
    [SerializeField] TextMeshProUGUI TextBox;

    private Material NPCMaterial;
    [SerializeField] float LineThickness = 1;
    private GameObject TargetNPC;
    private GameObject TargetNPCImage;
    private Material TargetNPCMaterial;

    private GameObject m_Player;
    private GameObject m_PlayerSprite;
    private Player PlayerScript;
    public bool IsTalking = false;
    private bool IsFirstTalk = false;
    private bool IsWaitingStop = false;

    [SerializeField]
    private PlayableDirector playableDirector;

    DialogController dialogController;
    SelectManager selectManager;
    public TMP_Typewriter m_typewriter;
    public GameObject[] Options;
    public TextMeshProUGUI[] OptionTexts;
    // public Text[] OptionTexts;
    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;
    string FileId;
    string Id;


    //comeが編集****************************************************************************************************
    // string ConversationDataFolderPath;
    // string[] Files;
    // List<string> ConversationDataList;
    //**************************************************************************************************************:comeが編集



    //comeが編集****************************************************************
    //private void Awake()
    //{
    //    //base.Awake();
    //    SaveLoadManager.Instance.SetLoadable(this);
    //}
    //********************************************************comeが編集
    [System.Obsolete]
    private void Start()
    {
        m_Player = GameObject.Find("Player");
        PlayerScript = m_Player.GetComponent<Player>();
        m_PlayerSprite = m_Player.transform.Find("PlayerSprite").gameObject;
        dialogController = new DialogController();
        selectManager = new SelectManager(OptionTexts, Color.yellow, Color.white);

        //comeが編集**************************************************************************************
        /// <summary>
        /// 指定したフォルダからConversationDataを全て取ってくる
        /// </summary>
        //ConversationDataList = new List<string>();
        //// インスペクターのLabel Referenceで指定されたものを用いてPathを取得
        //ConversationDataFolderPath = "Assets/Data/" + _labelReference.labelString;
        //// フォルダ内のすべてのファイル名を取得する
        //Files = System.IO.Directory.GetFiles(@ConversationDataFolderPath, "*");
        //for (int i = 0; i < Files.Length; i++)
        //{
        //    // 拡張子名部分を取得
        //    string extension = System.IO.Path.GetExtension(Files[i]);
        //    if (extension == ".asset")
        //    {
        //        // 拡張子をのぞいたファイル名部分を取得
        //        string filename = System.IO.Path.GetFileNameWithoutExtension(Files[i]);
        //        ConversationDataList.Add(filename);
        //    }
        //}

        //// デバッグ用 : 必要なファイルが取り出せているか
        ////Debug.Log("ファイル名を出力します");
        ////foreach (var output in ConversationDataList) Debug.Log(output);
        ////Debug.Log("ファイル名を出力しました");

        //// TODO : クエストの進行度によって用いるConversationDataを決める
        //FileId = ConversationDataList[0];
        //**************************************************************************************comeが編集
    }


    //comeが編集**************************************************************************************
    //AsyncOperationHandle<IList<ConversationData>> m_handle;
    //public Dictionary<string, ConversationData> m_data = new Dictionary<string, ConversationData>();

    //public async Task LoadAsync(CancellationToken cancellationToken)
    //{
    //    Debug.Log("try conversation load", gameObject);
    //    //ゲーム内アイテムデータを読み込む
    //    m_handle = Addressables.LoadAssetsAsync<ConversationData>(_labelReference, null);
    //    await m_handle.Task;
    //    foreach (var res in m_handle.Result)
    //    {
    //        m_data.Add(res.name, res);
    //        Debug.Log($"Load Conversation: '{res.name}'");
    //    }
    //    Addressables.Release(m_handle);
    //    return;
    //}
    //**************************************************************************************comeが編集


    //comeが編集**************************************************************************************
    //public ConversationData GetConversation(string ID)
    //{
    //    if (!m_data.ContainsKey(ID)) return null;
    //    return m_data[ID];
    //}
    //**************************************************************************************comeが編集

    //Quest activeQuest = null;

    //Quest[] quests;

    //GameModel model = Schedule.GetModel<GameModel>();

    //void OnEnable()
    //{
    //    quests = gameObject.GetComponentsInChildren<Quest>();
    //}


    [System.Obsolete]
    private void Update()
    {
        // 状態の更新
        UpdateState();

        if (m_State == State.Normal)
            UpdateGlowImage();
        else if (m_State == State.TryStop)
            return;
        else if (m_State == State.SettingPosition)
            return;
        else if (m_State == State.Talking)
        {
            // セレクトに関する更新
            if (IsOptionTalk(CurrentConversation))
            {
                if (Input.GetKeyDown("left"))
                    selectManager.UpdateLeft();   // 左押したときに関する更新
                if (Input.GetKeyDown("right"))
                    selectManager.UpdateRight();  // 右押したときに関する更新
            }

            if (CurrentConversation == null)
                ProceedTalk();
            else
            {
                if (Input.GetKeyDown("space"))
                    ProceedTalk();
            }

        }


        //if (TargetNPC != null)
        //{
        //    // セレクトに関する更新
        //    if (IsOptionTalk(CurrentConversation))
        //    {
        //        if (Input.GetKeyDown("left"))
        //            selectManager.UpdateLeft();   // 左押したときに関する更新
        //        if (Input.GetKeyDown("right"))
        //            selectManager.UpdateRight();  // 右押したときに関する更新
        //    }

        //    if ((Input.GetKeyDown("space") && !IsTalking) || IsWaitingStop)
        //    {
        //        PlayerScript.ChangeState(Player.State.FREEZE);
        //        SearchNearNPC.Instance.GetNearNPC();
        //        SearchNearNPC.Instance.IsDecided = true;
        //        IsTalking = true;
        //        if (PlayerScript.IsWalking)
        //        {
        //            IsWaitingStop = true;
        //        }
        //        else
        //        {
        //            float diff = Mathf.Abs(m_Player.transform.position.x - TargetNPC.transform.position.x);
        //            if (diff > 2.0 - 0.5 && diff < 2.0 + 0.5)
        //            {
        //                //Quaternion quaternion = m_PlayerSprite.transform.rotation;
        //                //float PlayerSprite_rotation_y = quaternion.eulerAngles.y;
        //                // プレイヤーが対象のNPCの方向に向くようにする
        //                if (m_Player.transform.position.x < TargetNPC.transform.position.x)
        //                    m_PlayerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
        //                else
        //                    m_PlayerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
        //            }
        //            else
        //            {
        //                playableDirector.Play();
        //            }
        //            IsFirstTalk = true;
        //            IsWaitingStop = false;
        //        }
        //    }

        //    if (!IsWaitingStop)
        //    {
        //        if (!IsFirstTalk && Input.GetKeyDown("space"))
        //        {
        //            ProceedTalk();
        //        }

        //        // タイムライン再生が終わったらスペースを押さなくても1回分の会話は進む
        //        if (IsFirstTalk && playableDirector.state != PlayState.Playing)
        //        {
        //            IsFirstTalk = false;
        //            ProceedTalk();
        //        }
        //    }

        //    //下キーが押されたら文字送りをスキップして本文を出力する。
        //    if (Input.GetKeyDown("down"))
        //    {
        //        m_typewriter.Skip();
        //    }
        //}
    }


    private bool IsOptionTalk(Conversations conversations)
    {
        if (conversations == null) return false;

        return conversations.options.Count != 0;
    }

    public GameObject GetTargetNPC()
    {
        return TargetNPC;
    }

    private void ProceedTalk()
    {
        // CurrentConversationの更新
        if (IsOptionTalk(CurrentConversation))
        {
            // 選ばれた選択肢の色を元に戻す
            selectManager.ChangeColorDown(selectManager.GetSelectNum());
            // ConversationsのConversationOption型リストのtargetIdをIdとして指定
            Id = CurrentConversation.options[selectManager.GetSelectNum()].targetId;


            CurrentConversation = CurrentConversationData.Get(Id);

        }
        else
        {
            if (CurrentConversation == null)  // 一番最初だけ例外
            {
                // FirstConversationをIdとして指定
                int index = TargetNPC.GetComponent<NPCController>().GetConversationIndex();

                //comeが編集***************************************************************************************************************
                //FileId = ConversationDataList[index];
                //CurrentConversationData = GetConversation(FileId);

                FileId = TargetNPC.GetComponent<NPCController>().ConversationDataList[index];
                CurrentConversationData = TargetNPC.GetComponent<NPCController>().GetConversation(FileId);
                //*****************************************************************************************************************comeが編集

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


        // 番兵だったら会話を終了し、CurrentConversationを初期化
        if (CurrentConversation.id == "FINISH")
        {
            CurrentConversation = null;
            PlayerScript.ChangeState(Player.State.IDLE);
            //IsTalking = false;
        }

        // 選択肢に関する更新
        if (IsOptionTalk(CurrentConversation))
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
            selectManager.ChangeColorUp(selectManager.GetSelectNum());
        }
        else
        {
            // 選択肢を隠す
            dialogController.Hide(Options[0]);
            dialogController.Hide(Options[1]);
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
            if (CurrentConversation == null)
            {
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

    void UpdateGlowImage()
    {
        // 前回自分が対象のNPCならば光らせないようにする
        if (TargetNPC != null)
            SetGlowLine(TargetNPC, 0);
            //TargetNPCMaterial.SetFloat("_Thick", 0);

        TargetNPC = SearchNearNPC.Instance.GetNearNPC();

        // 今回自分が対象のNPCならば光らせる
        if (TargetNPC != null)
        {
            SetGlowLine(TargetNPC, LineThickness);

            //TargetNPCImage = TargetNPC.transform.GetChild(0).gameObject;
            //TargetNPCMaterial = TargetNPCImage.GetComponent<Renderer>().material;
            //TargetNPCMaterial.SetFloat("_Thick", LineThickness);  // 光らせる

            //// 会話中は光らせない
            //if (!IsTalking)
            //{
            //    //TargetNPCImage = TargetNPC.transform.FindChild("NPCImage(Sprite)").gameObject;
            //    //TargetNPCImage = TargetNPC.transform.FindChild("PlayerSprite").gameObject;
            //    TargetNPCImage = TargetNPC.transform.GetChild(0).gameObject;
            //    TargetNPCMaterial = TargetNPCImage.GetComponent<Renderer>().material;
            //    TargetNPCMaterial.SetFloat("_Thick", LineThickness);  // 光らせる
            //}
        }
    }

    void SetGlowLine(GameObject gameObject,float num)
    {
        if (gameObject == null) return;
        GameObject image = gameObject.transform.GetChild(0).gameObject;
        Material material= image.GetComponent<Renderer>().material;
        material.SetFloat("_Thick", num);  // 光らせる
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
}
