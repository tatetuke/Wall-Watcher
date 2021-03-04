using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

/// <summary>
/// 効果音の再生・停止などを行う
/// 同時に再生されるSEは3つ以上になり、
/// 音源を複数持つためそれらの管理も行う(予定）
/// </summary>
public class SEManager : SingletonMonoBehaviour<SEManager>, IAudio
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioTable m_AudioTable;

    private AsyncOperationHandle<IList<AudioClip>> m_handle;
    public Dictionary<string, AudioClip> m_AudioClipDictionary = new Dictionary<string, AudioClip>();


    private void Start()
    {
        //Load("escape").Forget();
        Play("aaa");
        Play("footsteps");
    }

    private void Update()
    {
        ////テスト用
        //if (Input.GetKeyDown(KeyCode.F))
        //    Play("escape");
        //if (Input.GetKeyDown(KeyCode.G))
        //    Play("footsteps");

    }

    public void Stop()
    {
        m_AudioSource.Stop();
    }

    public bool IsPlaying()
    {
        return m_AudioSource.isPlaying;
    }

    /// <summary>指定したIdのaudioClipをロードして辞書に加える　ロードの可否を返す</summary>
    public async UniTask<bool> Load(string audioId)
    {
        Debug.Log("try SE load", gameObject);

        var audioPiece = m_AudioTable.GetPiece(audioId);
        //ロード失敗
        if (audioPiece == null)
        {
            Debug.LogWarning($"<color=red>The audio </color>'{audioId}' <color=red>is not found</color>");
            return false;
        }
        //ロード済み
        if (m_AudioClipDictionary.ContainsKey(audioPiece.id))
        {
            //Debug.Log("Already loaded");
            return true;
        }

        m_handle = Addressables.LoadAssetsAsync<AudioClip>(audioPiece.reference, null);
        await m_handle.Task;
        //一つのみのはず
        foreach (var res in m_handle.Result)
        {
            m_AudioClipDictionary.Add(audioPiece.id, res);
            Debug.Log($"Load SE(id, file): '{audioPiece.id}', '{res.name}'");
            Debug.Log($"Loaded audio: {m_AudioClipDictionary.Count}");
        }
        bool result = (m_handle.Result.Count == 0) ? false : true;
        Addressables.Release(m_handle);
        return result;
    }

    /// <summary>string型のIdで指定されたaudioClipを再生する 再生完了などが検知できる</summary>
    public async UniTask PlayTask(string SEId)
    {
        //ロード済みでない
        if (!m_AudioClipDictionary.ContainsKey(SEId))
        {
            bool loadResult = await Load(SEId);
            if (loadResult == false)
                return;
        }

        m_AudioSource.PlayOneShot(m_AudioClipDictionary[SEId]);
    }



    /// <summary>string型のIdで指定されたaudioClipを再生する</summary>
    /// <param name="SEId">Assets/Data/AudioTable_SEの中身を参考にすること</param>
    public async void Play(string SEId)
    {
        //ロード済みでない
        if (!m_AudioClipDictionary.ContainsKey(SEId))
        {
            bool loadResult = await Load(SEId);
            if (loadResult == false)
                return;
            
        }

        m_AudioSource.PlayOneShot(m_AudioClipDictionary[SEId]);
    }

    public void Play(AssetReferenceAudio assetReferenceAudio)
    {
        
    }

    public void ClearDictionary()
    {
        m_AudioClipDictionary.Clear();
    }

}
