//https://qiita.com/matsumotokaka11/items/2eb0e7ac34c6dec3f80b
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Cysharp.Threading.Tasks;

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
        //if (Input.GetKeyDown(KeyCode.I))
        //    FadeIn("Master", 4).Forget();
        //if (Input.GetKeyDown(KeyCode.O))
        //    FadeOut("Master", 4).Forget();
    }

    /// <summary>ボリュームをデシベル単位で指定する</summary>
    /// <param name="GroupName">Master or BGM or SE</param>
    /// <param name="volumeDb">-80 ~ 20デシベル</param>
    public void SetVolume(string GroupName, float volumeDb)
    {
        m_AudioMixer.SetFloat(GroupName, volumeDb);
    }



    /// <summary>フェードイン</summary>
    /// <param name="GroupName">Master or BGM or SE</param>
    public async UniTask FadeIn(string GroupName, float fadeTime)
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
            SetVolume(GroupName, volume);

            await UniTask.DelayFrame(1);
            elapsedTime += Time.deltaTime;
        } while (elapsedTime <= fadeTime && m_FadeState == FadeState.FADEIN);
        //割り込みされずにフェードが終了したら
        if (m_FadeState == FadeState.FADEIN)
            m_FadeState = FadeState.NONE;
    }

    /// <summary>フェードアウト</summary>
    /// <param name="GroupName">Master or BGM or SE</param>
    public async UniTask FadeOut(string GroupName, float fadeTime)
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
            SetVolume(GroupName, volume);

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
    }

    private async void Test()
    {
        await FadeIn("SE", 4);
        await FadeOut("Master", 4);
        await FadeIn("Master", 4);
    }
}
