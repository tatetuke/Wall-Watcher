using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// セレクト画面におけるセレクトしている番号や
/// 今選んでいる番号を選ばれていない番号を色でわかりやすくする
/// </summary>
public class SelectManager
{
    Color SelectedColor;     // 選択されているときの色
    Color NotSelectedColor;  // 選択されていないときの色
    Text[] Texts;
    int Size;

    // コンストラクタでテキストの配列と使う色を取得
    public SelectManager(/*選択肢のテキストの配列*/   Text[] texts,
                         /*選択されているときの色*/   Color selected_color,
                         /*選択されていないときの色*/ Color not_selected_color)
    {
        Texts = texts;
        Size = texts.Length;
        SelectedColor = selected_color;
        NotSelectedColor = not_selected_color;
    }


    /// <summary>
    /// 選択番号およびテキストカラーの更新(上下左右の4パターン)
    /// 
    /// 元々選ばれており濃くなっていた番号を薄くする
    /// ->選択番号を更新
    /// ->その選択番号を濃くする
    /// </summary>
    public void UpdateLeft(ref int selectnum)
    {
        ChangeColorDown(selectnum);
        selectnum += Size;
        selectnum--;
        selectnum %= Size;
        ChangeColorUp(selectnum);
    }

    public void UpdateRight(ref int selectnum)
    {
        ChangeColorDown(selectnum);
        selectnum++;
        selectnum %= Size;
        ChangeColorUp(selectnum);
    }

    public void UpdateUp(ref int selectnum)
    {
        ChangeColorDown(selectnum);
        selectnum += Size;
        selectnum--;
        selectnum %= Size;
        ChangeColorUp(selectnum);
    }

    public void UpdateDown(ref int selectnum)
    {
        ChangeColorDown(selectnum);
        selectnum++;
        selectnum %= Size;
        ChangeColorUp(selectnum);
    }


    /// 選択されている番号は濃くする
    public void ChangeColorUp(int selectnum)
    {
        Texts[selectnum].color = SelectedColor;
    }

    /// 選択されていない番号は薄くする
    public void ChangeColorDown(int selectnum)
    {
        Texts[selectnum].color = NotSelectedColor;
    }
}
