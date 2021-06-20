using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

/// <summary>
/// BGMの再生・停止・切り替えなどを行う
/// 同時に再生されるBGMは多くて三つなど、少ない（現状一つ）
/// </summary>
public class BGMManager : SingletonMonoBehaviour<BGMManager>, IAudio
{
    [SerializeField] private AudioSource m_AudioSource;
    [SerializeField] private AudioTable m_AudioTable;

    private AsyncOperationHandle<IList<AudioClip>> m_handle;
    public Dictionary<string, AudioClip> m_AudioClipDictionary = new Dictionary<string, AudioClip>();


    private void Start()
    {
        //Load("escape").Forget();
    }

    private void Update()
    {
        //テスト用
        //if (Input.GetKeyDown(KeyCode.R))
        //    Play("bgm01");
        //if (Input.GetKeyDown(KeyCode.T))
        //    Play("bgm02");
        
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
        Debug.Log("try BGM load", gameObject);

        var audioPiece = m_AudioTable.GetPiece(audioId);
        //ロード失敗
        if (audioPiece == null)
        {
            Debug.Log("The audio isnt found");
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

    /// <summary>Idで指定されたaudioClipを再生する 再生完了などが検知できる</summary>
    public async UniTask PlayTask(string SEId)
    {
        //ロード済みでない
        if (!m_AudioClipDictionary.ContainsKey(SEId))
        {
            bool loadResult = await Load(SEId);
            if (loadResult == false)
                return;
        }

        m_AudioSource.clip =  m_AudioClipDictionary[SEId];
        m_AudioSource.Play();
    }

    /// <summary>Idで指定されたaudioClipを再生する</summary>
    public async void Play(string SEId)
    {
        //ロード済みでない
        if (!m_AudioClipDictionary.ContainsKey(SEId))
        {
            bool loadResult = await Load(SEId);
            if (loadResult == false)
                return;
        }

        m_AudioSource.clip = m_AudioClipDictionary[SEId];
        m_AudioSource.Play();
    }

    public void Play(AssetReferenceAudio assetReferenceAudio)
    {

    }

    public void ClearDictionary()
    {
        m_AudioClipDictionary.Clear();
    }


}
