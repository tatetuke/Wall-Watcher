using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogController
{
    // ボタンのテキストを選択肢のテキストに書き換える
    public void SetText(Text option, string newtext)
    {
        option.text = newtext;
    }

    public void Display(GameObject option)
    {
        option.SetActive(true);
    }

    public void Hide(GameObject option)
    {
        option.SetActive(false);
    }
}
