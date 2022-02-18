using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

using map;  // マップ番号

public class TItleUIController : MonoBehaviour
{
    [Header("Object references")]
    [SerializeField] Animator PushAnyKeyAnimator;
    [SerializeField] Transform optionsParent;//選択肢を保持する親
    [Header("Options")]
    [SerializeField] string loadGameSceneName = "FileSelect";//LOAD GAME押したときに飛ぶシーン名
    [SerializeField] string newGameSceneName = "MainMap3_CircleWay";//NEW GAME押したときに飛ぶシーン名
    [SerializeField] KeyCode enterKey=KeyCode.Space;//選択肢を決定するキー
    [Header("Debug")]
    [SerializeField,ReadOnly] int m_Select=0;//選択中のタイトルを表す変数.
    //選択しのリスト
    List<TitleOptionContainer> m_options = new List<TitleOptionContainer>();
    enum TitleState
    {
        PushAnyKey,
        GameTitleSlideMovie,
        GameTitle,
        Config,
    }
    [SerializeField,ReadOnly]private TitleState m_State;

    private void Awake()
    {
        foreach(Transform child in optionsParent)
        {
            m_options.Add(child.GetComponent<TitleOptionContainer>());
        }
        m_State = TitleState.PushAnyKey;
    }

    void Update()
    {
        switch (m_State)
        {
            case TitleState.PushAnyKey://何かボタンを押してくださいの状態
                if (Input.anyKey)
                {
                    //何かキーが押されたらゲームタイトルを表示させるアニメーションを出す。
                    Debug.Log("何らかのキーが押されました");
                    PushAnyKeyAnimator.SetBool("IsPushAnyKey", true);
                    m_State = TitleState.GameTitleSlideMovie;
                    if (!SaveDataReader.Instance.ExistsSaveFiles())
                    {
                        //セーブデータが存在しない場合LoadGamesを選べないようにする
                        m_options[0].enableFlag = false;
                    }
                }
            break;

            case TitleState.GameTitleSlideMovie://タイトルの選択画面に移る間のアニメーションの状態
                //ムービーが終わったらタイトル選択の状態へ遷移
                if (PushAnyKeyAnimator.GetCurrentAnimatorStateInfo(0).IsName("SelectState"))
                {
                    m_State = TitleState.GameTitle;
                    //選択肢のうち最初のやつが選択された状態にする
                    m_options[0].SetActive();
                }
            break;

            case TitleState.GameTitle://タイトルの選択画面の状態
                Debug.Log("選択フェーズ");

                //選択肢を選ぶ。
                SelectTitle();
                //選択肢の決定
                if (Input.GetKeyDown(enterKey))
                {
                    OnSelectOption();
                }
                break;
            case TitleState.Config://Configが選択された状態

                break;
           
        }
    }

    void OnSelectOption()
    {
        //選択できない場合終了
        if (!m_options[m_Select].enableFlag) return;
        switch (m_Select)
        {
            case 0:
                SceneManager.LoadScene(loadGameSceneName);
                break;
            case 1:
                Debug.Log("PlayNewGame");
                //FadeManager.Instance.LoadLevel(newGameSceneName, 1.5f);
                AllMapSet.warp_player_position(MAP_NUM.MainMap3F_Floor, MAP_NUM.MainMap3F_Floor, 0, -2, Direction2D.Invalid, 2.5f);
                break;
            case 2:
                m_State = TitleState.Config;
                break;
            case 3://ゲームを終了する
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
      UnityEngine.Application.Quit();
#endif
                break;
        }
    }

    /// <summary>
    /// タイトルを選択するときに選択肢の色を変化させる関数。
    /// </summary>
    private void SelectTitle()
    {
        
        if (Input.GetKeyDown("down"))
        {
            m_options[m_Select].SetInactive();
            m_Select++;
            m_Select %= m_options.Count;
            m_options[m_Select].SetActive();
        }
        else if (Input.GetKeyDown("up"))
        {
            m_options[m_Select].SetInactive();
            m_Select += m_options.Count;
            m_Select--;
            m_Select %= m_options.Count;
            m_options[m_Select].SetActive();
        }

    }

}
