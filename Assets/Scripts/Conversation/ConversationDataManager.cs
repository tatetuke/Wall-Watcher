using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class ConversationDataManager : SingletonMonoBehaviour<ConversationDataManager> ,ILoadableAsync
{
    [SerializeField] private AssetLabelReference _labelReference;

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

        return;
    }

    public ConversationData GetConversation(string ID)
    {
        if (!m_data.ContainsKey(ID)) return null;
        return m_data[ID];
    }
}
