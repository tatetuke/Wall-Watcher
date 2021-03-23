using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MinGameHakaiManager2 : MonoBehaviour
{
    public const int m_size = 7;
    [HideInInspector]public GameObject[,] Wall = new GameObject[m_size, m_size];
    
    [SerializeField] private UnityEvent UpdateItemData=new UnityEvent(); //アイテムデータのアップデート

    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
    private string PolutedLevel1;
   [HideInInspector] public string PolutedLevel2;

    public Sprite []WallSprite=new Sprite[2];

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
        if (clickedGameObject.tag != "Wall") return;
        Debug.Log(clickedGameObject);

        int raw = 0, column = 0;

        //対応するタイルの添え字を全探索
        (raw,column)=SearchIndex(clickedGameObject);
        

        for (int i = 0; i < 9; i++)
        {
            int nraw = raw + dy[i];
            int ncolumn = column + dx[i];
            if (nraw < 0 || nraw >= m_size || ncolumn < 0 || ncolumn >= m_size) continue;

            ChangeSprite(Wall[nraw, ncolumn]);
        }
        //アイテムの情報を更新
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

