using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// はい、いいえのポップアップを制御するクラス
/// </summary>
public class YesNoPupUpUI : MonoBehaviour
{
    [Header("Yes Button")]
    [SerializeField]Button yesButton;
    [SerializeField]TextMeshProUGUI yesText;
    [Header("No Button")]
    [SerializeField] Button noButton;
    [SerializeField]TextMeshProUGUI noText;
    [Header("Content")]
    [SerializeField] TextMeshProUGUI text;//本文

    public void SetText(string text_)
    {
        text.text = text_;
    }
    public void SetYes(string text,UnityAction onClick)
    {
        yesText.text = text;
        yesButton.onClick.AddListener(onClick);
    }
    public void SetNo(string text, UnityAction onClick)
    {
        noText.text = text;
        noButton.onClick.AddListener(onClick);
    }

}
