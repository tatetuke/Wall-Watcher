using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class MiniGamePauseUI : MonoBehaviour
{
    [SerializeField] Image pauseBackImage;
    Animator animator;

    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
    }
    IGameManager gameManager;
    T FindObjectOfInterface<T>() where T : class
    {
        foreach (var i in FindObjectsOfType<Component>())
        {
            var component = i as T;
            if (component != null)
            {
                return component;
            }
        }
        return null;
    }

    private void Start()
    {
        gameManager = FindObjectOfInterface<IGameManager>();
        gameObject.SetActive(false);//初期状態は隠す
        gameManager.OnPause().AddListener(()=> {
            gameObject.SetActive(true);
            animator.Play("CircuitPauseUIFadeIn");
        });
        gameManager.OnResume().AddListener(() =>
        {
            animator.Play("CircuitPauseUIFadeOut");
            gameObject.SetActive(false);
        });
    }

    /// <summary>
    /// 再開ボタンが押されたときの処理
    /// </summary>
    public void Resume()
    {
        gameManager.Resume();
    }
    /// <summary>
    /// ミニゲームを終了するボタンが押されたときの処理
    /// </summary>
    public void Quit()
    {
       // gameManager.();
    }
    /// <summary>
    /// デスクトップに戻るボタンが押されたときの処理
    /// </summary>
    public void BackToDesktop()
    {
      //  gameManager.EndGame();
    }
}
