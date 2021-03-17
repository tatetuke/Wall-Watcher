using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
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
        List<ISaveableAsync> m_saveablesAsync = new List<ISaveableAsync>();
        List<ILoadableAsync> m_loadablesAsync = new List<ILoadableAsync>();

        public UnityEvent OnPauseStart { get; } = new UnityEvent();
        public UnityEvent OnPauseEnd { get; } = new UnityEvent();
        public UnityEvent OnGameLoad { get; }=new UnityEvent();
        public UnityEvent OnLoadFinished { get; }=new UnityEvent();
        public UnityEvent OnGameSave { get; }= new UnityEvent();
        public UnityEvent OnSaveFinished { get; } = new UnityEvent();

        public bool IsLoadFinished { get; private set; } = false;
        public bool IsSaveFinished { get; private set; } = false;

        public void AddLoadableAsync(ILoadableAsync obj) => m_loadablesAsync.Add(obj);
        public void AddSaveableAsync(ISaveableAsync obj) => m_saveablesAsync.Add(obj);

        private void Awake()
        {

            loadCancellationTokenSource = new CancellationTokenSource();
            saveCancellationTokenSource = new CancellationTokenSource();
        }

        private CancellationTokenSource loadCancellationTokenSource;
        private CancellationTokenSource saveCancellationTokenSource;

        async Task LoadAsync()
        {
            foreach(var i in m_loadablesAsync)
            {
                await i.LoadAsync(loadCancellationTokenSource.Token);
            }
            //非同期でロードし、すべてのオブジェクトについて完了するまで待つ
            Debug.Log("All Data Loading Finished");
            OnLoadFinished.Invoke();
        }
        async Task SaveAsync()
        {
            foreach (var i in m_saveablesAsync)
            {
                await i.SaveAsync(saveCancellationTokenSource.Token);
            }
            //非同期でロードし、すべてのオブジェクトについて完了するまで待つ
            Debug.Log("All Data Save Finished");
            OnSaveFinished.Invoke();
        }

        // Start is called before the first frame update
        void Start()
        {
            //データをロードするときはSaveLoadManager.LoadをStartもしくはUpdate内で行ってください。
            //Awakeでは行わないよう
            m_state = GameState.loading;
            Debug.Log("Loading properties");
            PropertyLoader.Instance.LoadProperty();
            //SaveLoadManager.Instance.Load().Wait();
            //Waitするとロードしなくなる（Start内でAddressable.Wait()やろうとするといつまでたっても完了しないっぽい）
            Debug.Log("Player Data Loading...");
            IsLoadFinished = false;
            IsSaveFinished = false;
            OnLoadFinished.AddListener(() =>
            {
                m_state = GameState.running;
                IsLoadFinished = true;
            });
            OnSaveFinished.AddListener(() =>
            {
                IsSaveFinished = true;
            });
            OnGameLoad.Invoke();
            LoadAsync();
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
            OnGameSave.Invoke();
            SaveAsync().Wait();
            PropertyLoader.Instance.SaveProperty();
        }
    }

}