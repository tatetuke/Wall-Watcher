using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using UnityEngine.UI;
using Kyoichi;
public class MinGameHakaiManager2 : MonoBehaviour
{
    public int gameType = 1;
    public const int RawSize = 8;//盤面のサイズ
    public const int ColumnSize = 11;
    [HideInInspector]public int Rsize=RawSize; 
    [HideInInspector]public int Csize=ColumnSize; 
    [HideInInspector]public GameObject[,] Wall = new GameObject[RawSize, ColumnSize];//盤面全体
    [HideInInspector]public GameObject[,] WallAnime = new GameObject[RawSize, ColumnSize];//盤面全体
    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };//裏返す壁のIndex
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };//
    public string PolutedLevel1;//壁の画像の名前
    public string PolutedLevel2;//
    public string PolutedLevel3;//
    public string PolutedLevel4;//
    public string PolutedLevel5;//
    public string PolutedLevel6;//
    [SerializeField] private UnityEvent UpdateItemData=new UnityEvent(); //アイテムデータのアップデート

    [SerializeField] private MinGameHAKAIStatus gameStatus;//HPやHPを減らす関数を持つクラス

    [SerializeField]MinGameHakaiToolDataManager toolManager;
    [SerializeField]MinGameHakaiToolData tool;


    [SerializeField] GameObject shakeObj;//揺らすゲームオブジェクトの選択
    [SerializeField] GameObject lifeGage;//揺らすゲームオブジェクトの選択

    [SerializeField] private GameObject result;
    [SerializeField] private Transform resultCanvas;

    public Sprite []WallSprite=new Sprite[6];//壁の画像

    public MingameHAKAIGetItemManager itemManager;

    private Inventry inventory;

    public Image blackImage;//暗転のための画像
    public GameObject blackImageObj;


    [SerializeField] private MinGameHakaiItemGetUI ItemGetUI;//UIのアイテム欄を更新する.

    public HakaiSoundManager soundManager;

    public HakaiEndMessage endMessage;
    public GameObject endMessageObj;

    private Vector3 originalBordPosition;
    private Vector3 originalHPBarPosition;


    //UIがシェイク時にぶれるバグを修正仕様とした跡地
    //private Vector3 initShakeObj;
    //private Vector3 initLifeGage;
    enum GAME_STATE
    {
        PLAYING,
        PRESTART,
        PAUSE,
        RESULT,
        END,
        END_PROCESSING
    }
    [SerializeField] private GAME_STATE State;
    private void Start()
    {
        //インベントリ初期化
        inventory = GameObject.Find("Managers").GetComponent<Inventry>();

        //揺れによってオブジェクトが移動するバグ修正のための
        originalBordPosition = shakeObj.transform.position;
        originalHPBarPosition = lifeGage.transform.position;

        State = GAME_STATE.PRESTART;
        WallInit();
        WallAnimeInit();
        GetSpriteName();
        //取得できるかどうかについてアイテムの情報を更新
        UpdateItemData.Invoke();
        //UIのアイテム情報の更新
        ItemGetUI.ChangeGetItemUI();

        State = GAME_STATE.PLAYING;

        soundManager.PlayBGM();
    }
    private void Update()
    {
        

        //ゲーム状態に応じた処理を選択、実行
        StartCoroutine(MinGameState());


    }
    IEnumerator MinGameState()
    {


        switch (State)
        {
            case GAME_STATE.PRESTART:
                break;
            case GAME_STATE.PLAYING:
                Click();
                //ゲームが終わったかどうかの判定
                CheckGameEnd();

                break;
            case GAME_STATE.RESULT:
                State = GAME_STATE.END_PROCESSING;

                result.SetActive(true);


                //終了メッセージの表示
                yield return  StartCoroutine(EndMessageAnime());


                //リザルトの表示
                yield return  StartCoroutine(CreatGetItem());

                State = GAME_STATE.END;

                break;
            case GAME_STATE.PAUSE:
                break;

            case GAME_STATE.END_PROCESSING://終了の処理待ち
                break;

            case GAME_STATE.END:

                //フェードアウト
                yield return StartCoroutine(FadeOut(1.5f));

                PollutionManager.breakMarker();  // hekimenマップのマーカー状態を変更させる関数
                break;
        }

        yield return 0;

    }
    /// <summary>
    /// Wallを初期化する関数。
    /// タグがWallであるゲームオブジェクトをすべて取得する。
    /// </summary>
    private void WallInit()
    {
        int i, j;
        i = 0;
        j = 0;
        foreach (GameObject v in GameObject.FindGameObjectsWithTag("Wall"))
        {
            if (i >= RawSize)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = v;
            j++;
            if (j == ColumnSize)
            {
                j = 0;
                i++;
            }

        }


    }
    private void WallAnimeInit()
    {
        int i, j;
        i = 0;
        j = 0;
        foreach (GameObject v in GameObject.FindGameObjectsWithTag("WallAnime"))
        {
            if (j >= RawSize)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            WallAnime[i, j] = v;
            i++;
            if (i == ColumnSize)
            {
                i = 0;
                j++;
            }

        }


    }

    /// <summary>
    /// クリックしたオブジェクトを取得
    /// </summary>
    private void Click()
    {

        GameObject clickedGameObject;

        clickedGameObject = null;
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);

        if (hit2d)
        {
            clickedGameObject = hit2d.transform.gameObject;
        }
        //クリックしたものが壁でなければリターン
        if (clickedGameObject==null||clickedGameObject.tag != "Wall") return;


        //下。HPを0にする必要があるため削除?
        ////現在のHPよりくらうダメージが大きい場合,ゲージを揺らしてreturn 
        //if (gameStatus.life - (int)tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1] < 0)
        //{
        //    AttackErrorEffect();
        //    return;
        //}


        //ToDo道具によってSEを変化させる
        //削る時に出るSE。
        switch (toolManager.SelectToolNum)
        {
            case 0:
                soundManager.PlaySE(HakaiSoundManager.SE_TYPE.TOOL1);
                break;
            case 1:
                soundManager.PlaySE(HakaiSoundManager.SE_TYPE.TOOL2);

                break;
            case 2:
                soundManager.PlaySE(HakaiSoundManager.SE_TYPE.TOOL3);

                break;

        }






        Debug.Log(clickedGameObject.name);
        int raw = 0, column = 0;
        //クリックしたタイルのindexを取得。
        (raw, column) = SearchIndex(clickedGameObject);
        //周りのスプライトの画像を変える。
        for (int i = 0; i < 9; i++)
        {
            //使用している道具の裏返せる範囲で無ければスキップ
            if (!tool.Tools[toolManager.SelectToolNum].CanChangeSprite[i]) continue;
            int nraw = raw + dy[i];
            int ncolumn = column + dx[i];
            if (nraw < 0 || nraw >= RawSize || ncolumn < 0 || ncolumn >= ColumnSize) continue;
            if (gameType == 1)
            {
                ChangeSprite1(Wall[nraw, ncolumn],WallAnime[nraw,ncolumn]);
            }else if (gameType == 2)
            {
                ChangeSprite2(Wall[nraw, ncolumn]);

            }else if (gameType == 3)
            {
                ChangeSprite3(Wall[nraw, ncolumn]);
            }
        }
        //道具を使用した時のエフェクトを出す。
        ReverseSpriteEffect();
        //使用した道具に応じて体力を減らす
        //ToDo
        //レベルが0の時の例外処理（多分いらない）
        gameStatus.Damage(tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1]);

        //取得できるかどうかについてアイテムの情報を更新
        UpdateItemData.Invoke();

        //UIのアイテム情報の更新
        ItemGetUI.ChangeGetItemUI();
    }
    //アイテムが使えないときの揺れ
    private void AttackErrorEffect()
    {
        Debug.Log("この道具を使うには体力が足りません");
        //揺らす前に元の位置に初期化
        //lifeGage.transform.position = initLifeGage;
        //画面を揺らす。
        // シェイク(一定時間のランダムな動き)
        var duration = 0.35f;    // 時間
        var strength = 50f;    // 力
        var vibrato = 100;    // 揺れ度合い
        var randomness = 90f;   // 揺れのランダム度合い(0で一定方向のみの揺れになる)
        var snapping = false; // 値を整数に変換するか
        var fadeOut = true;  // 揺れが終わりに向かうにつれ段々小さくなっていくか(falseだとピタッと止まる)
        lifeGage.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
    }

    /// <summary>
    /// 盤面を反転させるときの画面の揺れ
    /// </summary>
    private void ReverseSpriteEffect()
    {
        //揺らす前に位置を初期化
        //shakeObj.transform.position = initShakeObj;
        // シェイク(一定時間のランダムな動き)
        var duration = 0.35f;    // 時間
        var strength = 0.3f;    // 力
        strength *= (float)tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1]/10;
        var vibrato = 100;    // 揺れ度合い
        var randomness = 90f;   // 揺れのランダム度合い(0で一定方向のみの揺れになる)
        var snapping = false; // 値を整数に変換するか
        var fadeOut = true;  // 揺れが終わりに向かうにつれ段々小さくなっていくか(falseだとピタッと止まる)
        shakeObj.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);

        //athleics

        StartCoroutine(SetOriginalPositionBord(duration));


    }
    private IEnumerator SetOriginalPositionBord(float time)
    {
        float t=0;
        
        while(t<=time){

            t += Time.deltaTime;
            yield return null;
        }

        shakeObj.transform.position = originalBordPosition;

        yield return 0;
    }

    /// <summary>
    ///壁の添え字を探索する。
    /// </summary>
    public (int,int) SearchIndex(GameObject obj)
    {
        for (int i = 0; i < RawSize; i++)
        {
            for (int j = 0; j < ColumnSize; j++)
            {
                if (obj == Wall[i, j])
                {
                    return(i, j);

                }
            }
        }
        Debug.LogError("SearchIndexの添え字が想定を超えている");
        return (0, 0);
    }

    /// <summary>
    /// スプライトの変更、2サイクルバージョン及びアニメションの開始
    /// </summary>
    /// <param name="m_Wall"></param>
    private void ChangeSprite1(GameObject m_Wall, GameObject m_WallAnime)
    {
        string spriteName = m_Wall.GetComponent<SpriteRenderer>().sprite.name;
        Animator animt = m_WallAnime.GetComponent<Animator>();
        if (spriteName == PolutedLevel1)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[2];
            animt.SetTrigger("StartAnime");
        }
        else if (spriteName == PolutedLevel3)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[0];
            animt.SetTrigger("StartAnime");

        }

    }

    /// <summary>
    /// スプライトの変更、3サイクルバージョン
    /// </summary>
    /// <param name="m_Wall"></param>
    private void ChangeSprite2(GameObject m_Wall)
    {
        string spriteName = m_Wall.GetComponent<SpriteRenderer>().sprite.name;
        if (spriteName == PolutedLevel1) {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[1];
        }
        else if(spriteName==PolutedLevel2)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[2];
            
        }else if (spriteName == PolutedLevel3)
        {
            return;
            //m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[0];//サイクルバージョン

        }
        
    }
    /// <summary>
    /// DPバージョン
    /// </summary>
    /// <param name="m_Wall"></param>
    private void ChangeSprite3(GameObject m_Wall)
    {
       
        string spriteName = m_Wall.GetComponent<SpriteRenderer>().sprite.name;

        if (spriteName == PolutedLevel1)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[1];
        }
        else if (spriteName == PolutedLevel2)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[2];
        }
        else if (spriteName == PolutedLevel3)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[3];
        }
        else if (spriteName == PolutedLevel4)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[4];
        }
        else if (spriteName == PolutedLevel5)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[5];
        }
    }
    /// <summary>
    /// スプライトのファイル名を取得
    /// </summary>
    private void GetSpriteName()
    {
        PolutedLevel1 = WallSprite[0].name;
        PolutedLevel2 = WallSprite[1].name;
        PolutedLevel3 = WallSprite[2].name;
        PolutedLevel4 = WallSprite[3].name;
        PolutedLevel5 = WallSprite[4].name;
        PolutedLevel6 = WallSprite[5].name;
    }

    /// <summary>
    /// ゲームを終了するかどうかの判定をとる。
    /// </summary>
    private void CheckGameEnd()
    {
        bool end = false;
        //HPが0ないときに終了する。
        if (gameStatus.life <= 0) end = true;


        //掘る場所がないとき終了する
        bool canDig = false;
        for(int i = 0; i <RawSize;i++)
        {
            for(int j = 0; j < ColumnSize; j++)
            {
                if (Wall[i, j].GetComponent<SpriteRenderer>().sprite.name != PolutedLevel6) canDig = true;
            }
        }

        if (!canDig) end = true;

        if (end) State = GAME_STATE.RESULT;


    }

    IEnumerator ResultGetItem(GameObject res)
    {
        HakaiResultGetItem resultData = res.GetComponent<HakaiResultGetItem>();
        //アニメーションの終了待ち
        while (!resultData.endFadeInAnime)
        {
            yield return null;
        }

        //入力待ち
        while (!Input.anyKeyDown)
        {
            yield return null;
        }
        resultData.animator.SetTrigger("fadeOut");

        //フェードアウトアニメの終了待ち
        while (!resultData.endFadeOutAnime)
        {
            yield return null;
        }

        //削除
        Destroy(res);
        

        yield return 0; 
    }

    IEnumerator CreatGetItem()
    {
        Debug.Log(itemManager.Item.Count);
        foreach (GameObject res in itemManager.Item)
        {
            MinGameHAKAIItem itemData = res.GetComponent<MinGameHAKAIItem>();
            //取得できないアイテムならパス
            if (!itemData.CanGetItem) continue;

            //インベントリーの更新(追加)
            inventory.AddItem(itemData.itemSO);

            GameObject view = Instantiate(Resources.Load("MinGameHakai/ResultGetItem"),resultCanvas.transform) as GameObject;

            HakaiResultGetItem viewData = view.GetComponent<HakaiResultGetItem>();
            viewData.itemIcon.GetComponent<Image>().sprite = itemData.itemSO.icon;
            viewData.itemDiscription.GetComponent<Text>().text = itemData.itemSO.description;
            viewData.itemName.GetComponent<Text>().text = itemData.itemSO.item_name;
            viewData.itemInInventoryNum.GetComponent<Text>().text = "x" + inventory.DataCount(itemData.itemSO).ToString();

            
            yield return StartCoroutine(ResultGetItem(view));

        }

        yield return 0;
    }

    IEnumerator FadeOut(float interval)
    {
        blackImageObj.SetActive(true);

        float time = 0;
       
        Color alpha = blackImage.color;
        
        while (time<=interval)
        {
            alpha.a = Mathf.Lerp(0f, 1f, time / interval);
            time += Time.deltaTime;
            blackImage.color = alpha;
            yield return null;
        }
        yield return 0;
    }

    IEnumerator EndMessageAnime()
    {
        endMessageObj.SetActive(true);

        endMessage.animator.SetTrigger("ShowMessage");

        while (!endMessage.endAnime)
        {
            yield return null;
        }


        yield return 0;
    }

}

