using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class UIView : MonoBehaviour
{
    public Selectable firstSelectUI;
    public Button backButton;
    /// <summary>
    /// Viewの表示が始まったとき実行される
    /// </summary>
    public UnityEvent OnViewShow { get; } = new UnityEvent();
    /// <summary>
    /// Viewのフェードアウトが始まるとき実行される
    /// </summary>
    public UnityEvent OnViewHideStart { get; } = new UnityEvent();
    /// <summary>
    /// Viewのフェードアウトが終わったとき実行される
    /// </summary>
    public UnityEvent OnViewHided { get; } = new UnityEvent();
    private void Awake()
    {
        //viewが起動したときに最初に選択されるボタンを選択
        OnViewShow.AddListener(() => { firstSelectUI?.Select(); });
    }

    /// <summary>
    /// Viewのフェードアウトのアニメーションが終わったとき，AnimationClipから実行する
    /// </summary>
    public void ViewHideFinished()
    {
        Debug.Log("UIView hide finished");
        OnViewHided.Invoke();
    }
}
