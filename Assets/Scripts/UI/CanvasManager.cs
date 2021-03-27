using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public interface IUIManager
{
    Player GetTarget();
}

public class CanvasManager : MonoBehaviour, IUIManager
{
    enum State
    {
        idle,
        fadeIn,
        active,
        fadeOut
    }
    [Tooltip("ポーズしたときにいつも表示されるview（ポーズしてないときは表示されない）")]
    [SerializeField] UIView allwaysShowView;
    [SerializeField] Animator animator;
    [SerializeField] string fadeInTrriger="FadeIn";
    [SerializeField] string fadeOutTrriger="FadeOut";
    [SerializeField] Button saveButton;
    [ReadOnly]
    [SerializeField]List<UIView> m_views = new List<UIView>();
    [SerializeField] string firstViewName;
    [Header("Debug")]
    [SerializeField,ReadOnly] State m_state;
    Stack<string> m_viewHistory = new Stack<string>();
    public UnityEvent OnCloseCanvas { get; } = new UnityEvent();

    [SerializeField,ReadOnly] Player m_targetPlaeyer;
    public Player GetTarget() => m_targetPlaeyer; 

    private void Awake()
    {
        m_views.AddRange(GetComponentsInChildren<UIView>(true));
        foreach (var i in m_views)
        {
            if (i == null||i.backButton==null) continue;
            i.backButton.onClick.AddListener(Back);
            i.SetManager(this);
        }
    }

    private void Start()
    {
        m_state = State.idle;
        allwaysShowView.gameObject.SetActive(false);
        foreach (var i in m_views)
            i.gameObject.SetActive(false);
       
        Kyoichi.GameManager.Instance.OnPauseStart.AddListener(() =>
        {
            m_state = State.fadeIn;
            animator.SetTrigger(fadeInTrriger);
            SwitchView(firstViewName);
            allwaysShowView.gameObject.SetActive(true);
        });
        Kyoichi.GameManager.Instance.OnPauseEnd.AddListener(() =>
        {
            m_state = State.fadeOut;
            animator.SetTrigger(fadeOutTrriger);
        });
        //会話中はセーブできないようにする
        ConversationDataManager.Instance.OnTalkAccepted.AddListener(() =>
        {
            saveButton.interactable = false;
        });
        //会話が終わったら解除
        ConversationDataManager.Instance.OnTalkEnd.AddListener(() =>
        {
            saveButton.interactable = true;
        });
        //シーン内のプレイヤーを取得
        m_targetPlaeyer = FindObjectOfType<Player>();
    }

    /// <summary>
    /// Animationのトリガー用
    /// ポーズのフェードインが開始したとき
    /// </summary>
   public void OnFadeInFinished()
    {
        m_state = State.active;
    }
    /// <summary>
    /// Animationのトリガー用
    /// ポーズのフェードアウトが終了したときに呼び出される
    /// </summary>
    public void OnFadeOutFinished()
    {
        //フェードアウトのアニメーションをしている間にもう一度ポーズ画面を開いたとき
        if (m_state == State.fadeIn) return;
        m_state = State.idle;
        foreach (var i in m_views)
        {
            i.gameObject.SetActive(false);
        }
        allwaysShowView.gameObject.SetActive(false);
            m_viewHistory.Clear();
        OnCloseCanvas.Invoke();
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
