using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HakaiSoundManager : MonoBehaviour
{
    public enum SE_TYPE { TOOL1,TOOL2,TOOL3 }

    public AudioClip[] bgm;
    public AudioClip[] toolSE;
    public AudioClip[] clearSE;
    public AudioClip[] humerSE;
    public AudioClip[] pickelSE;
    public AudioClip getItemSE;

    [SerializeField]
    [Range(0, 1)]
    private float bgmVolume = 1.0f;
    [SerializeField]
    [Range(0, 1)]
    private float seVolume = 1.0f;

    public AudioSource bgmAudioSource;
    public AudioSource seAudioSource;

    public AudioClip seTurnThePage;
    public AudioClip seOpenTutorial;

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
                int randomValue1 = Random.Range(0, pickelSE.Length);
                seAudioSource.PlayOneShot(pickelSE[randomValue1],seVolume);
                break;

            case SE_TYPE.TOOL2:
                int randomValue2 = Random.Range(0, humerSE.Length);
                seAudioSource.PlayOneShot(humerSE[randomValue2], seVolume);
                break;

            case SE_TYPE.TOOL3:
                seAudioSource.PlayOneShot(toolSE[2], seVolume);
                break;
        }
    }



    public void PlayOpenTutorial()
    {
        seAudioSource.PlayOneShot(seOpenTutorial, seVolume);
        return;
    }

    public void PlayTurnThePage()
    {
        seAudioSource.PlayOneShot(seTurnThePage, seVolume);
        return;
    }

    public void PlayGetItemSound()
    {
        seAudioSource.PlayOneShot(getItemSE, seVolume);

    }
}
