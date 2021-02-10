using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TItleUIController : MonoBehaviour
{
    [SerializeField]
    Animator PushAnyKeyAnimator;
    private int m_Select=0;//選択中のタイトルを表す変数.
    private bool IsSelectChange=true;
    [SerializeField] Text[] TitleText;
    [SerializeField] Image[] TitleImage;


    void Update()
    {
        if (Input.anyKey)
        {
            Debug.Log("何らかのキーが押されました");
            PushAnyKeyAnimator.SetBool("IsPushAnyKey", true);
            PushAnyKeyAnimator.SetBool("IsPushAnyKey", true);
            
        }

        if (PushAnyKeyAnimator.GetCurrentAnimatorStateInfo(0).IsName("SelectState"))
        {
            Debug.Log("選択フェーズ");
            if (Input.GetKeyDown(KeyCode.Space) && m_Select == 0)
            {
                Debug.Log("PlayNewGame");
                FadeManager.Instance.LoadLevel("MainMap3_CircleWay", 1.5f);
            }

            if (Input.GetKeyDown("down"))
            {
             
                    ChangeColorDown();
                    m_Select++;
                    m_Select %= TitleText.Length;
                    ChangeColorUp();
                
    
            }
            else if (Input.GetKeyDown("up"))
            {

                ChangeColorDown();
                m_Select += TitleText.Length;
                m_Select--;
                m_Select %= TitleText.Length;
                ChangeColorUp();

            }

        }
    }



    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を薄くする関数
    /// </summary>
    private void ChangeColorDown()
    {
        TitleText[m_Select].color = new Color32(0, 0, 0, 100);
        TitleImage[m_Select].color = new Color32(255, 255, 255, 0);
    }
    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を濃くする関数
    /// </summary>
    private void ChangeColorUp()
    {
        TitleText[m_Select].color = new Color32(0, 0, 0, 255);
        TitleImage[m_Select].color = new Color32(255, 255, 255, 255);
    }
}
