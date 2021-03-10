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
    public float MaxRGB = 80;

    public Material Brown;
    public Material Green;

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
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            IsInputing = true;
        }

        if (Input.GetMouseButtonUp(0))
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
            WallController wallController = gameObject.GetComponent<WallController>();
            wallController.CountUp();
            if (wallController.ColorNum >= MaxRGB) wallController.ColorNum -= 3;

            //ChangeColor(gameObject);
            byte num = wallController.ColorNum;
            //gameObject.GetComponent<Renderer>().material.color = new Color(num, num, num, 1);
            //gameObject.GetComponent<SpriteRenderer>().color = Color.gray;
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

    public void OnClick()
    {
        SceneManager.LoadScene("Paint");

        //GameObject[] g = GameObject.FindGameObjectsWithTag("NPC");
        //foreach(var a in g)
        //{
        //    a.GetComponent<SpriteRenderer>().color = Color.gray;
        //}
        ////g.GetComponent<SpriteRenderer>().color = Color.gray;
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
