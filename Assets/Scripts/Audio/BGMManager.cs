using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMManager : MonoBehaviour, IAudio
{
    [SerializeField] private AudioSource m_AudioSource = default;

    public void Stop()
    {
        m_AudioSource.Stop();

    }

    public bool IsPlaying()
    {
        return m_AudioSource.isPlaying;
    }


}
