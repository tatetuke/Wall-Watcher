using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakaiSoundManager : MonoBehaviour
{
    public enum SE_TYPE { TOOL1,TOOL2,TOOL3 }

    public AudioClip[] bgm;
    public AudioClip[] toolSE;
    public AudioClip[] clearSE;

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
    public void PlayBGM(int num=0)
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



    //SE再生
    public void PlaySE(SE_TYPE seType)
    {
        switch (seType)
        {
            case SE_TYPE.TOOL1:
                seAudioSource.PlayOneShot(toolSE[0], seVolume);
                break;

            case SE_TYPE.TOOL2:
                seAudioSource.PlayOneShot(toolSE[1], seVolume);
                break;

            case SE_TYPE.TOOL3:
                seAudioSource.PlayOneShot(toolSE[2], seVolume);
                break;
        }
    }

    
}
