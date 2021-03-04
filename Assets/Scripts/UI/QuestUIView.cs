﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestUIView : UIView
{
    [SerializeField] Transform questListContainer;
    [SerializeField] GameObject questPrefab;
    [SerializeField] Transform descriptionContainer;
    [SerializeField] GameObject descriptionPrefab;
    [SerializeField] Button descriptionCloseButton;

    public void OnQuestClicked()
    {
        Instantiate(descriptionPrefab, descriptionContainer);
    }
    private void Start()
    {
        descriptionCloseButton.onClick.AddListener(()=> { 
        
        });


    }

}