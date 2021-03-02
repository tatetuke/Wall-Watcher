using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerScript
{
    public UnityEvent OnStart { get; } = new UnityEvent();
    public UnityEvent OnSeconds { get; } = new UnityEvent();
    public UnityEvent OnFinished { get; } = new UnityEvent();

    float m_time = 0f;
    float m_endTime = 0f;
   // bool m_isReversed = false;
    float m_speed = 1f;
    bool m_isRunning = false;
    public void StartTimer(float beginTime, float endTime)
    {
        m_isRunning = true;
        m_endTime = endTime;
        m_time = beginTime;
        if (beginTime > endTime)
        {
            //m_isReversed = true;
            m_speed = -1f;
        }
        else
        {
            //m_isReversed = false;
            m_speed = 1f;
        }
        OnStart.Invoke();
    }

    public void UpdateTimer()
    {
        if (!m_isRunning) return;
        if (Mathf.Approximately(m_time, m_endTime))
        {
            m_isRunning = false;
            OnFinished.Invoke();
        }
        m_time += Time.deltaTime * m_speed;
    }
    public float GetTime() { return m_time; }
    public int getMinute() { return (int)(m_time / 60f); }
    public float getSecond() { return m_time % 60f; }

}
