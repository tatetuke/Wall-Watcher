using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 回路の修理ゲームのUIを管理
/// ゲームの進行度自体とは別のクラスで。
/// </summary>
[RequireComponent(typeof(Animator))]
public class CircuitGameUIManager : MonoBehaviour
{
    [SerializeField] Image dragAndDrop;
    [SerializeField] Transform itemContainer;
    [SerializeField] GameObject itemPrefab;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        var gameManager = FindObjectOfType<CircuitGameManager>();
        gameManager.OnGameStart.AddListener(()=> {
            animator.Play("CircuitUIFadeIn");
        });
        gameManager.OnGameClear.AddListener(() =>
        {
            animator.Play("CircuitUIFadeOut");
        });
    }

}
