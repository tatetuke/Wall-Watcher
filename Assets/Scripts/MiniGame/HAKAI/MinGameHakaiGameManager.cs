using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinGameHakaiGameManager : MonoBehaviour
{
    public const int row_size = 11;
    public const int col_size = 8;
    GameObject[,] Wall = new GameObject[row_size, col_size];

    int[] dx = new int[9] { -1,0,1,-1,0,1,-1,0,1};
    int[] dy = new int[9] { -1,-1,-1,0,0,0,1,1,1};


    public string PollutedLevel1;
    public string PollutedLevel2;
    public string PollutedLevel3;
    public Sprite[] WallSprite;
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
            if (j >= col_size)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = v;
            i++;
            if (i == row_size)
            {
                i = 0;
                j++;
            }

        }


    }
    /// <summary>
    /// 周りの壁が
    /// </summary>
    private bool CanDigAround(int raw, int column)
    {

        for(int i = 0; i < 9; i++)
        {
            int nraw = raw + dy[i];
            int ncolumn = column + dx[i];
            if (nraw < 0 || nraw >= row_size || ncolumn < 0 || ncolumn >= col_size) continue;
            //ほりたい壁が汚染された壁だった時
            if (Wall[nraw, ncolumn].GetComponent<SpriteRenderer>().sprite.name== PollutedLevel3) return false;
               
        }
        return true;

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

        if (clickedGameObject == null) return;
        Debug.Log(clickedGameObject);

        int raw=0, column=0;
     
         for (int i = 0; i < row_size; i++)
         {
            for (int j = 0; j < col_size; j++)
            {
                if (clickedGameObject == Wall[i, j])
                {
                    raw = i;
                    column = j;
                  
                }
            }
         }
        if(CanDigAround(raw, column))
        {

            for (int i = 0; i < 9; i++)
            {
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= row_size || ncolumn < 0 || ncolumn >= col_size) continue;

                ChangeSprite(Wall[nraw, ncolumn]);
            }

        }
        else
        {

            Debug.Log("掘れません");
        }

    }
    private void ChangeSprite( GameObject m_Wall)
    {
        string wallName = m_Wall.GetComponent<SpriteRenderer>().sprite.name;
        if (wallName == PollutedLevel1)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[1];
        }
        else if (wallName == PollutedLevel2)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[0];
        }
        else if (wallName == PollutedLevel3)
        {
            m_Wall.GetComponent<SpriteRenderer>().sprite = WallSprite[2];
        }

    }

}
