using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

/// <summary>
/// ゲーム内のクエストデータをロードし、一覧としてまとめる
/// </summary>
public class QuestsManager : SingletonMonoBehaviour<QuestsManager>,ILoadableAsync
{
    [SerializeField] private AssetLabelReference _labelReference;
    [SerializeField,ReadOnly] List<QuestDataSO> m_quests = new List<QuestDataSO>();
    /// <summary>
    /// クエストの名前からQuestDataSOを返す
    /// </summary>
    /// <param name="questName"></param>
    /// <returns></returns>
    public QuestDataSO GetQuest(string questName)
    {
        foreach(var i in m_quests)
        {
            if (i.name == questName) return i;
        }
            Debug.LogWarning($"'{questName}' not found");
        return null;
    }

    AsyncOperationHandle<IList<QuestDataSO>> m_handle;
    void Awake()
    {
        Kyoichi.GameManager.Instance.AddLoadableAsync(this);
    }
    void OnDisable()
    {
        Addressables.Release(m_handle);
    }
    public async Task LoadAsync(CancellationToken token)
    {
        m_handle = Addressables.LoadAssetsAsync<QuestDataSO>(_labelReference, null);
        await m_handle.Task;
        Debug.Log("<color=#4a19bd>Quest loading</color>");
        foreach (var res in m_handle.Result)
        {
            m_quests.Add(res);
        }
    }

}
