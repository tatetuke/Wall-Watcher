using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public Selectable firstSelectUI;
    public Button backButton;
    public UnityEvent OnViewShow = new UnityEvent();
    public UnityEvent OnViewHide = new UnityEvent();
    private void Awake()
    {
        OnViewShow.AddListener(() => { firstSelectUI?.Select(); });
    }
}
