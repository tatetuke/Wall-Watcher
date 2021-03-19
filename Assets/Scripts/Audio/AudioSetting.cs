using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 音量設定画面でスライダーから呼び出される
/// </summary>
public class AudioSetting : MonoBehaviour
{
    [SerializeField] private List<float> spareVolumes;

    public void SetMasterVolume(float volume)
    {
        AudioMixerManager.Instance.SetVolume("Master", Value2Db(volume));
    }

    public void SetBGMVolume(float volume)
    {
        AudioMixerManager.Instance.SetVolume("BGM", Value2Db(volume));
    }

    public void SetSEVolume(float volume)
    {
        AudioMixerManager.Instance.SetVolume("SE", Value2Db(volume));
    }

    private float Value2Db(float volume)
    {
        return AudioMixerManager.Instance.ConvertValue2dB(volume);
    }

    //まだ使われてない
    
    /// <summary>現状の音量設定をとっておく</summary>
    public void SaveTemporaryVolumes()
    {
        spareVolumes = AudioMixerManager.Instance.GetVolumes();
    }

    /// <summary>とっておいた音量設定に戻す</summary>
    public void RevertVolumeChange()
    {
        AudioMixerManager.Instance.SetVolumes(spareVolumes);
    }

}
