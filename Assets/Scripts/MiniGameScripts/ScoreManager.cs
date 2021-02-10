using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// スコアを管理する
/// </summary>
public class ScoreManager
{
    // TODO : 他の要素(材料混成度など), 計算方法

    // 満足度
    int Satisfaction;
    // 混成度
    int Mix;

    // コンストラクタで初期化
    public ScoreManager()
    {
        Satisfaction = 0;
        Mix = 0;
    }

    public void UpdateScore()
    {
        if (true) Satisfaction++;

        if (true) Mix++;
    }

    public char GetSatisfaction()
    {
        char res;
        if (Satisfaction >= 1000) res = 'A';
        else if (Satisfaction >= 500) res = 'B';
        else res = 'C';
        return res;
    }

    public char GetMix()
    {
        char res;
        if (Mix >= 2000) res = 'A';
        else if (Satisfaction >= 700) res = 'B';
        else res = 'C';
        return res;
    }

    public double GetEarnReward()
    {
        double res = (double)Satisfaction * Mix / 31415.92;
        return res;
    }
}
