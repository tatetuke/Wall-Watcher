using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Kyoichi
{
    /// <summary>
    /// セーブ・ロードのタイミングを制御
    /// </summary>
    public class GameManager : SingletonMonoBehaviour<GameManager>
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
            PropertyLoader.Instance.LoadProperty();
            SaveLoadManager.Instance.Load().Wait();
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
                else if (m_state == GameState.pause)
                {
                    m_state = GameState.running;
                    OnPauseEnd.Invoke();
                }
            }
        }

            //ゲームを終了したときに自動でセーブされるようになってます
        private void OnApplicationQuit()
        {
            SaveLoadManager.Instance.Save().Wait();
            PropertyLoader.Instance.SaveProperty();
        }
    }

}