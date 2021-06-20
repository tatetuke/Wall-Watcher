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
    char Satisfaction;
    // 混成度
    char Mix;

    private int MaxRGB = 78;


    // コンストラクタで初期化
    public ScoreManager()
    {
    }

    public int GetMaxRGB()
    {
        return MaxRGB;
    }

    public void UpdateMix()
    {
        int mixDiff = 0;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var gameObject in gameObjects)
        {
            Mix_Wall mix_Wall = gameObject.GetComponent<Mix_Wall>();
            mixDiff += Mathf.Abs(mix_Wall.ColorNum - 120);
        }
        if (mixDiff <= 500) Mix = 'A';
        else if (mixDiff <= 2000) Mix = 'B';
        else if (mixDiff <= 4000) Mix = 'C';
        else Mix = 'D';
    }

    public void UpdateSatisfaction()
    {
        int score = 0;
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("NPC");
        foreach (var gameObject in gameObjects)
        {
            Paint_Wall paint_Wall = gameObject.GetComponent<Paint_Wall>();
            switch (paint_Wall.GetState())
            {
                case Paint_Wall.WallState.CRACKED:
                    score += -20;
                    break;
                case Paint_Wall.WallState.DRY:
                    score += -5;
                    break;
                case Paint_Wall.WallState.PAINTED:
                    score += 10;
                    break;
            }
        }
        if (score >= 490) Satisfaction = 'A';
        else if (score >= 200) Satisfaction = 'B';
        else if (score >= 0) Satisfaction = 'C';
        else Satisfaction = 'D';
    }

    public char GetMix()
    {
        return Mix;
    }

    public char GetSatisfaction()
    {
        return Satisfaction;
    }

    public double GetEarnReward()
    {
        return 23.5645;
    }
}