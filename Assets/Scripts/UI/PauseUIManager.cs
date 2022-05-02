using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
[DisallowMultipleComponent]
public class PauseUIManager : CanvasManager
{
    enum State
    {
        idle,
        fadeIn,
        active,
        fadeOut
    }
     Animator animator;
    [SerializeField] string fadeInTrriger = "FadeIn";
    [SerializeField] string fadeOutTrriger = "FadeOut";
    [SerializeField] Button quitButton;
    [Header("Debug")]
    [SerializeField, ReadOnly] State m_state;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        m_views.AddRange(GetComponentsInChildren<UIView>(true));
        foreach (var i in m_views)
        {
            if (i?.backButton == null) continue;
            i.backButton.onClick.AddListener(Back);
        }
    }

    private void Start()
    {
        m_state = State.idle;
        allwaysShowView.gameObject.SetActive(false);
        foreach (var i in m_views)
            i.gameObject.SetActive(false);

        PauseManager.Instance.OnPauseEnter.AddListener(() =>
        {
            m_state = State.fadeIn;
            animator.SetTrigger(fadeInTrriger);
            SwitchView(firstViewName);
            allwaysShowView.gameObject.SetActive(true);
        });
        PauseManager.Instance.OnPauseExit.AddListener(() =>
        {
            m_state = State.fadeOut;
            animator.SetTrigger(fadeOutTrriger);
        });
        //会話中はセーブできないようにする
        /*ConversationDataManager.Instance.OnTalkAccepted.AddListener(() =>
        {
            saveButton.interactable = false;
        });
        //会話が終わったら解除
        ConversationDataManager.Instance.OnTalkEnd.AddListener(() =>
        {
            saveButton.interactable = true;
        });*/
        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
        });
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

}
