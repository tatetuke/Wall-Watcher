using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class MinGameHakaiManager2 : MonoBehaviour
{
    public const int m_size = 7;//盤面のサイズ
    [HideInInspector]public GameObject[,] Wall = new GameObject[m_size, m_size];//盤面全体
    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };//裏返す壁のIndex
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };//
    private string PolutedLevel1;//壁の画像の名前
    public string PolutedLevel2;//
    [SerializeField] private UnityEvent UpdateItemData=new UnityEvent(); //アイテムデータのアップデート

    [SerializeField] private MinGameHAKAIStatus gameStatus;//HPやHPを減らす関数を持つクラス

    [SerializeField]MinGameHakaiToolDataManager toolManager;
    [SerializeField]MinGameHakaiToolData tool;

    [SerializeField] GameObject shakeObj;//揺らすゲームオブジェクトの選択
    [SerializeField] GameObject lifeGage;//揺らすゲームオブジェクトの選択

    public Sprite []WallSprite=new Sprite[2];//壁の画像

    enum Game_State
    {
        Playing,
        PreStart,
        Pause,
        Result,
        End
    }
    Game_State State;
    private void Start()
    {
        State = Game_State.PreStart;
        WallInit();
        GetSpriteName();
    }
    private void Update()
    {
        ClickProcessing();

    }
    private void MinGameState()
    {

        switch (State)
        {
            case Game_State.PreStart:
                break;
            case Game_State.Playing:
                break;

            case Game_State.Result:
                break;
            case Game_State.Pause:
                break;
            case Game_State.End:
                break;


        }


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
            if (j >= m_size)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = v;
            i++;
            if (i == m_size)
            {
                i = 0;
                j++;
            }

        }


    }

    /// <summary>
    /// クリックしたオブジェクトを取得
    /// </summary>
    private void ClickProcessing()
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

        ////現在のHPよりくらうダメージが大きい場合ゲージを揺らす
        //if(gameStatus.life-(int)tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1] < 0)
        //{
        //    Debug.Log("この道具を使うには体力が足りません");
        //    //画面を揺らす。
        //    iTween.ShakePosition(lifeGage, iTween.Hash("x", 0.3f, "y", 0.3f, "time", 0.5f));
        //    return;
        //}

        Debug.Log(clickedGameObject);
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
            if (nraw < 0 || nraw >= m_size || ncolumn < 0 || ncolumn >= m_size) continue;

            ChangeSprite(Wall[nraw, ncolumn]);
        }

        ReverseSprite();
    }



    /// <summary>
    /// 盤面を反転させるときの処理
    /// </summary>
    private void ReverseSprite()
    {

        // シェイク(一定時間のランダムな動き)
        var duration = 0.7f;    // 時間
        var strength = 0.5f;    // 力
        var vibrato = 100;    // 揺れ度合い
        var randomness = 90f;   // 揺れのランダム度合い(0で一定方向のみの揺れになる)
        var snapping = false; // 値を整数に変換するか
        var fadeOut = true;  // 揺れが終わりに向かうにつれ段々小さくなっていくか(falseだとピタッと止まる)
        shakeObj.transform.DOShakePosition(duration, strength, vibrato, randomness, snapping, fadeOut);
        
        //使用した道具に応じて体力を減らす
        //ToDo
        //レベルが0の時の例外処理（多分いらない）
        gameStatus.Damage(tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1]);

        //取得できるかどうかについてアイテムの情報を更新
        UpdateItemData.Invoke();

    }

    /// <summary>
    ///壁の添え字を探索する。
    /// </summary>
    public (int,int) SearchIndex(GameObject obj)
    {
        for (int i = 0; i < m_size; i++)
        {
            for (int j = 0; j < m_size; j++)
            {
                if (obj == Wall[i, j])
                {
                    return(i, j);

                }
            }
        }
        Debug.LogError("SearchIndexがおかしい");
        return (0, 0);
    }




    /// <summary>
    /// スプライトの変更
    /// </summary>
    /// <param name="m_Wall"></param>
    private void ChangeSprite(GameObject m_Wall)
    {
        string spriteName = m_Wall.GetComponent<SpriteRenderer>().sprite.name;
        if (spriteName == PolutedLevel1) {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[1];
           }
        else if(spriteName==PolutedLevel2)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[0];
            
        }
        

    }
    /// <summary>
    /// スプライトのファイル名を取得
    /// </summary>
    private void GetSpriteName()
    {
        PolutedLevel1=WallSprite[0].name;
        PolutedLevel2=WallSprite[1].name;


    }
}

