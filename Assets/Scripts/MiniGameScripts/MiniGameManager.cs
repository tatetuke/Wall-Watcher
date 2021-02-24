using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ミニゲームの内容自体を管理する
/// </summary>
public class MiniGameManager : MonoBehaviour
{
    ScoreManager Score;
    RuleManager Rule;

    public GameObject PressAnyKeyText;
    public GameObject ResultText;
    public GameObject StartText;
    public GameObject FinishText;
    public GameObject FinishTaskButtonCanvas;
    public Text SatisfactionText;
    public Text MixText;
    public Text EarnRewardText;
    public Button FinishTaskButton;

    bool IsStarted;
    bool IsFinished;

    // 壁の状態
    int[,] Wall;

    public void Start()
    {
        Score = new ScoreManager();
        // Rule = new RuleManager();
        IsStarted = false;
        IsFinished = false;
    }

    public void Update()
    {
        if (IsStarted == false && Input.GetKeyDown(KeyCode.Space))
        {
            IsStarted = true;
            PressAnyKeyText.SetActive(false);
            ShowStartText();
            Invoke("ClearStartText", 2);
        }

        if (IsFinished)
        {
            // ボタン押したらメインシーン戻るなど。。。
        }
        Score.UpdateScore();
    }

    public void ShowStartText()
    {
        StartText.SetActive(true);
    }

    public void ClearStartText()
    {
        StartText.SetActive(false);
        FinishTaskButtonCanvas.SetActive(true);
    }


    // 作業終了ボタンが押されたらミニゲーム終了
    public void FinishMiniGame()
    {
        FinishTaskButton.interactable = false;
        FinishText.SetActive(true);
        Invoke("ShowResult", 2);
    }

    public void ShowResult()
    {
        IsFinished = true;
        FinishText.SetActive(false);
        ResultText.SetActive(true);
        // スコアを書き加える
        SatisfactionText.text += "    " + Score.GetSatisfaction();
        MixText.text += "    " + Score.GetMix();
        string specified = Score.GetEarnReward().ToString("F4");
        EarnRewardText.text += "      " + specified;
    }
}
