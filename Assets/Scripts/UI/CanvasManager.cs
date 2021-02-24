using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
public class CanvasManager : MonoBehaviour
{
    [SerializeField,ReadOnly]
    List<UIView> views = new List<UIView>();
    [SerializeField] string firstViewName;
    private void Awake()
    {
        views.AddRange(GetComponentsInChildren<UIView>(true));
        foreach (var i in views) i.backButton.onClick.AddListener(Back);
    }

    Stack<string> viewHistory = new Stack<string>();

    private void Start()
    {
        foreach (var i in views)
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
    }
    public UIView GetView(string viewName)
    {
        foreach (var i in views)
            if (i.name == viewName)
                return i;
        return null;
    }
    public void SwitchView(string viewName)
    {
        Debug.Log($"switch to '{viewName}'");
        if (viewHistory.Count == 0)
        {
            var obj = GetView(viewName);
            if (obj == null) return;
            obj.gameObject.SetActive(true);
            obj.OnViewShow.Invoke();
            viewHistory.Push(viewName);
            return;
        }
        bool findsView = false;
        string beforeActive = viewHistory.Peek();
        if (viewName == beforeActive) return;
        foreach(var i in views)
        {
            i.gameObject.SetActive(false);
            if (i.name == viewName)
            {
                i.gameObject.SetActive(true);
                i.OnViewShow.Invoke();
                viewHistory.Push(viewName);
                findsView = true;
            }else if(i.name== beforeActive)
            {
                i.OnViewHide.Invoke();
            }
        }
        if (!findsView)
        {
            Debug.LogWarning($"View '{viewName}' is not found");
        }
    }
    public void Back()
    {
        Debug.Log("back");
        string beforeActive = viewHistory.Peek();
        viewHistory.Pop();
        if (viewHistory.Count == 1)
        {
            OnPushEndButton();
            foreach (var i in views)
            {
                if (i.name == beforeActive)
                {
                i.OnViewHide.Invoke();
                }
            }
            return;
        }
        foreach (var i in views)
        {
            i.gameObject.SetActive(false);
            if (i.name == viewHistory.Peek())
            {
                i.gameObject.SetActive(true);
                i.OnViewShow.Invoke();
            }
            else if (i.name == beforeActive)
            {
                i.OnViewHide.Invoke();
            }
        }

    }
    /// <summary>
    /// ゲームを終了するボタンが押されたとき
    /// </summary>
    public void OnPushEndButton()
    {
        foreach (var i in views)
        {
            i.gameObject.SetActive(false);
        }
    }

}
