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
        [SerializeField,ReadOnly]
        GameState m_state = GameState.nothing;
        public UnityEvent OnPauseStart { get; } = new UnityEvent();
        public UnityEvent OnPauseEnd { get; } = new UnityEvent();
        // Start is called before the first frame update
        void Start()
        {
            //データをロードするときはSaveLoadManager.LoadをStartもしくはUpdate内で行ってください。
            //Awakeでは行わないよう
            Debug.Log("Loading properties");
            PropertyLoader.Instance.LoadProperty();
            //SaveLoadManager.Instance.Load().Wait();
            //Waitするとロードしなくなる（Start内でAddressable.Wait()やろうとするといつまでたっても完了しないっぽい）
            Debug.Log("Player Data Loading...");
            SaveLoadManager.Instance.Load();
            SaveLoadManager.Instance.LoadAsync();
            m_state = GameState.loading;
            SaveLoadManager.Instance.OnLoadFinished.AddListener(() =>
            {
                m_state = GameState.running;
            });
            FindObjectOfType<CanvasManager>().OnCloseCanvas.AddListener(() =>
            {
                m_state = GameState.running;
                OnPauseEnd.Invoke();
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
            Debug.Log("Player Data Saving...");
            SaveLoadManager.Instance.Save();
            SaveLoadManager.Instance.SaveAsync().Wait();
            PropertyLoader.Instance.SaveProperty();
        }
    }

}