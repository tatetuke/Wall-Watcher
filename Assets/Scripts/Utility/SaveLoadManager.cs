using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// オブジェクトのセーブ、ロードを一括で行える管理クラス
/// </summary>
sealed public class SaveLoadManager : SingletonMonoBehaviour<SaveLoadManager>
{
    private void Start()
    {
        Load();
    }
    Queue<ISaveable> m_saveables = new Queue<ISaveable>();
    Queue<ILoadable> m_loadables = new Queue<ILoadable>();
    Queue<ISaveableAsync> m_saveablesAsync = new Queue<ISaveableAsync>();
    Queue<ILoadableAsync> m_loadablesAsync = new Queue<ILoadableAsync>();

    /// <summary>
    /// GamaManager.Start()でデータがロードされるので、Awake内で行ってください。
    /// </summary>
    /// <param name="item"></param>
    public void SetLoadable(ILoadable item) { m_loadables.Enqueue(item); }

    /// <summary>
    /// GamaManager.Start()でデータがロードされるので、Awake内で行ってください。
    /// </summary>
    /// <param name="item"></param>
    public void SetLoadable(ILoadableAsync item) { m_loadablesAsync.Enqueue(item); }

    /// <summary>
    /// GamaManager.Start()でデータがロードされるので、Awake内で行ってください。
    /// </summary>
    /// <param name="item"></param>
    public void SetSaveable(ISaveable item) { m_saveables.Enqueue(item); }

    /// <summary>
    /// GamaManager.Start()でデータがロードされるので、Awake内で行ってください。
    /// </summary>
    /// <param name="item"></param>
    public void SetSaveable(ISaveableAsync item) { m_saveablesAsync.Enqueue(item); }

    /// <summary>
    /// セーブする関数。
    /// StartもしくはUpdate内で呼んでください。
    /// Awake、OnEnableで呼ぶとSaveできないオブジェクトが発生する可能性があります。
    /// </summary>
    /// <returns></returns>
    public async Task Save()
    {
        Debug.Log("Player Data Saving...");
        while (m_saveables.Count > 0)
        {
            var obj = m_saveables.Peek();
            obj.Save();
            m_saveables.Dequeue();
        }
        await SaveAsync();//非同期でセーブし、すべてのオブジェクトについて完了するまで待つ
        Debug.Log("All Data Saving Finished");
    }

    /// <summary>
    /// ロードする関数。
    /// StartもしくはUpdate内で呼んでください。
    /// Awake、OnEnableで呼ぶとLoadできないオブジェクトが発生する可能性があります。
    /// </summary>
    /// <returns></returns>
    public async Task Load()
    {
        Debug.Log("Player Data Loading...");
       // GamePropertyManager.Instance.LoadProperty();
        while (m_loadables.Count > 0)
        {
            var obj = m_loadables.Peek();
            obj.Load();
            m_loadables.Dequeue();
        }
        await LoadAsync();//非同期でロードし、すべてのオブジェクトについて完了するまで待つ
        Debug.Log("All Data Loading Finished");
    }


    private CancellationTokenSource loadCancellationTokenSource;
    private CancellationTokenSource saveCancellationTokenSource;
    private void Awake()
    {
        loadCancellationTokenSource = new CancellationTokenSource();
        saveCancellationTokenSource = new CancellationTokenSource();
    }
    async Task LoadAsync()
    {
        while (m_loadablesAsync.Count > 0)
        {
            var obj = m_loadablesAsync.Peek();
            await obj.Load(loadCancellationTokenSource.Token);
            m_loadablesAsync.Dequeue();
        }
    }
    async Task SaveAsync()
    {
        while (m_saveablesAsync.Count > 0)
        {
            var obj = m_saveablesAsync.Peek();
            await obj.Save(saveCancellationTokenSource.Token);
            m_saveablesAsync.Dequeue();
        }
    }

}
