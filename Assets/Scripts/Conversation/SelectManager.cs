using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
/// <summary>
/// セレクト画面におけるセレクトしている番号や
/// 今選んでいる番号を選ばれていない番号を色でわかりやすくする
/// </summary>
public class SelectManager
{
    private Color SelectedColor;     // 選択されているときの色
    private Color NotSelectedColor;  // 選択されていないときの色
    private TextMeshProUGUI[] Texts;
    private int Size;
    private int SelectNum;

    // コンストラクタでテキストの配列と使う色を取得
    public SelectManager(/*選択肢のテキストの配列*/   TextMeshProUGUI[] texts,
                         /*選択されているときの色*/   Color selected_color,
                         /*選択されていないときの色*/ Color not_selected_color)
    {
        Texts = texts;
        Size = texts.Length;
        SelectedColor = selected_color;
        NotSelectedColor = not_selected_color;
        SelectNum = 0;
    }


    /// <summary>
    /// 選択番号およびテキストカラーの更新(上下左右の4パターン)
    /// 
    /// 元々選ばれており濃くなっていた番号を薄くする
    /// ->選択番号を更新
    /// ->その選択番号を濃くする
    /// </summary>
    public void UpdateLeft()
    {
        int preSelectNum = SelectNum;
        int nextSelectNum = SelectNum = (SelectNum + Size - 1) % Size;
        ChangeColorDown(preSelectNum);
        ChangeColorUp(nextSelectNum);
    }

    public void UpdateRight()
    {
        int preSelectNum = SelectNum;
        int nextSelectNum = SelectNum = (SelectNum + 1) % Size;
        ChangeColorDown(preSelectNum);
        ChangeColorUp(nextSelectNum);
    }

    public void UpdateUp()
    {
        int preSelectNum = SelectNum;
        int nextSelectNum = SelectNum = (SelectNum + Size - 1) % Size;
        ChangeColorDown(preSelectNum);
        ChangeColorUp(nextSelectNum);
    }

    public void UpdateDown()
    {
        int preSelectNum = SelectNum;
        int nextSelectNum = SelectNum = (SelectNum + 1) % Size;
        ChangeColorDown(preSelectNum);
        ChangeColorUp(nextSelectNum);
    }


    /// 選択されている番号は濃くする
    public void ChangeColorUp(int num)
    {
        Texts[num].color = SelectedColor;
    }

    /// 選択されていない番号は薄くする
    public void ChangeColorDown(int num)
    {
        Texts[num].color = NotSelectedColor;
    }


    public void ChangeSelectNum(int num)
    {
        SelectNum = num;
    }

    public int GetSelectNum()
    {
        return SelectNum;
    }
}
