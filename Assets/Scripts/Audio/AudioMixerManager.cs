//https://qiita.com/matsumotokaka11/items/2eb0e7ac34c6dec3f80b
using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using Cysharp.Threading.Tasks;
using DG.Tweening;


/// <summary>
/// 全体ボリューム、個別ボリューム、音のフェードイン・アウトを管理する
/// TODO フェードイン・アウトでマックスボリュームが変更されてしまう
/// TODO マックスボリュームを保存可能にしたい
/// </summary>
public class AudioMixerManager : SingletonMonoBehaviour<AudioMixerManager>
{
    [SerializeField] private AudioMixer m_AudioMixer = default;
    [SerializeField] private FadeState m_FadeState = FadeState.NONE;
    //[SerializeField] private Dictionary<string, float> m_AudioVolumes = new Dictionary<string, float>();
    /// <summary>最大ボリューム(デシベル)</summary>
    [SerializeField] private List<float> m_Volumes = new List<float>();

    public enum FadeState
    {
        NONE,
        FADEIN,
        FADEOUT,
    }

    // Start is called before the first frame update
    private void Awake()
    {
        if (this != Instance)
        {
            Destroy(this);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        m_Volumes = new List<float>() { 0, 0, 0 };
}

    private void Update()
    {
        ////テスト用
        if (Input.GetKeyDown(KeyCode.I))
            DoFadeIn("Master", 4);
        if (Input.GetKeyDown(KeyCode.O))
            DoFadeOut("Master", 4);

    }

    /// <summary>最大ボリュームをデシベル単位で指定する</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    /// <param name="volumeDb">-80 ~ 20デシベル</param>
    public void SetVolume(string groupName, float volumeDb)
    {
        m_Volumes[VolumeIndex(groupName)] = volumeDb;
        m_AudioMixer.SetFloat(groupName, volumeDb);
    }

    public void SetVolumes(List<float> volumes)
    {
        if (volumes.Count != m_Volumes.Count)
            Debug.LogWarning("invalid List was given");
        else 
            m_Volumes = volumes;
    }

    public List<float> GetVolumes()
    {
        return m_Volumes;
    }

    /// <summary>フェードイン 線形にデシベルが大きくなる</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    public DG.Tweening.Core.TweenerCore<float, float,DG.Tweening.Plugins.Options.FloatOptions> DoFadeIn(string groupName, float fadeTime)
    {
        //return m_AudioMixer.DOSetFloat(groupName, m_Volumes[VolumeIndex(groupName)], fadeTime).SetEase(Ease.OutQuint);
        return m_AudioMixer.DOSetFloat(groupName, m_Volumes[VolumeIndex(groupName)], fadeTime).SetEase(Ease.OutExpo);
    }

    /// <summary>フェードアウト 線形にデシベルが小さくなる</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    public DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> DoFadeOut(string groupName, float fadeTime)
    {
        //return m_AudioMixer.DOSetFloat(groupName, ConvertValue2dB(0), fadeTime).SetEase(Ease.InQuint);
        return m_AudioMixer.DOSetFloat(groupName, ConvertValue2dB(0), fadeTime).SetEase(Ease.InExpo);
    }

    /// <summary>0~1の値を-80~0デシベルの値に変換する</summary>
    public float ConvertValue2dB(float volume)
    {
        return Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0);
        //return -80 + volume * 80; //線形
    }

    private int VolumeIndex(string groupName)
    {
        if (groupName == "Master")
            return 0;
        if (groupName == "BGM")
            return 1;
        if (groupName == "SE")
            return 2;

        Debug.LogWarning("invalid groupName");
        return 0;
    }

}
