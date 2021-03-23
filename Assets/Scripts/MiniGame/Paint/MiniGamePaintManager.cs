using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MiniGamePaintManager : MonoBehaviour
{
    public GameObject FrameArrow;
    public GameObject FrameSquare;
    private GameObject NowFrame;

    public int ParamSize = 20;
    public int ConditionCanClick = 8;
    public const int WallLength = 7;
    private int Cost = 0;
    GameObject[,] Wall = new GameObject[WallLength, WallLength];
    public TextMeshProUGUI m_TextDirection;
    public TextMeshProUGUI m_TextCost;
    private string[] Textes = new string[5] { "□", "↑", "→", "↓", "←" };
    Color Brown = new Color(1, 0.7f, 0, 1);
    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
    // MEMO : 誤差が怖いので足し引きでパラメータをいじるのではなく
    //        あらかじめ決められたパラメータに変更するという方針のためのList
    List<float> ParamList = new List<float>();
    private Direction m_Direction = Direction.Square;
    enum Direction
    {
        Square,
        Up,
        Right,
        Down,
        Left
    }


    void Start()
    {
        ListInit();
        WallInit();
        m_TextDirection.text = Textes[(int)m_Direction];
        if (m_Direction == Direction.Square)
            NowFrame = FrameSquare;
        else
            NowFrame = FrameArrow;
    }

    void Update()
    {
        if (/*左クリックが押されたら*/Input.GetMouseButtonDown(0))
        {
            int raw, column;
            (raw, column) = GetClickObjectIndex();
            int add, sub;
            (add, sub) = SetAddSub(m_Direction, raw, column);

            if (CanClick(raw, column, sub))
                UpdateWall(m_Direction, raw, column, add, sub);
        }
        else if (/*右クリックが押されたら*/Input.GetMouseButtonDown(1))
        {
            int raw, column;
            (raw, column) = GetClickObjectIndex();
             FillSoil(raw, column);
        }
        else if (/*中クリックが押されたら*/Input.GetMouseButtonDown(2))
        {
            ChangeDirection();
        }
        else
        {
            GameObject clickedGameObject;
            clickedGameObject = null;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
            if (hit2d)
            {
                clickedGameObject = hit2d.transform.gameObject;
            }
            if (clickedGameObject != null)
            {
                int raw, column;
                (raw, column) = GetClickObjectIndex();
                int add, sub;
                (add, sub) = SetAddSub(m_Direction, raw, column);

                if (CanClick(raw, column, sub))
                {
                    NowFrame.SetActive(true);
                    if (m_Direction == Direction.Square)
                    {
                        NowFrame.transform.position = Wall[raw, column].transform.position;
                    }
                    else
                    {
                        int angleZ = 90 * (2 - (int)m_Direction);
                        Quaternion quaternion = Quaternion.Euler(0, 0, angleZ);
                        NowFrame.transform.position = Wall[raw, column].transform.position;
                        NowFrame.transform.rotation = quaternion;
                    }
                }
                else
                {
                    NowFrame.SetActive(false);
                }
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
        int raw = -100, column = -100;

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
        //Debug.Log($"{raw}, {column}");
        return (raw, column);
    }

    private (int, int) SetAddSub(Direction direction, int raw, int column)
    {
        if (raw < 0 || column < 0) return (-100, -100);

        int add, sub;
        if (direction == Direction.Square)
        {
            if (IsBrown(raw, column))
            {
                add = +3; sub = 0;
            }
            else
            {
                add = +1;
                if ((raw == 0 && (column == 0 || column == WallLength - 1))  // 角
                   || ((raw == WallLength - 1) && (column == 0 || column == WallLength - 1)))
                {
                    sub = -3;
                }
                else if (raw == 0 || raw == WallLength - 1 || column == 0 || column == WallLength - 1)// 端
                {
                    sub = -5;
                }
                else
                    sub = -8;
            }
        }
        else
        {
            if (IsBrown(raw, column))
            {
                add = +4; sub = 0;
            }
            else
            {
                add = 1; sub = -add * 6;
            }
        }
        return (add, sub);
    }

    private bool IsBrown(int raw, int column)
    {
        SpriteRenderer spriteRenderer = Wall[raw, column].GetComponent<SpriteRenderer>();
        return spriteRenderer.color == Brown;
    }

    private bool CanClick(int raw, int column, int sub)
    {
        if (raw < 0 || column < 0) return false;

        return IsEnoughCost(raw, column, sub) && !IsOutOfFrame(m_Direction, raw, column);
    }

    bool IsEnoughCost(int raw, int column, int sub)
    {
        float alpha = Wall[raw, column].GetComponent<SpriteRenderer>().color.a;
        int Index = ParamList.IndexOf(alpha);
        if (ParamList.IndexOf(alpha) == -1) Debug.Log("見つかりませんでした");
        bool res = false;
        res |= (Wall[raw, column].GetComponent<SpriteRenderer>().color == Brown);
        res |= (Index + sub >= 0);
        return res;
    }

    bool IsOutOfFrame(Direction direction, int raw, int column)
    {
        if (direction == Direction.Square)
        {
            for (int i = 0; i < 9; i++)
            {
                if (dx[i] == 0 && dy[i] == 0) continue;
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength)
                    return true;
            }
            return false;
        }
        else
        {
            int draw, dcolumn;
            (draw, dcolumn) = SetItr(m_Direction);
            int nraw = raw;
            int ncolumn = column;
            for (int i = 0; i < 3; i++)
            {
                nraw += draw; ncolumn += dcolumn;
                if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength)
                    return true;
            }
            return false;
        }
    }

    private void UpdateWall(Direction direction,int raw, int column,int add, int sub)
    {
        if (direction == Direction.Square)
        {
            for (int i = 0; i < 9; i++)
            {
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength) continue;
                if (dx[i] == 0 && dy[i] == 0) ChangeSprite(Wall[nraw, ncolumn], sub);
                else ChangeSprite(Wall[nraw, ncolumn], add);
            }
        }
        else
        {
            int draw, dcolumn;
            (draw, dcolumn) = SetItr(m_Direction);
            ChangeSprite(Wall[raw, column], sub);
            ChangeSprite(Wall[raw + draw, column + dcolumn], 2 * add);
            ChangeSprite(Wall[raw + 2 * draw, column + 2 * dcolumn], add);
            ChangeSprite(Wall[raw + 3 * draw, column + 3 * dcolumn], add);
        }
    }

    private (int, int) SetItr(Direction direction)
    {
        if (direction == Direction.Up)
            return (0, -1);
        else if (direction == Direction.Right)
            return (+1, 0);
        else if (direction == Direction.Down)
            return (0, +1);
        else /*if (direction == Direction.Left)*/
            return (-1, 0);
    }

    private void ChangeSprite(GameObject gameObject, int changeAmount)
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer.color == Brown)
        {
            if (changeAmount <= 0)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
            }
        }
        else
        {
            float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
            if (ParamList.IndexOf(alpha) == -1) Debug.Log("見つかりませんでした");
            int nextIndex = ParamList.IndexOf(alpha) + changeAmount;
            if (nextIndex < 0) nextIndex = 0;
            if (nextIndex >= ParamSize) nextIndex = ParamSize - 1;
            spriteRenderer.color = new Color(1, 1, 1, ParamList[nextIndex]);
        }
    }

    private void FillSoil(int raw, int column)
    {
        if (raw < 0 || column < 0) return;
        ChangeCost(24);
        float alpha = Wall[raw, column].GetComponent<SpriteRenderer>().color.a;
        int Index = ParamList.IndexOf(alpha);
        if (ParamList.IndexOf(alpha) == -1) Debug.Log("見つかりませんでした");
        ChangeCost(ParamSize - 1 - Index);
        SpriteRenderer spriteRenderer = Wall[raw, column].GetComponent<SpriteRenderer>();
        spriteRenderer.color = Brown;
    }

    private void ChangeCost(int num)
    {
        Cost += num;
        m_TextCost.text = Cost.ToString();
    }

    void ChangeDirection()
    {
        NowFrame.SetActive(false);
        if (m_Direction == Direction.Square)
            NowFrame = FrameArrow;
        else if (m_Direction == Direction.Left)
            NowFrame = FrameSquare;

        m_Direction = (Direction)(((int)m_Direction + 1) % 5);
        m_TextDirection.text = Textes[(int)m_Direction];
    }
}