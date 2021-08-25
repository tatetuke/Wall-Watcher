using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MixMachineManager : MonoBehaviour
{
    [Header("縁取りの太さ")] [SerializeField] float OutlineWidthInit = 1f;
    [SerializeField] Animator MachineAnim;

    [SerializeField] GameObject SoilGaugeGameObject;
    [SerializeField] GameObject WaterGaugeGameObject;
    private Image SoilGauge;
    private Image WaterGauge;

    private float SoilGaugeParam;
    private float WaterGaugeParam;
    private float DeltaSoilParam;
    private float DeltaWaterParam;

    State m_State = State.Far;
    public enum State { 
        Far,
        Close,
        Playing
    }


    void Start()
    {
        Material material = this.GetComponent<Renderer>().material;
        material.SetFloat("Vector1_C1366B5E", OutlineWidthInit);
        SoilGauge = SoilGaugeGameObject.GetComponent<Image>();
        WaterGauge = WaterGaugeGameObject.GetComponent<Image>();
        DeltaSoilParam = Random.Range(0.005f, 0.01f);
        DeltaWaterParam = Random.Range(0.008f, 0.015f);
    }

    void Update()
    {
        Debug.Log(MachineAnim.GetBool("IsStarted"));
        if (Input.GetKeyDown("space"))
        {
            if (m_State == State.Close)
            {
                ChangeState(State.Playing);
                MachineAnim.SetBool("IsStarted", true);
                //FadeManager.Instance.LoadLevel("Mix", 1f);
                //Debug.Log("MiniGameMixシーンに遷移!");
            }
        }

        if (m_State == State.Playing)
        {
            SoilGauge.fillAmount += DeltaSoilParam;
            WaterGauge.fillAmount += DeltaWaterParam;
            if (SoilGauge.fillAmount >= 1 || SoilGauge.fillAmount <= 0) DeltaSoilParam *= -1;
            if (WaterGauge.fillAmount >= 1 || WaterGauge.fillAmount <= 0) DeltaWaterParam *= -1;
        }
    }

    public void OnSoilButtonClick()
    {
        DeltaSoilParam = 0;
    }

    public void OnWaterButtonClick()
    {
        DeltaWaterParam = 0;
    }

    // プレイヤーが近かったら赤く表示
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetGlowLine(Color.red);
            Debug.Log("Closeになった");
            ChangeState(State.Close);
        }
    }

    // プレイヤーが遠かったらシアンで表示
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SetGlowLine(Color.cyan);
            Debug.Log("Farになった");
            ChangeState(State.Far);
        }
    }

    void SetGlowLine(Color color)
    {
        Material material = this.GetComponent<Renderer>().material;
        material.SetColor("Color_7C7012AB", color);
    }

    private bool IsInCollider()
    {
        Material material = this.GetComponent<Renderer>().material;
        return material.GetColor("Color_7C7012AB") == Color.red;
    }

    public void CompleteMix()
    {
        ChangeState(State.Close);
        MachineAnim.SetBool("IsStarted", false);
    }


    void ChangeState(State state)
    {
        m_State = state;
        if (state == State.Playing)
        {
            MachineAnim.SetBool("IsStarted", true);
        }
    }
}
