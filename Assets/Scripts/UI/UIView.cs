using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIView : MonoBehaviour
{
    public Button backButton;
    protected CanvasManager m_parentManager;
    public void SetManager(CanvasManager manager) => m_parentManager = manager;
    public UnityEvent OnViewShow = new UnityEvent();
    public UnityEvent OnViewHide = new UnityEvent();
}
