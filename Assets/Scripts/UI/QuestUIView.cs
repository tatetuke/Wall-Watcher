using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIView : UIView
{
    [SerializeField] Transform questListContainer;
    [SerializeField] GameObject questPrefab;
    [Header("クエストの詳細表示")]
    [SerializeField] Transform descriptionWindow;
    [SerializeField] Transform descriptionContainer;
    [SerializeField] GameObject descriptionPrefab;
    [SerializeField] Button descriptionCloseButton;
    [Header("debug")]
    [SerializeField,ReadOnly] QuestHolder target;

    public void OnQuestClicked()
    {
        Instantiate(descriptionPrefab, descriptionContainer);
    }
    private void Awake()
    {
        OnViewShow.AddListener(Initialize);
        OnViewHide.AddListener(() =>
        {
            foreach (Transform child in questListContainer)
            {
                Destroy(child.gameObject);
            }
        });
    }
    private void Start()
    {
        descriptionCloseButton.onClick.AddListener(() =>
        {
            descriptionWindow.gameObject.SetActive(false);
        });

        descriptionWindow.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        target = FindObjectOfType<QuestHolder>();
        foreach (var i in target.Data)
        {
            var scr=Instantiate(questPrefab, questListContainer).GetComponent<QuestContainerUI>();
            scr.SetOnClicked(()=> { 
            
            });
        }
    }
}
