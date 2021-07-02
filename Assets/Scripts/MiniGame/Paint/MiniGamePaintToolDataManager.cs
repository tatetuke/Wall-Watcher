﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MiniGamePaintToolDataManager : MonoBehaviour
{
    [SerializeField] private MiniGamePaintToolData toolDataScript;
    [SerializeField] private Text level;
    [SerializeField] private Text toolName;
    [SerializeField] private Text descripton;
    [SerializeField] private Text damage;
    [SerializeField] private Image hanniImage;
    private MiniGamePaintToolStatus[] Tools;
    public int SelectToolNum;
    private void Start()
    {
        SelectToolNum = 0;
        Tools = toolDataScript.Tools;

    }
    //それぞれのボタンを押した際に呼ばれる関数
    public void PushTool1()
    {
        MiniGamePaintManager.Instance.ChangeRange(MiniGamePaintManager.Range.Square);
        ChangeUI();
    }
    public void PushTool2()
    {
        Debug.Log("2が押された");
        MiniGamePaintManager.Instance.ChangeRange(MiniGamePaintManager.Range.Up);
        ChangeUI();
        Debug.Log(MiniGamePaintManager.Instance.m_Range);
    }
    public void PushTool3()
    {
        MiniGamePaintManager.Instance.ChangeRange(MiniGamePaintManager.Range.Up);
        ChangeUI();
    }


    /// <summary>
    /// UIの中のテキストや画像を変更する関数
    /// </summary>
    public void ChangeUI()
    {
        //レベルが0(アイテム取得状態でない)ならば終了
        if (Tools[SelectToolNum].level == 0) return;
        //表示するレベルを変更
        level.text = "Level " + Tools[SelectToolNum].level;
        //道具の名前を変更
        toolName.text = Tools[SelectToolNum].toolName;
        //道具の説明を変更
        descripton.text = Tools[SelectToolNum].descripton;
        //道具が与えるダメージの表記を変更
        damage.text = "消費体力：" + GetDamageLevel(Tools[SelectToolNum].damage[Tools[SelectToolNum].level - 1]);
        //道具が削る範囲の説明用画像を変更
        hanniImage.sprite = Tools[SelectToolNum].HanniImage;
    }
    /// <summary>
    /// ダメージに応じた体力の減り具合を文字に変換する関数
    /// </summary>
    /// <param name="damage"></param>
    /// <returns></returns>
    private string GetDamageLevel(int damage)
    {
        if (damage >= 50)
        {
            return "極大";

        }
        else if (damage >= 30)
        {
            return "大";
        }
        else if (damage >= 20)
        {

            return "中";
        }
        else if (damage >= 10)
        {
            return "小";
        }
        else
        {
            return "極小";
        }
    }
}
