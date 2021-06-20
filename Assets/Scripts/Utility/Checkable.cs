using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Checkerで確認することができるオブジェクト
/// 話しかけられるNPCや確認できる機械につける
/// </summary>
public class Checkable : MonoBehaviour
{

    [System.Serializable] public class CheckableEvent : UnityEvent<Checker> { }

    public bool m_DestroyAfterChecked = false;
    /// <summary>
    /// このオブジェクトをチェックしたときに移動できるか
    /// </summary>
    public bool m_MoveAfterChecked = false;

    /// <summary>
    /// チェックされたときに実行されるコールバック
    /// </summary>
    public CheckableEvent OnChecked = new CheckableEvent();

    /// <summary>
    /// チェックされる範囲に入ったら実行されるコールバック
    /// </summary>
    public CheckableEvent OnEnter = new CheckableEvent();

    /// <summary>
    /// チェックされる範囲から出たら実行されるコールバック
    /// </summary>
    public CheckableEvent OnExit = new CheckableEvent();

    public void TakeCheck(Checker checker)
    {
        OnChecked.Invoke(checker);
        if (m_DestroyAfterChecked)
            Destroy(gameObject);
    }

}
