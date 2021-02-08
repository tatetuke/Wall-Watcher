using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// バフ・デバフを管理するクラス
/// </summary>
public class Buf
{
    /// <summary>
    /// 係数を返す
    /// </summary>
    /// <returns></returns>
    public float GetA() { return 1.0f; }
    public float GetB() { return 0.0f; }

    float m_time;
    public void Update() {
        if (m_time > m_duration) OnFinished.Invoke();
        if (Mathf.Approximately(m_time % m_functionInterval, 0f))
        {
            OnUpdateInterval.Invoke();
        }
        m_time += Time.deltaTime;
    }

    public float m_duration;
    public float m_functionInterval;

    UnityEvent OnFinished = new UnityEvent();
    UnityEvent OnUpdateInterval = new UnityEvent();
    public void Kill()
    {
        OnFinished.Invoke();
    }

}
