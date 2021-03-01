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
public class ConversationDataManager : SingletonMonoBehaviour<ConversationDataManager>, ILoadableAsync
{
    [SerializeField] private AssetLabelReference _labelReference;
    [SerializeField] TextMeshProUGUI TextBox;

    private Material NPCMaterial;
    [SerializeField] float LineThickness = 1;
    private GameObject TargetNPC;
    private GameObject TargetNPCImage;
    private Material TargetNPCMaterial;

    private GameObject m_Player;
    private GameObject m_PlayerSprite;
    private Player PlayerScript;

    [SerializeField]
    private PlayableDirector playableDirector;

    DialogController dialogController;
    SelectManager selectManager;
    public TMP_Typewriter m_typewriter;
    public GameObject[] Options;
    public TextMeshProUGUI[] OptionTexts;
    // public Text[] OptionTexts;
    private int SelectNum;
    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;
    string FileId;
    string Id;

    string ConversationDataFolderPath;
    string[] Files;
    List<string> ConversationDataList;


    private void Awake()
    {
        //base.Awake();
        SaveLoadManager.Instance.SetLoadable(this);
    }

    [System.Obsolete]
    private void Start()
    {
        m_Player = GameObject.Find("Player");
        PlayerScript = m_Player.GetComponent<Player>();
        m_PlayerSprite = m_Player.transform.FindChild("PlayerSprite").gameObject;
        SelectNum = 0;
        dialogController = new DialogController();
        selectManager = new SelectManager(OptionTexts, Color.yellow, Color.black);

        /// <summary>
        /// 指定したフォルダからConversationDataを全て取ってくる
        /// </summary>
        List<string> ConversationDataList = new List<string>();
        // インスペクターのLabel Referenceで指定されたものを用いてPathを取得
        ConversationDataFolderPath = "Assets/Data/" + _labelReference.labelString;
        // フォルダ内のすべてのファイル名を取得する
        Files = System.IO.Directory.GetFiles(@ConversationDataFolderPath, "*");
        for (int i = 0; i < Files.Length; i++)
        {
            // 拡張子名部分を取得
            string extension = System.IO.Path.GetExtension(Files[i]);
            if (extension == ".asset")
            {
                // 拡張子をのぞいたファイル名部分を取得
                string filename = System.IO.Path.GetFileNameWithoutExtension(Files[i]);
                ConversationDataList.Add(filename);
            }
        }

        // デバッグ用 : 必要なファイルが取り出せているか
        //Debug.Log("ファイル名を出力します");
        //foreach (var output in ConversationDataList) Debug.Log(output);
        //Debug.Log("ファイル名を出力しました");

        // TODO : クエストの進行度によって用いるConversationDataを決める
        FileId = ConversationDataList[0];
    }

    AsyncOperationHandle<IList<ConversationData>> m_handle;
    public Dictionary<string, ConversationData> m_data = new Dictionary<string, ConversationData>();

    public async Task Load(CancellationToken cancellationToken)
    {
        Debug.Log("try conversation load", gameObject);
        //ゲーム内アイテムデータを読み込む
        m_handle = Addressables.LoadAssetsAsync<ConversationData>(_labelReference, null);
        await m_handle.Task;
        foreach (var res in m_handle.Result)
        {
            m_data.Add(res.name, res);
            Debug.Log($"Load Conversation: '{res.name}'");
        }
        Addressables.Release(m_handle);
        return;
    }

    public ConversationData GetConversation(string ID)
    {
        if (!m_data.ContainsKey(ID)) return null;
        return m_data[ID];
    }

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
        // 前回自分が対象のNPCならば光らせないようにする
        if (TargetNPC == this.gameObject)
            TargetNPCMaterial.SetFloat("_Thick", 0);

        TargetNPC = SearchNearNPC.Instance.NearNPC();

        // TargetNPC != null : 対象がいる
        // CurrentConversation != null : 会話しはじめてる

        // 今回自分が対象のNPCならば光らせる
        if (TargetNPC == this.gameObject)
        {
            // 会話中は光らせない
            if (/*まだ話しかけていない*/CurrentConversation == null)
            {
                TargetNPCImage = TargetNPC.transform.FindChild("NPCImage(Sprite)").gameObject;
                TargetNPCMaterial = TargetNPCImage.GetComponent<Renderer>().material;
                TargetNPCMaterial.SetFloat("_Thick", LineThickness);  // 光らせる
            }
        }

        if (TargetNPC == this.gameObject)
        {
            // セレクトに関する更新
            if (IsOptionTalk(CurrentConversation))
            {
                if (Input.GetKeyDown("left"))
                    selectManager.UpdateLeft(ref SelectNum);   // 左押したときに関する更新
                if (Input.GetKeyDown("right"))
                    selectManager.UpdateRight(ref SelectNum);  // 右押したときに関する更新
            }

            // スペースが押されたら会話を進める
            if (Input.GetKeyDown("space"))
            {
                if (/*話しかけたら*/CurrentConversation == null)
                {
                    Quaternion quaternion = m_PlayerSprite.transform.rotation;
                    float m_PlayerSprite_rotation_y = quaternion.eulerAngles.y;
                    // 会話中はプレイヤーは動けないようにする
                    //PlayerScript.ChangeState(Player.State.IDLE);
                    PlayerScript.ChangeState(Player.State.FREEZE);

                    // プレイヤーが対象のNPCの方向に向くようにする
                    if (m_Player.transform.position.x < TargetNPC.transform.position.x)  // プレイヤー,対象のNPC の順番
                    {
                        if (m_PlayerSprite_rotation_y == 0)  // プレイヤーが左向いている
                        {
                            m_PlayerSprite.transform.rotation = Quaternion.Euler(0, 180, 0);
                        }
                    }
                    else                                                                 // 対象のNPC,プレイヤー の順番
                    {
                        //Debug.Log(TargetNPC.transform.position.x);
                        if (m_PlayerSprite_rotation_y != 0)  // プレイヤーが右向いている
                        {
                            m_PlayerSprite.transform.rotation = Quaternion.Euler(0, 0, 0);
                        }
                    }
                }

                // CurrentConversationの更新
                if (IsOptionTalk(CurrentConversation))
                {
                    // 選ばれた選択肢の色を元に戻す
                    selectManager.ChangeColorDown(SelectNum);
                    // ConversationsのConversationOption型リストのtargetIdをIdとして指定
                    Id = CurrentConversation.options[SelectNum].targetId;
                    CurrentConversation = CurrentConversationData.Get(Id);
                }
                else
                {
                    if (CurrentConversation == null)  // 一番最初だけ例外
                    {
                        // FirstConversationをIdとして指定
                        CurrentConversationData = GetConversation(FileId);
                        Id = CurrentConversationData.GetFirst();
                        CurrentConversation = CurrentConversationData.Get(Id);
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
                    SelectNum = 0;
                    selectManager.ChangeColorUp(SelectNum);
                }
                else
                {
                    // 選択肢を隠す
                    dialogController.Hide(Options[0]);
                    dialogController.Hide(Options[1]);
                }
            }
            //下キーが押されたら文字送りをスキップして本文を出力する。
            if (Input.GetKeyDown("down"))
            {
                m_typewriter.Skip();
            }
        }
    }


    private bool IsOptionTalk(Conversations conversations)
    {
        if (conversations == null) return false;

        return conversations.options.Count != 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            //タグの変更.SearchNearNPCで使われる.
            this.tag = "CanConversationNPC";
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            //タグの変更.SearchNearNPCで使われる.
            this.tag = "NPC";

            //// MEMO : 以下は初期化だが、会話中プレイヤーを操作できないようにすれば要らない
            //CurrentConversation = null;
            //TextBox.text = "";
            //selectManager.ChangeColorDown(SelectNum);
            //dialogController.Hide(Options[0]);
            //dialogController.Hide(Options[1]);
        }
    }

    public GameObject GetTargetNPC()
    {
        return TargetNPC;
    }
}
