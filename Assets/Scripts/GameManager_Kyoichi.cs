using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// セーブ・ロードをテストするLoadTestシーンのマネージャー
/// </summary>
public class GameManager_Kyoichi : SingletonMonoBehaviour<GameManager_Kyoichi>
{
   enum GameState
    {
        nothing,
        loading,
        running,
        pause
    }
    GameState m_state = GameState.nothing;
    public UnityEvent OnPauseStart { get; } = new UnityEvent();
    public UnityEvent OnPauseEnd { get; } = new UnityEvent();
    // Start is called before the first frame update
    void Start()
    {
        //データをロードするときはSaveLoadManager.LoadをStartもしくはUpdate内で行ってください。
        //Awakeでは行わないよう
        SaveLoadManager.Instance.Load();
        m_state = GameState.loading;
        SaveLoadManager.Instance.OnLoadFinished.AddListener(() =>
        {
            m_state = GameState.running;
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (m_state == GameState.running)
            {
                m_state = GameState.pause;
                OnPauseStart.Invoke();
            }
            else if(m_state==GameState.pause)
            {
                m_state = GameState.running;
                OnPauseEnd.Invoke();
            }
        }
    }

    private void OnApplicationQuit()
    {
        //ゲームを終了したときに自動でセーブされるようになってます
        SaveLoadManager.Instance.Save();
    }
}
