using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SEManager : MonoBehaviour, IAudio
{
    [SerializeField] private AudioSource m_AudioSource = default;
    [SerializeField] private List<AudioClip> m_AudioClips = default;

    public enum SEType
    {
        FOOTSTEPS,
        ESCAPE,
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
            Play(SEType.ESCAPE);
        if (Input.GetKeyDown(KeyCode.LeftArrow))
            Play(SEType.FOOTSTEPS);
    }

    public void Stop()
    {
        m_AudioSource.Stop();
    }

    public bool IsPlaying()
    {
        return m_AudioSource.isPlaying;
    }

    public void Play(SEType type)
    {
        Debug.Log(type + "is playing");
        if ((int)type >= m_AudioClips.Count)
            return;

        m_AudioSource.PlayOneShot(m_AudioClips[(int)type]);

        switch (type)
        {
            case SEType.FOOTSTEPS:
                break;
            case SEType.ESCAPE:
                break;
            default:
                break;
        }
    }


}
