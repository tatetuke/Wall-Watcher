using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 中ボタンを押すと、上下左右を変更できるようにする
/// 左クリックしたら3マス分変化させる
/// </summary>
public class MiniGamePaintManager : MonoBehaviour
{
    public int ParamSize = 20;
    public int ConditionCanClick = 8;
    public const int WallLength = 7;
    GameObject[,] Wall = new GameObject[WallLength, WallLength];

    // MEMO : 誤差が怖いので足し引きでパラメータをいじるのではなく
    //        あらかじめ決められたパラメータに変更するという方針のためのList
    List<float> ParamList = new List<float>();

    enum Direction
    {
        Up,
        Right,
        Down,
        Left
    }

    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };

    void Start()
    {
        ListInit();
        WallInit();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int raw, column;
            (raw, column) = GetClickObjectIndex();
            if (raw >= 0 && column >= 0)
            {
                // 壁の厚さを更新する処理を書く
                for (int i = 0; i < 9; i++)
                {
                    int nraw = raw + dy[i];
                    int ncolumn = column + dx[i];
                    if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength) continue;
                    if (dx[i] == 0 && dy[i] == 0) ChangeSprite(Wall[nraw, ncolumn], -ConditionCanClick);
                    else ChangeSprite(Wall[nraw, ncolumn], +1);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            int raw, column;
            (raw, column) = GetClickObjectIndex();
            if (raw >= 0 && column >= 0)
                ChangeSprite(Wall[raw, column], +8);
        }


    }

    /// <summary>
    /// Wallを初期化する関数。
    /// タグがWallであるゲームオブジェクトをすべて取得する。
    /// </summary>
    private void WallInit()
    {
        int i = 0, j = 0;
        foreach (GameObject v in GameObject.FindGameObjectsWithTag("Wall"))
        {
            if (j >= WallLength)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = v;
            SpriteRenderer spriteRenderer = Wall[i, j].GetComponent<SpriteRenderer>();
            spriteRenderer.color = new Color(1, 1, 1, ParamList[Random.Range(0, ParamSize - 1)]);
            i++;
            if (i == WallLength)
            {
                i = 0;
                j++;
            }
        }
    }

    void ListInit()
    {
        float param = 0;
        for (int i = 0; i < ParamSize; i++)
        {
            ParamList.Add(param);
            param += (float)1 / (ParamSize - 1);
        }
    }


    private void ClickProcessing()
    {
        int raw, column;
        (raw, column) = GetClickObjectIndex();

        if (CanDigAround(Wall[raw, column]))
        {
            //ChangeSprite(Wall[raw, column]);

            // 壁の厚さを更新する処理を書く
            for (int i = 0; i < 9; i++)
            {
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength) continue;
                if (dx[i] == 0 && dy[i] == 0) ChangeSprite(Wall[nraw, ncolumn], -3);
                else ChangeSprite(Wall[nraw, ncolumn], +1);
            }

        }
        else
        {

            Debug.Log("掘れません");
        }

    }

    private bool CanDigAround(GameObject gameObject)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
        int Index = ParamList.IndexOf(alpha);
        return Index >= ConditionCanClick;
    }

    private void ChangeSprite(GameObject gameObject, int changeAmount)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
        int nextIndex = ParamList.IndexOf(alpha) + changeAmount;
        if (nextIndex < 0) nextIndex = 0;
        if (nextIndex >= ParamSize) nextIndex = ParamSize - 1;
        spriteRenderer.color = new Color(1, 1, 1, ParamList[nextIndex]);
    }


    private (int, int) GetClickObjectIndex()
    {
        GameObject clickedGameObject;
        clickedGameObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        if (hit2d)
        {
            clickedGameObject = hit2d.transform.gameObject;
        }
        if (clickedGameObject == null) return (-1, -1);
        int raw = -1, column = -1;

        // 今見ている壁の場所を取得
        for (int i = 0; i < WallLength; i++)
        {
            for (int j = 0; j < WallLength; j++)
            {
                if (clickedGameObject == Wall[i, j])
                {
                    raw = i;
                    column = j;
                }
            }
        }
        return (raw, column);
    }
}