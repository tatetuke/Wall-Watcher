using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class BGMManager : SingletonMonoBehaviour<BGMManager>, IAudio
{
    [SerializeField] private AudioSource m_AudioSource = default;
    [SerializeField] private AssetLabelReference m_LabelReference;

    public void Stop()
    {
        m_AudioSource.Stop();
    }

    public bool IsPlaying()
    {
        return m_AudioSource.isPlaying;
    }


}
