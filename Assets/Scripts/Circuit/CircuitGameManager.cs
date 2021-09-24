using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
    /// ゲームを開始したときに実行される
    /// </summary>
    public UnityEvent OnGameStart { get; } = new UnityEvent();
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
        cleared,//クリア条件達成
                //quitting,
    }

    [SerializeField, ReadOnly] State m_state = State.notStarted;
    private void Awake()
    {
        gameTimer = GetComponent<Timer>();
    }
    private void Start()
    {
        foreach (var i in targetConnecter)
        {
            if (i == null) continue;
            i.OnConnectEnter.AddListener((receiver) =>
            {
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
        StartGame();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            Kyoichi.SceneChangeWrapper.Instance.SceneChange(
                "CIrcuitGame_template"
                );
        }
    }

    public void AddCircuitToGame(CircuitSO data)
    {
        Instantiate(data.prefab).GetComponent<CircuitScript>().SetData(data);
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

    public void StartGame()
    {
        OnGameStart.Invoke();
        m_state = State.running;
        gameTimer.StartTimer(0, 100);
    }

    /// <summary>
    /// ゲームを完了させる
    /// </summary>
    public void ClearGame()
    {
        Quit();
    }
    public void EndGame()
    {
        OnEndGame().Invoke();
    }

    public UnityEvent OnStartGame() => OnGameStart;
    public UnityEvent OnClearGame() => OnGameClear;
    public UnityEvent OnEndGame() => OnGameQuit;

    public void EndProgram()
    {
    }

    public void Back()
    {
    }
}