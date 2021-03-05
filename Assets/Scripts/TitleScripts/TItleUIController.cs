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
    enum TitleState
    {
        PushAnyKey,
        GameTitleSlideMovie,
        GameTitle,
        NewGame,
        LoadGame,
        Config,
        End
    }
    [SerializeField]private TitleState TState;

    private void Start()
    {
        TState = TitleState.PushAnyKey;
    }

    void Update()
    {
        switch (TState)
        {
            case TitleState.PushAnyKey://何かボタンを押してくださいの状態

            if (Input.anyKey)
            {
                //何かキーが押されたらゲームタイトルを表示させるアニメーションを出す。
                Debug.Log("何らかのキーが押されました");
                PushAnyKeyAnimator.SetBool("IsPushAnyKey", true);
                TState = TitleState.GameTitleSlideMovie;
            }
            break;

            case TitleState.GameTitleSlideMovie://タイトルの選択画面に移る間のアニメーションの状態

                if (PushAnyKeyAnimator.GetCurrentAnimatorStateInfo(0).IsName("SelectState"))
                {
                    TState = TitleState.GameTitle;
                }
            break;

            case TitleState.GameTitle://タイトルの選択画面の状態
                Debug.Log("選択フェーズ");

                //選択肢を選ぶ。
                SelectTitle();
                //選択肢の決定
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (m_Select == 0)//NewGameに状態遷移する。
                    {
                        Debug.Log("PlayNewGame");
                        TState = TitleState.NewGame;
                    }
                    if (m_Select == 1)//LoadGameに状態遷移する。
                    {
                        TState = TitleState.LoadGame;
                    }
                    if (m_Select == 2)//Configに状態遷移する。
                    {
                        TState = TitleState.Config;
                    }
                }
                break;

            case TitleState.NewGame://NewGameが選択された状態

                FadeManager.Instance.LoadLevel("MainMap3_CircleWay", 1.5f);
                TState = TitleState.End;
                break;

            case TitleState.LoadGame://LoadGameが選択された状態

                break;

            case TitleState.Config://Configが選択された状態

                break;
           
        }
    }



    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を薄くする関数
    /// </summary>
    private void ChangeColorDown()
    {
        //黒
        TitleText[m_Select].color = new Color32(0, 0, 0, 100);
        //無色透明
        TitleImage[m_Select].color = new Color32(255, 255, 255, 0);
    }
    /// <summary>
    /// タイトル画面の選択肢において、テキストとスプライトの色を濃くする関数
    /// </summary>
    private void ChangeColorUp()
    {
        //黒
        TitleText[m_Select].color = new Color32(0, 0, 0, 255);
        //元画像のまま
        TitleImage[m_Select].color = new Color32(255, 255, 255, 255);
    }
    /// <summary>
    /// タイトルを選択するときに選択肢の色を変化させる関数。
    /// </summary>
    private void SelectTitle()
    {
        
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
