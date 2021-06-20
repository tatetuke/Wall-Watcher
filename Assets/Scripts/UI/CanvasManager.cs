using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;


public class CanvasManager : MonoBehaviour
{
    [Tooltip("ポーズしたときにいつも表示されるview（ポーズしてないときは表示されない）")]
    [SerializeField] UIView allwaysShowView;
    [SerializeField] string firstViewName;
    [Header("Debug")]
    [SerializeField, ReadOnly] List<UIView> m_views = new List<UIView>();
    Stack<string> m_viewHistory = new Stack<string>();
    public UnityEvent OnCloseCanvas { get; } = new UnityEvent();

    private void Awake()
    {
        m_views.AddRange(GetComponentsInChildren<UIView>(true));
        foreach (var i in m_views)
        {
            if (i == null||i.backButton==null) continue;
            i.backButton.onClick.AddListener(Back);
        }
    }

    private void Start()
    {
        if (allwaysShowView != null)
            allwaysShowView.gameObject.SetActive(false);
        foreach (var i in m_views)
            i.gameObject.SetActive(false);
    }

    UIView GetView(string viewName)
    {
        foreach (var i in m_views)
            if (i.name == viewName)
                return i;
        return null;
    }
    public void SwitchView(string viewName)
    {
        Debug.Log($"switch to '{viewName}'");
        if (m_viewHistory.Count == 0)//初めてviewを起動したとき
        {
            var obj = GetView(viewName);
            if (obj == null) return;
            ShowView(obj);
            m_viewHistory.Push(viewName);
            return;
        }
        string beforeActive = m_viewHistory.Peek();
        if (viewName == beforeActive) return;
        var view = GetView(viewName);
        if (view == null)
        {
            Debug.LogWarning($"View '{viewName}' is not found");
            return;
        }
        ShowView(view);
        HideView(GetView(beforeActive));
        m_viewHistory.Push(viewName);
    }
    void Back()
    {
        Debug.Log("back");
        //もしviewを移動させてない状態でbackを押した場合、viewを非表示にし終了
        if (m_viewHistory.Count == 1)
        {
            Kyoichi.GameManager.Instance.PauseEnd();
            return;
        }
        string lastActive = m_viewHistory.Peek();
        HideView(GetView(lastActive));
        m_viewHistory.Pop();
        string nextActive = m_viewHistory.Peek();
        ShowView(GetView(nextActive));
    }
    void ShowView(UIView view)
    {
        view.gameObject.SetActive(true);
        view.OnViewShow.Invoke();
    }
    void HideView(UIView view)
    {
        view.gameObject.SetActive(false);
        view.OnViewHide.Invoke();
    }
}
