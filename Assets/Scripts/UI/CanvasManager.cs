using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{
    [ReadOnly]
    [SerializeField]List<UIView> m_views = new List<UIView>();
    [SerializeField] string firstViewName;
    Stack<string> m_viewHistory = new Stack<string>();
    public UnityEvent OnCloseCanvas { get; } = new UnityEvent();

    [SerializeField,ReadOnly] Player m_targetPlaeyer;
    public Player GetTarget() => m_targetPlaeyer; 

    private void Awake()
    {
        m_views.AddRange(GetComponentsInChildren<UIView>(true));
        foreach (var i in m_views)
        {
            i.backButton.onClick.AddListener(Back);
            i.SetManager(this);
        }
    }

    private void Start()
    {
        foreach (var i in m_views)
        {
            i.gameObject.SetActive(false);
        }

        Kyoichi.GameManager.Instance.OnPauseStart.AddListener(() =>
        {
            SwitchView(firstViewName);
        });
        Kyoichi.GameManager.Instance.OnPauseEnd.AddListener(() =>
        {
            OnPushEndButton();
        });
        m_targetPlaeyer = FindObjectOfType<Player>();
    }
    public UIView GetView(string viewName)
    {
        foreach (var i in m_views)
            if (i.name == viewName)
                return i;
        return null;
    }
    public void SwitchView(string viewName)
    {
        Debug.Log($"switch to '{viewName}'");
        if (m_viewHistory.Count == 0)
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
    public void Back()
    {
        Debug.Log("back");
        //もしviewを移動させてない状態でbackを押した場合、viewを非表示にし終了
        if (m_viewHistory.Count == 1)
        {
            OnPushEndButton();
            string view = m_viewHistory.Peek();
            m_viewHistory.Pop();
            HideView(GetView(view));
            OnCloseCanvas.Invoke();//キャンバスを閉じる
            return;
        }
        string lastActive = m_viewHistory.Peek();
        HideView(GetView(lastActive));
        m_viewHistory.Pop();
        string nextActive = m_viewHistory.Peek();
        ShowView(GetView(nextActive));
    }
    /// <summary>
    /// ゲームを終了するボタンが押されたとき
    /// </summary>
    public void OnPushEndButton()
    {
        foreach (var i in m_views)
        {
            i.gameObject.SetActive(false);
        }
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
