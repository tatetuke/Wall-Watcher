using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIView : UIView
{
    [SerializeField] Transform questListContainer;
    [SerializeField] GameObject questPrefab;
    [SerializeField] Transform descriptionWindow;
    [SerializeField] Transform descriptionContainer;
    [SerializeField] GameObject descriptionPrefab;
    [SerializeField] Button descriptionCloseButton;
    [SerializeField] QuestHolder target;

    public void OnQuestClicked()
    {
        Instantiate(descriptionPrefab, descriptionContainer);
    }
    private void Start()
    {
        descriptionCloseButton.onClick.AddListener(() =>
        {
            descriptionWindow.gameObject.SetActive(false);
        });

        OnViewShow.AddListener(Initialize);
        OnViewHide.AddListener(() =>
        {
            foreach (Transform child in questListContainer)
            {
                Destroy(child.gameObject);
            }
        });
        descriptionWindow.gameObject.SetActive(false);
    }

    public void Initialize()
    {
        foreach(var i in target.Data)
        {
            Instantiate(questPrefab, questListContainer);
        }
    }
}
