using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// ゲームのポーズを管理
/// ポーズしたとき、再開するときのコールバック
/// </summary>
public class PauseManager : SingletonMonoBehaviour<PauseManager>
{
    public UnityEvent OnPauseEnter { get; } = new UnityEvent();
    public UnityEvent OnPauseExit { get; } = new UnityEvent();

    //ポーズしてるかどうか
    [SerializeField,ReadOnly]bool pause = false;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!pause)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    /// <summary>
    /// ポーズさせる
    /// </summary>
    public void Pause()
    {
        OnPauseEnter.Invoke();
        pause = true;
    }

    /// <summary>
    /// 再開させる
    /// </summary>
    public void Resume()
    {
        OnPauseExit.Invoke();
        pause = false;
    }

}
