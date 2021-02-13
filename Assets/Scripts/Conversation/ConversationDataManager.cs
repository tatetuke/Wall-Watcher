using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ConversationDataManager : SingletonMonoBehaviour<ConversationDataManager> //,ILoadableAsync
{
   new void Awake()
    {
        // base.Awake();
        // SaveLoadManager.Instance.SetLoadable(this);
    }

    AsyncOperationHandle<IList<ConversationData>> m_handle;
    public Dictionary<string,ConversationData> m_data = new Dictionary<string, ConversationData>();

    //public IEnumerator Load(PlayerDataTable data)
    //{
    //    Debug.Log("try conversation load", gameObject);
    //    //ゲーム内アイテムデータを読み込む
    //    m_handle = Addressables.LoadAssetsAsync<ConversationData>("Event", null);
    //    m_handle.Completed += op =>
    //    {
    //        foreach (var res in op.Result)
    //        {
    //            m_data.Add(res.name, res);
    //            Debug.Log($"Load Conversation: '{res.name}'");
    //        }
    //    };
    //    yield return new WaitUntil(() => m_handle.IsDone);
    //    Addressables.Release(m_handle);
    //}

    public ConversationData GetConversation(string ID)
    {
        if (!m_data.ContainsKey(ID)) return null;
        return m_data[ID];
    }
}
