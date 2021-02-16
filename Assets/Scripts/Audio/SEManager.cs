using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Cysharp.Threading.Tasks;

public class SEManager : SingletonMonoBehaviour<SEManager>, IAudio
{
    [SerializeField] private AudioSource m_AudioSource = default;
    [SerializeField] private AssetLabelReference m_LabelReference;
    [Header("almost unused")]
    [SerializeField] private List<AudioClip> m_AudioClips = default;

    AsyncOperationHandle<IList<AudioClip>> m_handle;
    public Dictionary<string, AudioClip> m_AudioClipDictionary = new Dictionary<string, AudioClip>();

    public enum SEType
    {
        FOOTSTEPS,
        ESCAPE,
    }

    private void Start()
    {
        LoadAll().Forget();
    }

    private void Update()
    {
        //テスト用
        if (Input.GetKeyDown(KeyCode.UpArrow))
            Play("walking_on_floor1");
        if (Input.GetKeyDown(KeyCode.DownArrow))
            Play("se_maoudamashii_se_escape");
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
    public async UniTask<bool> Load(string audioClipId)
    {
        Debug.Log("try SE load", gameObject);
        m_handle = Addressables.LoadAssetsAsync<AudioClip>(audioClipId, null);
        await m_handle.Task;
        foreach (var res in m_handle.Result)
        {
            m_AudioClipDictionary.Add(res.name, res);
            Debug.Log($"Load SE: '{res.name}'");
        }
        bool result = (m_handle.Result.Count == 0) ? false : true;
        Addressables.Release(m_handle);

        return result;
    }
    /// <summary>Addressablesに含まれるSEラベルのaudioClipをすべてロードして辞書に加える　ロードの可否を返す</summary>
    public async UniTask<bool> LoadAll()
    {
        Debug.Log("try SE load", gameObject);
        m_handle = Addressables.LoadAssetsAsync<AudioClip>(m_LabelReference, null);
        await m_handle.Task;
        foreach (var res in m_handle.Result)
        {
            m_AudioClipDictionary.Add(res.name, res);
            Debug.Log($"Load SE: '{res.name}'");
        }
        bool result = (m_handle.Result.Count == 0) ? false : true;
        Addressables.Release(m_handle);

        return result;
    }

    /// <summary>enum型で指定されたaudioClipを再生する</summary>
    public void Play(SEType type)
    {
        Debug.Log(type + "is playing");
        if ((int)type >= m_AudioClips.Count)
            return;

        //AudioClip selected = m_AudioClips[(int)type]; //インスペクター上で指定したaudioClipで指定する方法

        //AddressableでロードしたaudioClipを指定する方法
        AudioClip selected = null;
        switch (type)
        {
            case SEType.FOOTSTEPS:
                selected = m_AudioClipDictionary["walking_on_floor1_edit3"];
                break;
            case SEType.ESCAPE:
                selected = m_AudioClipDictionary["se_maoudamashii_se_escape"];
                break;
            default:
                break;
        }
        m_AudioSource.PlayOneShot(selected);
    }

    /// <summary>string型のIdで指定されたaudioClipを再生する</summary>
    public void Play(string SEId)
    {
        if (!m_AudioClipDictionary.ContainsKey(SEId))
            return;

        m_AudioSource.PlayOneShot(m_AudioClipDictionary[SEId]);
    }

}
