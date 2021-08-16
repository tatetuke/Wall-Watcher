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

    private void Start()
    {
        gameObject.SetActive(false);//初期状態は隠す
        PauseManager.Instance.OnPauseEnter.AddListener(()=> {
            gameObject.SetActive(true);
            animator.Play("CircuitPauseUIFadeIn");
        });
        PauseManager.Instance.OnPauseExit.AddListener(() =>
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
        PauseManager.Instance.Resume();
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
