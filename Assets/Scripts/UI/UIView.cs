using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public Selectable firstSelectUI;
    public Button backButton;
    public UnityEvent OnViewShow { get; } = new UnityEvent();
    public UnityEvent OnViewHide { get; } = new UnityEvent();
    private void Awake()
    {
        OnViewShow.AddListener(() => { firstSelectUI?.Select(); });
    }
}
