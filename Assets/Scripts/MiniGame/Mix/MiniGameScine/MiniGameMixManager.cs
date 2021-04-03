using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniGameMixManager : MonoBehaviour
{
    public float ParamSpeed = 0.001f;

    [SerializeField] GameObject SoilUpButtonColor;
    [SerializeField] GameObject SoilDownButtonColor;
    [SerializeField] GameObject WaterUpButtonColor;
    [SerializeField] GameObject WaterDownButtonColor;

    [SerializeField] GameObject SoilGaugeGameObject;
    [SerializeField] GameObject WaterGaugeGameObject;
    private Image SoilGauge;
    private Image WaterGauge;


    private float SoilGaugeParam;
    private float WaterGaugeParam;


    void Start()
    {
        SoilGauge = SoilGaugeGameObject.GetComponent<Image>();
        WaterGauge = WaterGaugeGameObject.GetComponent<Image>();
        SoilGaugeParam = 0;
        WaterGaugeParam = 0;
    }

    void Update()
    {
        GlowButton();
        UpdateGauge();
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
}