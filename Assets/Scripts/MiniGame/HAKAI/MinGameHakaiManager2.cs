using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
public class MinGameHakaiManager2 : MonoBehaviour
{
    public const int RawSize = 7;//盤面のサイズ
    public const int ColumnSize = 7;
    [HideInInspector]public int Rsize=RawSize; 
    [HideInInspector]public int Csize=ColumnSize; 
    [HideInInspector]public GameObject[,] Wall = new GameObject[RawSize, ColumnSize];//盤面全体
    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };//裏返す壁のIndex
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };//
    private string PolutedLevel1;//壁の画像の名前
    private string PolutedLevel1_1;//
    public string PolutedLevel2;//
    [SerializeField] private UnityEvent UpdateItemData=new UnityEvent(); //アイテムデータのアップデート

    [SerializeField] private MinGameHAKAIStatus gameStatus;//HPやHPを減らす関数を持つクラス

    [SerializeField]MinGameHakaiToolDataManager toolManager;
    [SerializeField]MinGameHakaiToolData tool;

    [SerializeField] GameObject shakeObj;//揺らすゲームオブジェクトの選択
    [SerializeField] GameObject lifeGage;//揺らすゲームオブジェクトの選択

    public Sprite []WallSprite=new Sprite[2];//壁の画像

    [SerializeField] private MinGameHakaiItemGetUI ItemGetUI;//UIのアイテム欄を更新する.

    //UIがシェイク時にぶれるバグを修正仕様とした跡地
    //private Vector3 initShakeObj;
    //private Vector3 initLifeGage;
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
        //取得できるかどうかについてアイテムの情報を更新
        UpdateItemData.Invoke();
        //UIのアイテム情報の更新
        ItemGetUI.ChangeGetItemUI();
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
            if (j >= RawSize)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = v;
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

        //現在のHPよりくらうダメージが大きい場合ゲージを揺らしてreturn 
        if (gameStatus.life - (int)tool.Tools[toolManager.SelectToolNum].damage[tool.Tools[toolManager.SelectToolNum].level - 1] < 0)
        {
            AttackErrorEffect();
            return;
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

            ChangeSprite(Wall[nraw, ncolumn]);
        }
        //道具を使用できないときのエフェクトを出す。
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

    //道具を使用できないときのエフェクト
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
    /// 盤面を反転させるときのエフェクト
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
    /// スプライトの変更
    /// </summary>
    /// <param name="m_Wall"></param>
    private void ChangeSprite(GameObject m_Wall)
    {
        string spriteName = m_Wall.GetComponent<SpriteRenderer>().sprite.name;
        if (spriteName == PolutedLevel1) {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[1];
        }
        else if(spriteName==PolutedLevel1_1)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[2];
            
        }else if (spriteName == PolutedLevel2)
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
        PolutedLevel1_1 = WallSprite[1].name;
        PolutedLevel2=WallSprite[2].name;


    }
}

