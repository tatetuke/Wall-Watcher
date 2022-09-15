using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Threading.Tasks;
using System;

[SerializeField]
public class PauseUIEvent:UnityEvent<string>{}

[RequireComponent(typeof(Animator))]
public class PauseUIMenu : UIView
{

    public PauseUIEvent OnChangeView = new PauseUIEvent();

    [SerializeField]
    Animator[] childAnimations;
    Animator animator;
    [SerializeField]
    Button quitButton;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }


    private void Start()
    {
        OnViewShow.AddListener(() =>
        {
            animator.SetTrigger("FadeIn");
            foreach (var anim in childAnimations)
            {
                anim.SetTrigger("FadeIn");
            }
        });
        OnViewHideStart.AddListener(() =>
        {
            animator.SetTrigger("FadeOut");
            foreach (var anim in childAnimations)
            {
                anim.SetTrigger("FadeOut");
            }
        });


        quitButton.onClick.AddListener(() =>
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
        });

        
    }

    public void ChangeView(string viewName)
    {
        OnChangeView.Invoke(viewName);
    }

}
