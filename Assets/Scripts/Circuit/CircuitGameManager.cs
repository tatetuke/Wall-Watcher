using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Timer))]
/// <summary>
/// 回路の修理Sceneに配置するスクリプト
/// ゲームの進行を管理
/// </summary>
public class CircuitGameManager : MonoBehaviour
{
    /// <summary>
    /// クリアするために、どういうConnecterがつながっていればいいか
    /// </summary>
    [SerializeField] List<Connector> targetConnecter = new List<Connector>();
    /// <summary>
    /// ゲームに必要なアイテムたち
    /// </summary>
    [SerializeField] List<CircuitSO> requiredItems = new List<CircuitSO>();
    public IEnumerable<CircuitSO> RequiredItems { get => requiredItems; }

    /// <summary>
    /// ゲームを開始したときに実行される
    /// </summary>
    public UnityEvent OnGameStart { get; } = new UnityEvent();
    /// <summary>
    /// ポーズボタンを押したときに実行される
    /// </summary>
    public UnityEvent OnGamePause { get; } = new UnityEvent();
    /// <summary>
    /// ポーズを終了し、修理ゲームに戻るときに実行される
    /// </summary>
    public UnityEvent OnGameResume { get; } = new UnityEvent();
    /// <summary>
    /// ゲームの終了条件を満たしたときに実行される
    /// </summary>
    public UnityEvent OnGameClear { get; } = new UnityEvent();
    /// <summary>
    /// クリア演出が終わったり、ゲームを中断したときなど、
    /// ゲームを終了したときに実行される
    /// </summary>
    public UnityEvent OnGameQuit { get; } = new UnityEvent();
    [Header("Debug")]
    //現在接続されているConnecter
    [SerializeField, ReadOnly] int currentCount = 0;
    //成功判定になるためのConnecterのカウント
    [SerializeField, ReadOnly] int sumCount = 0;
    public Timer gameTimer { get; private set; }

    public enum State
    {
        notStarted,//まだ初期化されてない
      // starting,
        running,//実行中
        pause,//ポーズ中
       cleared,//クリア条件達成
       //quitting,
    }

    [SerializeField, ReadOnly] State m_state=State.notStarted;
    private void Awake()
    {
        gameTimer = GetComponent<Timer>();
    }
    private void Start()
    {
        foreach(var i in targetConnecter)
        {
            if (i == null) continue;
            i.OnConnectEnter.AddListener((receiver)=> {
                currentCount++;
                if (currentCount >= sumCount)
                {
                    OnGameClear.Invoke();
                }
            });
            i.OnConnectExit.AddListener((receiver) =>
            {
                currentCount--;
                if (currentCount < 0) currentCount = 0;
            });
            sumCount++;
        }
        OnGameStart.Invoke();
        m_state = State.running;
        gameTimer.StartTimer(0,100);
    }

    public void AddCircuitToGame(CircuitSO data)
    {
        Instantiate(data.prefab).GetComponent<CircuitScript>().SetData(data);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_state == State.running)
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
    /// ゲームを完了させる
    /// </summary>
    public void EndGame()
    {
        Quit();
    }
    /// <summary>
    /// ポーズし、ゲームを中断する
    /// </summary>
    public void Pause()
    {
        if (m_state == State.pause) return;
        m_state = State.pause;
        OnGamePause.Invoke();
    }
    /// <summary>
    /// ポーズを解除しゲームを再開させる
    /// </summary>
    public void Resume()
    {
        if (m_state == State.running) return;
        m_state = State.running;
        OnGameResume.Invoke();
    }
    /// <summary>
    /// ミニゲームを終了させ、マップに戻る
    /// </summary>
    public void Quit()
    {
        OnGameQuit.Invoke();
    }
    /// <summary>
    /// WallWatcherを終了し、デスクトップ画面に戻る
    /// </summary>
    public void BackToDesktop()
    {

    }

}
