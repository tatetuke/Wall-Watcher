using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// ミニゲームの内容自体を管理する
/// </summary>
public class MixManager : MonoBehaviour
{
    ScoreManager Score;
    RuleManager Rule;
    public int MaxRGB = 78;

    //public GameObject PressAnyKeyText;
    //public GameObject ResultText;
    //public GameObject StartText;
    //public GameObject FinishText;
    //public GameObject FinishTaskButtonCanvas;
    //public Text SatisfactionText;
    //public Text MixText;
    //public Text EarnRewardText;
    //public Button FinishTaskButton;

    bool IsStarted;
    bool IsFinished;

    public GameObject prefab;
    bool IsInputing = false;

    // 壁の状態
    int[,] Wall;

    public void Start()
    {
        Score = new ScoreManager();
        // Rule = new RuleManager();
        IsStarted = false;
        IsFinished = false;
        MaxRGB = Score.GetMaxRGB();
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsInputing = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            IsInputing = false;
        }

        //if (IsStarted == false && Input.GetKeyDown(KeyCode.Space))
        //{
        //    IsStarted = true;
        //    PressAnyKeyText.SetActive(false);
        //    ShowStartText();
        //    Invoke("ClearStartText", 2);
        //}

        //if (IsFinished)
        //{
        //    // ボタン押したらメインシーン戻るなど。。。
        //}
        //Score.UpdateScore();

        GameObject gameObject = getClickObject();
        if (gameObject != null)
        {
            Mix_Wall mix_Wall = gameObject.GetComponent<Mix_Wall>();
            if (mix_Wall.ColorNum >= MaxRGB) mix_Wall.ColorNum -= 3;
            byte num = mix_Wall.ColorNum;
            gameObject.GetComponent<SpriteRenderer>().color = new Color32(num, num, num, 255);
        }
    }


    private GameObject getClickObject()
    {
        GameObject result = null;
        // 左クリックされた場所のオブジェクトを取得
        //if (Input.GetMouseButtonDown(0))
        if(IsInputing)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit))
            {
                result = hit.collider.gameObject;
            }
        }
        return result;
    }


    //private void ChangeColor(GameObject gameObject)
    //{
        

    //    Color nowColor = gameObject.GetComponent<Renderer>().material.color;
    //    Color nextColor = nowColor;
    //    //Color randomColor = new Color(Random.value, Random.value, Random.value, 1.0f);
    //    //gameObject.GetComponent<Renderer>().material.color = randomColor;


    //    if (ClickNum == 1) nextColor = Color.green;
    //    else if (ClickNum == 2) nextColor = Color.cyan;
    //    else nextColor = Color.white;

    //    gameObject.GetComponent<Renderer>().material.color = nextColor;
    //}

    //public void ShowStartText()
    //{
    //    StartText.SetActive(true);
    //}

    //public void ClearStartText()
    //{
    //    StartText.SetActive(false);
    //    FinishTaskButtonCanvas.SetActive(true);
    //}


    //// 作業終了ボタンが押されたらミニゲーム終了
    //public void FinishMiniGame()
    //{
    //    FinishTaskButton.interactable = false;
    //    FinishText.SetActive(true);
    //    Invoke("ShowResult", 2);
    //}

    //public void ShowResult()
    //{
    //    IsFinished = true;
    //    FinishText.SetActive(false);
    //    ResultText.SetActive(true);
    //    // スコアを書き加える
    //    SatisfactionText.text += "    " + Score.GetSatisfaction();
    //    MixText.text += "    " + Score.GetMix();
    //    string specified = Score.GetEarnReward().ToString("F4");
    //    EarnRewardText.text += "      " + specified;
    //}
}
