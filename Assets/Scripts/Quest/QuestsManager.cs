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
    /// <summary>
    /// クエストの名前からQuestDataSOを返す
    /// </summary>
    /// <param name="questName"></param>
    /// <returns></returns>
    public QuestDataSO GetQuest(string questName)
    {
        return null;
    }
    AsyncOperationHandle<IList<QuestDataSO>> m_handle;
    void Start()
    {
        Kyoichi.GameManager.Instance.OnRoomChanged.AddListener(() =>
        {
            LoadCurrentRoomQuests();
        });
    }
    public async Task LoadAsync(CancellationToken token)
    {
        m_handle = Addressables.LoadAssetsAsync<QuestDataSO>(_labelReference, null);
        await m_handle.Task;
        Debug.Log("<color=#4a19bd>Quest loading</color>");
        foreach (var res in m_handle.Result)
        {

        }
    }

    /// <summary>
    /// 現在のマップにあるQuestCheckerをロード
    /// </summary>
    void LoadCurrentRoomQuests()
    {
       var quests= FindObjectsOfType<QuestHolder>();
    }
}
