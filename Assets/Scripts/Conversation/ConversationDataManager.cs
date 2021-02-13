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
  
    private Conversations CurrentConversation;
    private ConversationData CurrentConversationData;
    string id;
    string FirstText;
    private bool IsTalk = false;

    private void Awake()
    {
        //base.Awake();
        SaveLoadManager.Instance.SetLoadable(this);
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
        FirstText = TextBox.text;
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
        if (IsTalk) Texting();
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
            TextBox.text = FirstText;
            Debug.Log("NPCと離れた!");
        }
    }


    /// <summary>
    /// 文章を表示します。
    /// スペースキーが押されたときに文章を送ります。
    /// </summary>
    private void Texting()
    {
       
        TextBox.text = CurrentConversation.text;
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("スペースキーが押されました");
            id = CurrentConversation.targetID;
            CurrentConversation = CurrentConversationData.Get(id);

        }
    }
}
