using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HakaiTutorialManager : MonoBehaviour
{
    [SerializeField] private MinGameHakaiManager2 gameManager;
    [SerializeField] private HakaiSoundManager soundManager;

    [System.Serializable]
    public struct BLOCK
    {
        /// <summary>
        /// 説明文
        /// </summary>
        public string describe;

        /// <summary>
        /// 画像
        /// </summary>
        public Sprite img;
    }
    [SerializeField, Tooltip("チュートリアルの情報")]
    private List<BLOCK> scenario=new List<BLOCK>();


    public enum STATE
    {
        NOT_PLAYING,
        PRE_PROCESSING,
        PLAYING,
        END_PROCESSING
    }
    static public STATE state=STATE.NOT_PLAYING;
    [SerializeField]private GameObject tutorialScene;
    [SerializeField]private Text describe;
    [SerializeField]private Image img;
    [SerializeField]private Text page;
    [SerializeField]private Image leftButton;
    [SerializeField]private Image rightButton;
    [SerializeField]private int pageNumber;

    // Update is called once per frame
    void Update()
    {
        Debug.Log("チュートリアルの状態："+state);
        switch (state)
        {
            case STATE.PRE_PROCESSING:
                InitScenario();
                state = STATE.PLAYING;
                break;
            case STATE.PLAYING:
                InputKey();
                break;
            
            case STATE.END_PROCESSING:
                EndTutorial();
                break;

            case STATE.NOT_PLAYING:
                break;
        }
    }

    /// <summary>
    /// 入力受付
    /// </summary>
    private void InputKey()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown("a"))
        {
            MovePreviousPage();
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown("d"))
        {
            MoveNextPage();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitTutorial();
        }
        return;
    }
    
    /// <summary>
    ///　次のページを開く
    /// </summary>
    public void MoveNextPage()
    {

        if (state != STATE.PLAYING) return;

        //次のページが存在しない場合、チュートリアルを終了する
        if (pageNumber+1 >= scenario.Count)
        {
            ExitTutorial();
            return;
        }
        //ページを進める
        pageNumber++;
        //SEを鳴らす
        soundManager.PlayTurnThePage();
        UpdatePage();
        return; 
    }
    /// <summary>
    ///　前のページを開く
    /// </summary>
    public void MovePreviousPage()
    {
        if (state != STATE.PLAYING) return;
        //初めのページの場合ページを更新しない。
        if (pageNumber-1 <0)
        {
            return;
        }
        //ページを戻す。
        pageNumber--;
        //SEを鳴らす
        soundManager.PlayTurnThePage();
        UpdatePage();
        return;
    }
    /// <summary>
    /// ページの更新
    /// </summary>
    public void UpdatePage()
    {
        img.sprite = scenario[pageNumber].img;
        describe.text = scenario[pageNumber].describe;
        page.text = (pageNumber+1).ToString() + " / " + scenario.Count.ToString();
        return;
    }
    /// <summary>
    /// 終了処理
    /// </summary>
    private void EndTutorial()
    {
        //ミニゲームの状態をPLAYINGに変更する
        gameManager.State = MinGameHakaiManager2.GAME_STATE.PLAYING;
        state = STATE.NOT_PLAYING;
        tutorialScene.SetActive(false);
        return;
    }
    
    public void ExitTutorial()
    {
        soundManager.PlayTurnThePage();
        state = STATE.END_PROCESSING;
        return;
    }

    //チュートリアルを始める
    public void StartTutorial()
    {
        if (state != STATE.NOT_PLAYING) return;
        if (gameManager.State != MinGameHakaiManager2.GAME_STATE.PLAYING) return;
        //ミニゲームの状態をPAUSEに変更する
        gameManager.State = MinGameHakaiManager2.GAME_STATE.PAUSE;
        state = STATE.PRE_PROCESSING;
        soundManager.PlayOpenTutorial();
        return;
    }


    /// <summary>
    /// 初期化
    /// </summary>
    private void InitScenario()
    {
        pageNumber = 0;
        UpdatePage();
        tutorialScene.SetActive(true);
        return;
    }
}
