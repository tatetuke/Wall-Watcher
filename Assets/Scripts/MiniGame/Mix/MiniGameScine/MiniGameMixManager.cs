using DG.Tweening;
using Fungus;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO : 
// 温度とかのテキスト
// 終了押したら
// マーク

public class MiniGameMixManager : MonoBehaviour
{
    //public CameraShake shake;
    enum State
    {
        IsPlaying,
        IsFinished
    }

    [SerializeField] Flowchart GuideMiniGameFlowChart;
    private State m_State = State.IsPlaying;

    [SerializeField] Animator MachineAnim;
    [SerializeField] Animator MarkAnim;

    [SerializeField] GameObject shakeObj;//揺らすゲームオブジェクトの選択
    [SerializeField] Button CompleteButton;

    //[SerializeField] GameObject TaskCompleteText;

    //[SerializeField] GameObject SoilUpButtonColor;
    //[SerializeField] GameObject SoilDownButtonColor;
    //[SerializeField] GameObject WaterUpButtonColor;
    //[SerializeField] GameObject WaterDownButtonColor;

    //[SerializeField] GameObject SoilGoodArea;
    //[SerializeField] GameObject WaterGoodArea;

    [SerializeField] Image MarkImage;
    [SerializeField] GameObject MarkAnimationGameObject;
    [SerializeField] GameObject MarkGameObject;
    [SerializeField] GameObject SoilHantei;
    [SerializeField] GameObject WaterHantei;
    [SerializeField] Sprite TripleCircleMark;
    [SerializeField] Sprite DoubleCircleMark;
    [SerializeField] Sprite SingleCircleMark;
    [SerializeField] Sprite TriangleMark;

    [SerializeField] Text Temperature;
    [SerializeField] Text Humidity;
    [SerializeField] Text HRC;

    [SerializeField] GameObject SoilArea;
    [SerializeField] GameObject WaterArea;
    [SerializeField] GameObject SoilGaugeGameObject;
    [SerializeField] GameObject WaterGaugeGameObject;
    private Image SoilGauge;
    private Image WaterGauge;
    [SerializeField] Button SoilButton;
    [SerializeField] Button WaterButton;

    private float ParamSpeed = 0.01f;
    private float IdealRatio = 2;  // 土/水
    private float SoilGaugeParam;
    private float WaterGaugeParam;
    private float DeltaSoilParam;
    private float DeltaWaterParam;
    private int Score = 0;
    private bool isCompleted = true;


    void Start()
    {
        MarkImage.sprite = TriangleMark;
        UpdateText();
        SoilGauge = SoilGaugeGameObject.GetComponent<Image>();
        WaterGauge = WaterGaugeGameObject.GetComponent<Image>();
        MiniGameMixInit();
        //SoilGaugeParam = 0;
        //WaterGaugeParam = 0;
        //CalcIdealRatio();
        //SetSoilArea();
        //WaterSoilArea();
        ////DeltaSoilParam = Random.Range(0.005f, 0.01f);
        ////DeltaWaterParam = Random.Range(0.008f, 0.015f);
        //DeltaSoilParam = 0.008f;
        //DeltaWaterParam = 0.015f;
    }

    public void MiniGameMixInit()
    {
        isCompleted = false;
        CompleteButton.interactable = true;
        SoilButton.interactable = true;
        WaterButton.interactable = true;
        SoilGaugeParam = 0;
        WaterGaugeParam = 0;
        CalcIdealRatio();
        SetSoilArea();
        WaterSoilArea();
        //DeltaSoilParam = Random.Range(0.005f, 0.01f);
        //DeltaWaterParam = Random.Range(0.008f, 0.015f);
        DeltaSoilParam = 0.008f;
        DeltaWaterParam = 0.015f;
        MarkGameObject.SetActive(false);
        Score = 0;
        SoilHantei.SetActive(false);
        WaterHantei.SetActive(false);
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    //GameObject g = GameObject.Find("HRC");
        //    Text text = WaterHantei.GetComponent<Text>();
        //    text.color = new Color(0f / 255f, 96f / 255f, 255f / 255f);
        //}
        //DecideMark(SoilArea, SoilGauge);
        SoilGauge.fillAmount += DeltaSoilParam;
        WaterGauge.fillAmount += DeltaWaterParam;
        if (SoilGauge.fillAmount >= 1 || SoilGauge.fillAmount <= 0) DeltaSoilParam *= -1;
        if (WaterGauge.fillAmount >= 1 || WaterGauge.fillAmount <= 0) DeltaWaterParam *= -1;

        if (m_State == State.IsPlaying)
        {
            //GlowButton();
            //UpdateGauge();
            //UpdateMark();
        }
    }

    private void SetSoilArea()
    {
        float mn = 122f;
        float mx = 330f;
        Vector3 pos = SoilArea.transform.localPosition;
        pos.y = Random.Range(mn, mx);
        SoilArea.transform.localPosition = pos;
    }

    private void WaterSoilArea()
    {
        float mn = 122f;
        float mx = 330f;
        Vector3 pos = WaterArea.transform.localPosition;
        pos.y = Random.Range(mn, mx);
        WaterArea.transform.localPosition = pos;
    }

    public void OnSoilButtonClick()
    {
        SoilHantei.SetActive(true);
        DeltaSoilParam = 0;
        SoilButton.interactable = false;
        Text hanteiText = SoilHantei.GetComponent<Text>();
        float mn, mx, diff;
        GameObject GoodArea;
        mn = 436f; mx = 753f;
        GoodArea = SoilArea.transform.Find("GoodArea").gameObject;
        diff = Mathf.Abs((GoodArea.transform.position.y - mn) / (mx - mn) - SoilGauge.fillAmount);
        if (diff < 15f / (mx - mn))
        {
            Score += 2;
            hanteiText.text = "Excellent";
            hanteiText.color = Color.red;
        }
        else if (diff < 50f / (mx - mn))
        {
            Score += 1;
            hanteiText.text = "Good";
            hanteiText.color = new Color(255f / 255f, 146f / 255f, 0f / 255f);
        }
        else
        {
            Score += 0;
            hanteiText.text = "Bad";
            hanteiText.color = new Color(0f / 255f, 96f / 255f, 255f / 255f);
        }
    }

    public void OnWaterButtonClick()
    {
        WaterHantei.SetActive(true);
        DeltaWaterParam = 0;
        WaterButton.interactable = false;
        Text hanteiText = WaterHantei.GetComponent<Text>();
        float mn, mx, diff;
        GameObject GoodArea;
        mn = 436f; mx = 745f;
        GoodArea = WaterArea.transform.Find("GoodArea").gameObject;
        diff = Mathf.Abs((GoodArea.transform.position.y - mn) / (mx - mn) - WaterGauge.fillAmount);
        if (diff < 15f / (mx - mn))
        {
            Score += 2;
            hanteiText.text = "Excellent";
            hanteiText.color = Color.red;
        }
        else if (diff < 50f / (mx - mn))
        {
            Score += 1;
            hanteiText.text = "Good";
            hanteiText.color = new Color(255f / 255f, 146f / 255f, 0f / 255f);
        }
        else
        {
            Score += 0;
            hanteiText.text = "Bad";
            hanteiText.color = new Color(0f / 255f, 96f / 255f, 255f / 255f);
        }
    }

    //private void UpdateMark()
    //{
    //    HideMark();
    //    if (WaterGaugeParam == 0)
    //    {
    //        Triangle.SetActive(true);
    //        return;
    //    }
    //    float ratio = SoilGaugeParam / WaterGaugeParam;
    //    Debug.Log(ratio);
    //    float relativeError = CalcRelativeError(ratio, IdealRatio);
    //    Debug.Log(relativeError);
    //    if (relativeError<=0.05f)
    //        TripleCircle.SetActive(true);
    //    else if (relativeError <= 0.1f)
    //        DoubleCircle.SetActive(true);
    //    else if (relativeError <= 0.2f)
    //        SingleCircle.SetActive(true);
    //    else
    //        Triangle.SetActive(true);
    //}

    //private void HideMark()
    //{
    //    TripleCircle.SetActive(false);
    //    DoubleCircle.SetActive(false);
    //    SingleCircle.SetActive(false);
    //    Triangle.SetActive(false);
    //}

    private float CalcRelativeError(float MeasuredValue, float TheoreticalValue)
    {
        float relativeError = Mathf.Abs((MeasuredValue - TheoreticalValue) / TheoreticalValue);
        return relativeError;
    }


    //private void GlowButton()
    //{
    //    HideGlowBotton();
    //    if (/*左クリックが押されている*/Input.GetMouseButton(0))
    //    {
    //        GameObject cursorObject = GetCursorObject();
    //        if (cursorObject == null)
    //            HideGlowBotton();
    //        else if (cursorObject.name == "SoilUpButton")
    //            SoilUpButtonColor.SetActive(true);
    //        else if (cursorObject.name == "SoilDownButton")
    //            SoilDownButtonColor.SetActive(true);
    //        else if (cursorObject.name == "WaterUpButton")
    //            WaterUpButtonColor.SetActive(true);
    //        else if (cursorObject.name == "WaterDownButton")
    //            WaterDownButtonColor.SetActive(true);
    //    }
    //}

    //private void HideGlowBotton()
    //{
    //    SoilUpButtonColor.SetActive(false);
    //    SoilDownButtonColor.SetActive(false);
    //    WaterUpButtonColor.SetActive(false);
    //    WaterDownButtonColor.SetActive(false);
    //}

    //private void UpdateGauge()
    //{
    //    if (/*左クリックが押されている*/Input.GetMouseButton(0))
    //    {
    //        GameObject cursorObject = GetCursorObject();
    //        if (cursorObject == null)
    //            HideGlowBotton();
    //        else if (cursorObject.name == "SoilUpButton")
    //        {
    //            SoilGaugeParam += ParamSpeed;
    //        }
    //        else if (cursorObject.name == "SoilDownButton")
    //        {
    //            SoilGaugeParam += -ParamSpeed;
    //        }
    //        else if (cursorObject.name == "WaterUpButton")
    //        {
    //            WaterGaugeParam += ParamSpeed;
    //        }
    //        else if (cursorObject.name == "WaterDownButton")
    //        {
    //            WaterGaugeParam += -ParamSpeed;
    //        }
    //    }
    //    SoilGaugeParam = AdjustParam(SoilGaugeParam);
    //    WaterGaugeParam = AdjustParam(WaterGaugeParam);
    //    SoilGauge.fillAmount = SoilGaugeParam;
    //    WaterGauge.fillAmount = WaterGaugeParam;
    //}

    // [0,1]の範囲に修正する
    private float AdjustParam(float num)
    {
        if (num <= 0) num = 0;
        if (num >= 1) num = 1;
        return num;
    }

    private void CalcIdealRatio()
    {
        IdealRatio = 2;
    }

    private void UpdateText()
    {
        Temperature.text = "10";
        Humidity.text = "20";
        HRC.text = "30";
    }

    private GameObject GetCursorObject()
    {
        GameObject cursorObject;
        cursorObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        if (hit2d)
        {
            cursorObject = hit2d.transform.gameObject;
        }
        return cursorObject;
    }

    public void CompleteButtonClick()
    {
        CompleteButton.interactable = false;
        MarkAnimationGameObject.SetActive(true);
        MarkAnim.SetBool("IsStarted", true);
        Invoke("CalcMark", 2f);
        Invoke("FinishTask", 3f);

        //CompleteButton.interactable = false;
        //// シェイク(一定間のランダムな動き)
        //var duration = 5f;    // 時間
        //var strength = 0.3f;    // 力
        ////strength *= (float)tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1] / 10;
        //var vibrato = 100;    // 揺れ度合い
        //var randomness = 90f;   // 揺れのランダム度合い(0で一定方向のみの揺れになる)
        //var snapping = false; // 値を整数に変換するか
        //var fadeOut = true;  // 揺れが終わりに向かうにつれ段々小さくなっていくか(falseだとピタッと止まる)
        //shakeObj.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        ////Invoke("ShowTaskComplete", 5.5f);
    }

    private void CalcMark()
    {
        MarkGameObject.SetActive(true);
        MarkAnimationGameObject.SetActive(false);
        MarkAnim.SetBool("IsStarted", false);
        //int score = 0;
        //float mn, mx, diff;
        //GameObject GoodArea;
        //mn = 436f; mx = 753f;
        //GoodArea = SoilArea.transform.Find("GoodArea").gameObject;
        //diff = Mathf.Abs((GoodArea.transform.position.y - mn) / (mx - mn) - SoilGauge.fillAmount);
        //if (diff < 15f / (mx - mn)) score += 2;
        //else if (diff < 50f / (mx - mn)) score += 1;
        //else score += 0;

        //mn = 436f; mx = 745f;
        //GoodArea = WaterArea.transform.Find("GoodArea").gameObject;
        //diff = Mathf.Abs((WaterArea.transform.position.y - mn) / (mx - mn) - WaterGauge.fillAmount);
        //if (diff < 15f / (mx - mn)) score += 2;
        //else if (diff < 50f / (mx - mn)) score += 1;
        //else score += 0;

        if (Score == 4) MarkImage.sprite = TripleCircleMark;
        else if (Score == 3) MarkImage.sprite = DoubleCircleMark;
        else if (Score == 2 || Score == 1) MarkImage.sprite = SingleCircleMark;
        else MarkImage.sprite = TriangleMark;
        Debug.Log("score:" + Score);
    }

    void FinishTask()
    {
        isCompleted = true;
        MachineAnim.SetBool("IsStarted", false);
    }

    private void ChangeState(State state)
    {
        m_State = state;
    }

    //void ShowTaskComplete()
    //{
    //    TaskCompleteText.SetActive(true);
    //}

    //public void CompleteMix()
    //{
    //    ChangeState(State.Close);
    //    MachineAnim.SetBool("IsStarted", false);
    //}

    void DecideMark(GameObject gameObject, Image gauge)
    {
        float mn = 436f;
        float mx = 753f;  // 745
        GameObject GoodArea = gameObject.transform.Find("GoodArea").gameObject;
        GameObject ExcellentArea = gameObject.transform.Find("ExcellentArea").gameObject;
        float pos = (GoodArea.transform.position.y - mn) / (mx - mn);
        if (Mathf.Abs(pos - gauge.fillAmount) < 15f / (mx - mn)) Debug.Log("Red");
        else if (Mathf.Abs(pos - gauge.fillAmount) < 50f / (mx - mn)) Debug.Log("Orange");
        else Debug.Log("Miss");
    }

    public void SetIsCompleted()
    {
        GuideMiniGameFlowChart.SetBooleanVariable("IsCompleted", isCompleted);
    }
}