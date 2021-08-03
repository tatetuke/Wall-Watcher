using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent OnGameStart { get; } = new UnityEvent();
    public UnityEvent OnGameClear { get; } = new UnityEvent();

    //現在接続されているConnecter
   [SerializeField,ReadOnly] int currentCount = 0;
    //成功判定になるためのConnecterのカウント
    [SerializeField, ReadOnly] int sumCount = 0;

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
    }

}
