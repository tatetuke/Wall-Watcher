using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using RPGM.Core;
using RPGM.Gameplay;

public class ConversationDataManager : SingletonMonoBehaviour<ConversationDataManager> ,ILoadableAsync
{
    [SerializeField] private AssetLabelReference _labelReference;
    [SerializeField] Text TextBox;

    public GameObject[] Options;
    public Text[] Texts;

    private Text ButtonAText, ButtonBText;
    DialogController dialogController;

    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;
    string id;
    //string FirstText;
    private bool IsTalk = false;

    private void Awake()
    {
        //base.Awake();
        SaveLoadManager.Instance.SetLoadable(this);
    }

    private void Start()
    {
        dialogController = new DialogController();
    }

    AsyncOperationHandle<IList<ConversationData>> m_handle;
    public Dictionary<string,ConversationData> m_data = new Dictionary<string, ConversationData>();

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
        id = "test";
        //FirstText = TextBox.text;
        CurrentConversationData = GetConversation(id);
        id = CurrentConversationData.GetFirst();
        CurrentConversation = CurrentConversationData.Get(id);
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
        //if (IsTalk) Texting();
        if (IsTalk && Input.GetKeyDown(KeyCode.Space))
        {
            ProceedTalk();
        }

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsTalk = true;
            Debug.Log("NPCと接近!");

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {

            IsTalk = false;
            id = CurrentConversationData.GetFirst();
            CurrentConversation = CurrentConversationData.Get(id);
            //元からテキストボックスに入力されていた文字を再度表示
            //TextBox.text = FirstText;
            Debug.Log("NPCと離れた!");
        }
    }


    /// <summary>
    /// 文章を表示します。
    /// スペースキーが押されたときに文章を送ります。
    /// </summary>
    private void ProceedTalk()
    {
        // TODO : 最後の会話の処理

        // 会話の内容の更新
        TextBox.text = CurrentConversation.text;

        // Branchesからテキストを抽出
        if (CurrentConversation.options.Count == 0)  // 選択肢がない場合 : 選択肢は隠す
        {
            dialogController.Hide(Options[0]);
            dialogController.Hide(Options[1]);
        }
        else                                         // 選択肢がある場合 : 選択肢を表示する
        {
            dialogController.Display(Options[0]);
            dialogController.Display(Options[1]);
            int itr = 0;
            // 選択肢の内容の更新
            foreach (var option in CurrentConversation.options)
            {
                Debug.Log(option.text);
                dialogController.SetText(Texts[itr], option.text);
                itr++;
            }
        }

        // CurrentConversationの更新
        id = CurrentConversation.targetID;
        CurrentConversation = CurrentConversationData.Get(id);
    }
}
