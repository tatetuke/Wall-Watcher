using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
     float end_time;
    [SerializeField,ReadOnly]float m_time;
    public class OnTimerUpdatedEvent : UnityEvent<float> { }
    public OnTimerUpdatedEvent OnTimerUpdated { get; } = new OnTimerUpdatedEvent();
    public UnityEvent OnTimerFinished { get; } = new UnityEvent();
    bool timer_starts = false;
    bool to_minus = false;//時間が減るタイプか
    public void StartTimer(float begin,float end)
    {
        m_time = begin;
        end_time = end;
        to_minus = begin > end;
        timer_starts = true;
    }

    private void Update()
    {
        if (!timer_starts) return;
        if (to_minus)
        {
            if(m_time< end_time)
            {
                OnTimerFinished.Invoke();
            }
            m_time -= Time.deltaTime;
        }
        else
        {
            if (m_time > end_time)
            {
                OnTimerFinished.Invoke();
            }
            m_time += Time.deltaTime;
        }
        OnTimerUpdated.Invoke(m_time);
    }



}
