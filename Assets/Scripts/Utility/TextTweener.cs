using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextTweener : MonoBehaviour
{
    /// <summary>
    /// テキストがすべて表示し終わったら実行される
    /// </summary>
    public UnityEvent OnTextFinished { get; } = new UnityEvent();
    public float speed = 0f;//新しく1文字表示されるまでの速度
    float m_time = 0f;
    int wordCount = 0;//文字数
    int currentIndex = 0;
    string m_text;
    public bool isFinished { get; private set; } = false;

    public string Text
    {
        get
        {
            return m_text;
        }
        set
        {
            m_text = value;
            m_time = 0f;
            wordCount = m_text.Length;
            currentIndex = 0;
            isFinished = false;
        }
    }

    TextMeshProUGUI target;
    private void Awake()
    {
        target = GetComponent<TextMeshProUGUI>();
        target.text = "";
    }
    // Update is called once per frame
    void Update()
    {
        if (isFinished) return;
        if (m_time >= speed)
        {
            if (currentIndex == wordCount)
            {
                isFinished = true;
                return;
            }
            m_time = 0f;
            target.text = Text.Substring(0, currentIndex + 1);//1文字目からcurrentIndexまでの文字を抽出
            currentIndex++;
        }
        m_time += Time.deltaTime;
    }

    public void ShowFullText()
    {
        target.text = Text;
        isFinished = true;
    }
}