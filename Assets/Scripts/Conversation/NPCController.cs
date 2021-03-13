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
/// 当たり判定と会話データの取得を管理します。
/// 喋らないNPCであればIs Talk NPCのチェックを外してください。
/// </summary>
public class NPCController : MonoBehaviour, ILoadableAsync
{
    //追加したコード********************************************************************************************************************
    [SerializeField] private AssetLabelReference _labelReference;
    [HideInInspector] public ConversationData CurrentConversationData;
    [HideInInspector]  public string FileId;
    [HideInInspector] string ConversationDataFolderPath;
    [HideInInspector] string[] Files;
    [HideInInspector] public List<string> ConversationDataList;

    [SerializeField] private bool IsTalkNPC = true;

    private void Awake()
    {
        //base.Awake();
        //SaveLoadManager.Instance.SetLoadable(this);
        Kyoichi.GameManager.Instance.AddLoadableAsync(this);
    }


    [System.Obsolete]
    private void Start()
    {
        /// <summary>
        /// 指定したフォルダからConversationDataを全て取ってくる
        /// </summary>
        ConversationDataList = new List<string>();
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

    public async Task LoadAsync(CancellationToken cancellationToken)
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

    //追加したコード********************************************************************************************************************



    private void OnTriggerEnter2D(Collider2D other)
    {
        if (IsTalkNPC)
        {
            if (other.gameObject.tag == "Player")
            {
                //タグの変更.SearchNearNPCで使われる.
                this.tag = "CanConversationNPC";
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (IsTalkNPC)
        {
            if (collision.gameObject.tag == "Player")
            {
                //タグの変更.SearchNearNPCで使われる.
                this.tag = "NPC";
            }
        }
    }

    public int GetConversationIndex()
    {
        int res = Random.value >= 0.5 ? 0 : 1;
        return res;
    }
}
