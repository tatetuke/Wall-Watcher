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
        public enum GameState
        {
            nothing,
            loading,
            running,
            pause
        }
        [SerializeField,ReadOnly]
        GameState m_state = GameState.nothing;

        public GameState State { get => m_state; }

        public UnityEvent OnPauseStart { get; } = new UnityEvent();
        public UnityEvent OnPauseEnd { get; } = new UnityEvent();
        public UnityEvent OnGameLoad { get; }=new UnityEvent();
        public UnityEvent OnLoadFinished { get; }=new UnityEvent();
        public UnityEvent OnGameSave { get; }= new UnityEvent();
        public UnityEvent OnSaveFinished { get; } = new UnityEvent();
        public UnityEvent OnRoomChanged { get; } = new UnityEvent();

        public bool IsLoadFinished { get; private set; } = false;
        public bool IsSaveFinished { get; private set; } = false;

        private CancellationTokenSource loadCancellationTokenSource;
        private CancellationTokenSource saveCancellationTokenSource;

        private void Awake()
        {
            loadCancellationTokenSource = new CancellationTokenSource();
            saveCancellationTokenSource = new CancellationTokenSource();
            DontDestroyOnLoad(gameObject);
        }


        // Start is called before the first frame update
        async void Start()
        {
            //データをロードするときはSaveLoadManager.LoadをStartもしくはUpdate内で行ってください。
            //Awakeでは行わないよう
            m_state = GameState.loading;
            Debug.Log("Loading properties");
            IsLoadFinished = false;
            IsSaveFinished = false;

            OnGameLoad.Invoke();
            await SaveLoadManager.Instance.LoadAllAsync();
            m_state = GameState.running;
            IsLoadFinished = true;
            OnLoadFinished.Invoke();

            OnPauseStart.AddListener(() =>
            {
                FindObjectOfType<Player>()?.ChangeState(Player.State.FREEZE);
            });
            OnPauseEnd.AddListener(() =>
            {
                FindObjectOfType<Player>()?.ChangeState(Player.State.IDLE);
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
                    PauseEnd();
                }
            }
        }

        //ゲームを終了したときに自動でセーブされるようになってます
       async private void OnApplicationQuit()
        {
            Debug.Log("Player Data Saving...");
            OnGameSave.Invoke();
            await SaveLoadManager.Instance.SaveAllAsync();
            OnSaveFinished.Invoke();
            IsSaveFinished = false;
        }
        public void PauseEnd()
        {
            m_state = GameState.running;
            OnPauseEnd.Invoke();
        }

        public void OnSceneChanged()
        {

        }
    }

}