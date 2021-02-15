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
    public Text[] OptionTexts;

    private Text ButtonAText, ButtonBText;
    DialogController dialogController;

    Conversations FirstConversation;
    private int SelectNum;
    private Conversations CurrentConversation = null;
    private ConversationData CurrentConversationData;
    string id;
    //string FirstText;
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
        FirstConversation = CurrentConversation;
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
        

        if (CanTalk && IsOptionTalks(CurrentConversation))
        {
            Select();
        }
        else if ( CanTalk && Input.GetKeyDown(KeyCode.Space))
        {
            ProceedTalk();
        }
        //if (!IsOptionTalks(CurrentConversation)&& CanTalk && Input.GetKeyDown(KeyCode.Space))
        //{
        //    ProceedTalk();
        //}


    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            CanTalk = true;
            Debug.Log("NPCと接近!");

        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
  
            CanTalk = false;
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
        if (CurrentConversation != FirstConversation)
        {
            // CurrentConversationの更新
            id = CurrentConversation.targetID;
            CurrentConversation = CurrentConversationData.Get(id);
        }
        // TODO : 最後の会話の処理

        // 会話の内容の更新
        TextBox.text = CurrentConversation.text;

        // Branchesからテキストを抽出
        //if (CurrentConversation.options.Count == 0)  // 選択肢がない場合 : 選択肢は隠す
        //{
            dialogController.Hide(Options[0]);
            dialogController.Hide(Options[1]);
        //}
        //else                                         // 選択肢がある場合 : 選択肢を表示する
        //{
           


        //}

       
    }


    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を薄くする関数
    /// </summary>
    private void ChangeColorDown()
    {
        //黒
        OptionTexts[SelectNum].color = new Color32(0, 0, 0, 100);
    }
    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を濃くする関数
    /// </summary>
    private void ChangeColorUp()
    {
        //黒
        //OptionTexts[SelectNum].color = new Color32(255, 0, 0, 255);
        OptionTexts[SelectNum].color = Color.yellow;
    }

    private void Select()
    {

        TextBox.text = CurrentConversation.text;
        dialogController.Display(Options[0]);
        dialogController.Display(Options[1]);
        int itr = 0;
        // 選択肢の内容の更新
        foreach (var option in CurrentConversation.options)
        {
            Debug.Log(option.text);
            dialogController.SetText(OptionTexts[itr], option.text);
            itr++;
        }
        if (Input.GetKeyDown("left"))
        {
            
            ChangeColorDown();

            SelectNum += CurrentConversation.options.Count;
            SelectNum--;
            SelectNum %= CurrentConversation.options.Count;

            ChangeColorUp();


        }
        if (Input.GetKeyDown("right"))
        {
            ChangeColorDown();

            SelectNum++;
            SelectNum %= CurrentConversation.options.Count;

            ChangeColorUp();

        }


        if (Input.GetKeyDown("space"))
        {
            // CurrentConversationの更新
            id = CurrentConversation.options[SelectNum].targetId;
            CurrentConversation = CurrentConversationData.Get(id);
            SelectNum = 0;
        }
    }

    private bool IsOptionTalks(Conversations conversations)
    {
        if (conversations == null)
        {
            Debug.Log("null");
            return false;
        }
        if (conversations.options.Count == 0) return false; 

        else return true;
        
    }
}
