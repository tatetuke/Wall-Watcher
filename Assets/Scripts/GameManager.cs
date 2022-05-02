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
        [SerializeField,ReadOnly] GameState m_state = GameState.nothing;
        public GameState State { get => m_state; }
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
            m_state = GameState.running;
            IsLoadFinished = true;
            OnLoadFinished.Invoke();

            await QuestsManager.Instance.LoadAsync();
            await ItemManager.Instance.LoadAsync();

            Inventry.Instance.LoadFromFile();

            PauseManager.Instance.OnPauseEnter.AddListener(() =>
             {
                 FindObjectOfType<Player>()?.ChangeState(Player.State.FREEZE);
             });
            PauseManager.Instance.OnPauseExit.AddListener(() =>
            {
                FindObjectOfType<Player>()?.ChangeState(Player.State.IDLE);
            });
        }

        //ゲームを終了したときに自動でセーブされるようになってます
       private void OnApplicationQuit()
        {
            Debug.Log("Player Data Saving...");
            Inventry.Instance.SaveToFile();
            OnGameSave.Invoke();
            OnSaveFinished.Invoke();
            IsSaveFinished = false;
        }

        public void OnSceneChanged()
        {

        }
    }

}