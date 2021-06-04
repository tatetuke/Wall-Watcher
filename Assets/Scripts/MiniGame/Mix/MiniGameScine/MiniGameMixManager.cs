using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMixManager : MonoBehaviour
{
    //public CameraShake shake;
    enum State
    {
        IsPlaying,
        IsFinished
    }

    private State m_State = State.IsPlaying;

    [SerializeField] GameObject shakeObj;//揺らすゲームオブジェクトの選択
    [SerializeField] Button CompleteButton;

    [SerializeField] GameObject TaskCompleteText;

    [SerializeField] GameObject SoilUpButtonColor;
    [SerializeField] GameObject SoilDownButtonColor;
    [SerializeField] GameObject WaterUpButtonColor;
    [SerializeField] GameObject WaterDownButtonColor;

    [SerializeField] GameObject TripleCircle;
    [SerializeField] GameObject DoubleCircle;
    [SerializeField] GameObject SingleCircle;
    [SerializeField] GameObject Triangle;

    [SerializeField] TextMeshProUGUI Temperature;
    [SerializeField] TextMeshProUGUI Humidity;
    [SerializeField] TextMeshProUGUI HRC;

    [SerializeField] GameObject SoilGaugeGameObject;
    [SerializeField] GameObject WaterGaugeGameObject;
    private Image SoilGauge;
    private Image WaterGauge;

    private float ParamSpeed = 0.003f;
    private float IdealRatio = 2;  // 土/水
    private float SoilGaugeParam;
    private float WaterGaugeParam;


    void Start()
    {
        UpdateText();
        SoilGauge = SoilGaugeGameObject.GetComponent<Image>();
        WaterGauge = WaterGaugeGameObject.GetComponent<Image>();
        SoilGaugeParam = 0;
        WaterGaugeParam = 0;
        CalcIdealRatio();
    }

    void Update()
    {
        if (m_State == State.IsPlaying)
        {
            GlowButton();
            UpdateGauge();
            UpdateMark();
        }
    }

    private void UpdateMark()
    {
        HideMark();
        if (WaterGaugeParam == 0)
        {
            Triangle.SetActive(true);
            return;
        }
        float ratio = SoilGaugeParam / WaterGaugeParam;
        Debug.Log(ratio);
        float relativeError = CalcRelativeError(ratio, IdealRatio);
        Debug.Log(relativeError);
        if (relativeError<=0.05f)
            TripleCircle.SetActive(true);
        else if (relativeError <= 0.1f)
            DoubleCircle.SetActive(true);
        else if (relativeError <= 0.2f)
            SingleCircle.SetActive(true);
        else
            Triangle.SetActive(true);
    }

    private void HideMark()
    {
        TripleCircle.SetActive(false);
        DoubleCircle.SetActive(false);
        SingleCircle.SetActive(false);
        Triangle.SetActive(false);
    }

    private float CalcRelativeError(float MeasuredValue, float TheoreticalValue)
    {
        float relativeError = Mathf.Abs((MeasuredValue - TheoreticalValue) / TheoreticalValue);
        return relativeError;
    }


    private void GlowButton()
    {
        HideGlowBotton();
        if (/*左クリックが押されている*/Input.GetMouseButton(0))
        {
            GameObject cursorObject = GetCursorObject();
            if (cursorObject == null)
                HideGlowBotton();
            else if (cursorObject.name == "SoilUpButton")
                SoilUpButtonColor.SetActive(true);
            else if (cursorObject.name == "SoilDownButton")
                SoilDownButtonColor.SetActive(true);
            else if (cursorObject.name == "WaterUpButton")
                WaterUpButtonColor.SetActive(true);
            else if (cursorObject.name == "WaterDownButton")
                WaterDownButtonColor.SetActive(true);
        }
    }

    private void HideGlowBotton()
    {
        SoilUpButtonColor.SetActive(false);
        SoilDownButtonColor.SetActive(false);
        WaterUpButtonColor.SetActive(false);
        WaterDownButtonColor.SetActive(false);
    }

    private void UpdateGauge()
    {
        if (/*左クリックが押されている*/Input.GetMouseButton(0))
        {
            GameObject cursorObject = GetCursorObject();
            if (cursorObject == null)
                HideGlowBotton();
            else if (cursorObject.name == "SoilUpButton")
            {
                SoilGaugeParam += ParamSpeed;
            }
            else if (cursorObject.name == "SoilDownButton")
            {
                SoilGaugeParam += -ParamSpeed;
            }
            else if (cursorObject.name == "WaterUpButton")
            {
                WaterGaugeParam += ParamSpeed;
            }
            else if (cursorObject.name == "WaterDownButton")
            {
                WaterGaugeParam += -ParamSpeed;
            }
        }
        SoilGaugeParam = AdjustParam(SoilGaugeParam);
        WaterGaugeParam = AdjustParam(WaterGaugeParam);
        SoilGauge.fillAmount = SoilGaugeParam;
        WaterGauge.fillAmount = WaterGaugeParam;
    }

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
        ChangeState(State.IsFinished);
        CompleteButton.interactable = false;
        // シェイク(一定時間のランダムな動き)
        var duration = 5f;    // 時間
        var strength = 0.3f;    // 力
        //strength *= (float)tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1] / 10;
        var vibrato = 100;    // 揺れ度合い
        var randomness = 90f;   // 揺れのランダム度合い(0で一定方向のみの揺れになる)
        var snapping = false; // 値を整数に変換するか
        var fadeOut = true;  // 揺れが終わりに向かうにつれ段々小さくなっていくか(falseだとピタッと止まる)
        shakeObj.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        Invoke("ShowTaskComplete", 5.5f);
    }

    private void ChangeState(State state)
    {
        m_State = state;
    }

    void ShowTaskComplete()
    {
        TaskCompleteText.SetActive(true);
    }
}