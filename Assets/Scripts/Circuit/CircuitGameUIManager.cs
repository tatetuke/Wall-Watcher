using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
/// <summary>
/// 回路の修理ゲームのUIを管理
/// ゲームの進行度自体とは別のクラスで。
/// </summary>
[DisallowMultipleComponent]
public class CircuitGameUIManager : MonoBehaviour
{
    [SerializeField] Image dragAndDrop;
    [SerializeField] DropZoneScript dropZone;
    [SerializeField] DropZoneScript itemView;
    [SerializeField] Transform itemContainer;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] Button exitButton;
    //現在ドラッグしてるアイテム
    CircuitSO draggingItem;
    [SerializeField] Animator animator;
    [SerializeField] TMPro.TextMeshProUGUI timer_text;
    private void Awake()
    {
        var gameManager = FindObjectOfType<CircuitGameManager>();
        //OnGameStartはStartで呼ばれるので、Awakeで登録しないといけない
        gameManager.OnGameStart.AddListener(() =>
        {
            Debug.Log("game start");
            animator.gameObject.SetActive(true);
            animator.Play("CircuitUIFadeIn");
        });
        gameManager.OnGameClear.AddListener(() =>
        {
            Debug.Log("game clear");
            exitButton.gameObject.SetActive(true);
        });
    }
    // Start is called before the first frame update
    void Start()
    {
        var gameManager = FindObjectOfType<CircuitGameManager>();
        gameManager.gameTimer.OnTimerUpdated.AddListener((time) => {
            timer_text.text = String.Format("{0:0.00}", time);
        });
        foreach (var i in gameManager.RequiredItems)
        {
            if (i == null) continue;
            var scr = Instantiate(itemPrefab, itemContainer).GetComponent<CircuitItemUI>();
            scr.SetImage(i.icon);
            scr.OnPointerDown.AddListener(() =>
            {
                Debug.Log("Pointer down");
                dragAndDrop.gameObject.SetActive(true);
                itemView.gameObject.SetActive(true);
                dragAndDrop.sprite = i.icon;
                dragAndDrop.preserveAspect = true;
                dropZone.gameObject.SetActive(true);
                draggingItem = i;
            });
            scr.OnPointerUp.AddListener(() =>
            {
                dragAndDrop.gameObject.SetActive(false);
            });
        }
        dropZone.OnDropObj.AddListener((obj) =>
        {
            gameManager.AddCircuitToGame(draggingItem);
            dropZone.gameObject.SetActive(false);
            itemView.gameObject.SetActive(false);
            draggingItem = null;
        });
        itemView.OnDropObj.AddListener((obj) =>
        {
            Debug.Log("drop to itemview");
            dropZone.gameObject.SetActive(false);
            itemView.gameObject.SetActive(false);
        });
        dropZone.gameObject.SetActive(false);
        itemView.gameObject.SetActive(false);
        exitButton.gameObject.SetActive(false);
        //ゲームを終了する処理を登録
        exitButton.onClick.AddListener(() =>
        {
            animator.gameObject.SetActive(true);
            animator.Play("CircuitUIFadeOut");
            gameManager.EndGame();
        });
    }

}
