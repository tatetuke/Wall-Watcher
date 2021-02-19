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

/// <summary>
/// 文章を表示します。
/// スペースキーが押されたときに文章を送ります。
/// </summary>
public class ConversationDataManager : SingletonMonoBehaviour<ConversationDataManager>, ILoadableAsync
{
    [SerializeField] private AssetLabelReference _labelReference;
    [SerializeField] TextMeshProUGUI TextBox;

    DialogController dialogController;
    SelectManager selectManager;

    public GameObject[] Options;
    public TextMeshProUGUI[] OptionTexts;
   // public Text[] OptionTexts;
    private int SelectNum;
    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;
    string FileId;
    string Id;
    private bool CanTalk = false;

    private void Awake()
    {
        //base.Awake();
        SaveLoadManager.Instance.SetLoadable(this);
    }

    private void Start()
    {
        SelectNum = 0;
        dialogController = new DialogController();
        selectManager = new SelectManager(OptionTexts, Color.yellow, Color.black);

        // どの会話セットを使うか指定
        // TODO : クエストやら進行度やらによってどうやって指定するか
        FileId = "test";
    }

    AsyncOperationHandle<IList<ConversationData>> m_handle;
    public Dictionary<string, ConversationData> m_data = new Dictionary<string, ConversationData>();

    public async Task Load()
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


    private void Update()
    {
        if (CanTalk)
        {
            // セレクトに関する更新
            if (IsOptionTalk(CurrentConversation))
            {
                if (Input.GetKeyDown("left"))
                {
                    selectManager.UpdateLeft(ref SelectNum);
                }
                if (Input.GetKeyDown("right"))
                {
                    selectManager.UpdateRight(ref SelectNum);
                }
            }

            // スペースが押されたら会話を進める
            if (Input.GetKeyDown("space"))
            {
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

                // 番兵だったら会話を終了し、CurrentConversationを初期化
                if (CurrentConversation.id == "FINISH")
                {
                    CurrentConversation = null;
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
            CanTalk = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CanTalk = false;
            // MEMO : 以下は初期化だが、会話中プレイヤーを操作できないようにすれば要らない
            CurrentConversation = null;
            TextBox.text = "";
            selectManager.ChangeColorDown(SelectNum);
            dialogController.Hide(Options[0]);
            dialogController.Hide(Options[1]);
        }
    }
}
