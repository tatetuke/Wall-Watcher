//https://qiita.com/matsumotokaka11/items/2eb0e7ac34c6dec3f80b
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;
using DG.Tweening;

/// <summary>
/// 全体ボリューム、個別ボリューム、音のフェードイン・アウトを管理する
/// TODO フェードイン・アウトでマックスボリュームが変更されてしまう
/// </summary>
public class AudioMixerManager : SingletonMonoBehaviour<AudioMixerManager>
{
    [SerializeField] AudioMixer m_AudioMixer = default;
    [SerializeField] FadeState m_FadeState = FadeState.NONE;

    public enum FadeState
    {
        NONE,
        FADEIN,
        FADEOUT,
    }

    // Start is called before the first frame update
    private void Start()
    {

    }

    private void Update()
    {
        ////テスト用
        if (Input.GetKeyDown(KeyCode.I))
            DoFadeIn("Master", 16);
        if (Input.GetKeyDown(KeyCode.O))
            DoFadeOut("Master", 16);
    }

    /// <summary>ボリュームをデシベル単位で指定する</summary>
    /// <param name="GroupName">Master or BGM or SE</param>
    /// <param name="volumeDb">-80 ~ 20デシベル</param>
    public void SetVolume(string GroupName, float volumeDb)
    {
        m_AudioMixer.SetFloat(GroupName, volumeDb);
    }

    /// <summary>フェードイン</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    /// <returns></returns>
    public DG.Tweening.Core.TweenerCore<float, float,DG.Tweening.Plugins.Options.FloatOptions> DoFadeIn(string groupName, float fadeTime)
    {
        return m_AudioMixer.DOSetFloat(groupName, ConvertVolume2dB(1), fadeTime);
    }

    /// <summary>フェードアウト</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    /// <returns></returns>
    public DG.Tweening.Core.TweenerCore<float, float, DG.Tweening.Plugins.Options.FloatOptions> DoFadeOut(string groupName, float fadeTime)
    {
        return m_AudioMixer.DOSetFloat(groupName, ConvertVolume2dB(0), fadeTime);
    }

    /// <summary>フェードイン Taskの方</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    private async UniTask FadeIn(string groupName, float fadeTime)
    {
        float elapsedTime = 0;
        //既に開始していたら
        if (m_FadeState == FadeState.FADEIN)
            return;
        m_FadeState = FadeState.FADEIN;
        do
        {
            //0~1からデシベルへ変換する
            float volume = ConvertVolume2dB(elapsedTime / fadeTime);
            SetVolume(groupName, volume);

            await UniTask.DelayFrame(1);
            elapsedTime += Time.deltaTime;
        } while (elapsedTime <= fadeTime && m_FadeState == FadeState.FADEIN);
        //割り込みされずにフェードが終了したら
        if (m_FadeState == FadeState.FADEIN)
            m_FadeState = FadeState.NONE;
    }

    /// <summary>フェードアウト</summary>
    /// <param name="groupName">Master or BGM or SE</param>
    private async UniTask FadeOut(string groupName, float fadeTime)
    {
        float elapsedTime = 0;
        //既に開始していたら
        if (m_FadeState == FadeState.FADEOUT)
            return;
        m_FadeState = FadeState.FADEOUT;
        do
        {
            //0~1からデシベルへ変換する
            float volume = ConvertVolume2dB(1 - elapsedTime / fadeTime);
            SetVolume(groupName, volume);

            await UniTask.DelayFrame(1);
            elapsedTime += Time.deltaTime;
        } while (elapsedTime <= fadeTime && m_FadeState == FadeState.FADEOUT);
        //割り込みされずにフェードが終了したら
        if (m_FadeState == FadeState.FADEOUT)
            m_FadeState = FadeState.NONE;
    }


    /// <summary>0~1の値を-80~0デシベルの値に変換する</summary>
    private float ConvertVolume2dB(float volume)
    {
        return Mathf.Clamp(20f * Mathf.Log10(Mathf.Clamp(volume, 0f, 1f)), -80f, 0f);
        //return -80 + volume * 80; //線形
        
    }

    private async void Test()
    {
        //await FadeIn("SE", 4);
        //await FadeOut("Master", 4);
        //await FadeIn("Master", 4);
        DoFadeIn("SE", 4);
        DoFadeOut("Master", 4);
        DoFadeIn("Master", 4);
    }
}
