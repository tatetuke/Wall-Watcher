using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerShop : MonoBehaviour
{
    public enum SE_TYPE { TOOL1, TOOL2, TOOL3 }

    public AudioClip[] bgm;
    public AudioClip selectSE;
    public AudioClip backSE;
    public AudioClip decideSE;

    [SerializeField]
    [Range(0, 1)]
    private float bgmVolume = 1.0f;
    [SerializeField]
    [Range(0, 1)]
    private float seVolume = 1.0f;

    public AudioSource bgmAudioSource;
    public AudioSource seAudioSource;



    /// <summary>
    /// BGMの再生
    /// </summary>
    /// <param name="num">再生するBGMの選択</param>
    public void PlayBGM(int num = 0)
    {
        if (num >= 0)
        {
            bgmAudioSource.clip = bgm[num];
            bgmAudioSource.loop = true;
            bgmAudioSource.volume = bgmVolume;
            bgmAudioSource.Play();
        }

    }

    public void StopBGM()
    {
        bgmAudioSource.Stop();

    }

    public void PlaySelectSE()
    {
        seAudioSource.PlayOneShot(selectSE);
    }

    public void PlayDecideSE()
    {
        seAudioSource.PlayOneShot(decideSE);
    }

    public void PlayBackSE()
    {
        seAudioSource.PlayOneShot(backSE);
    }


}
