using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// ミニゲームの塗るパートを統括する
/// クリック時の土・コストの更新や、クリックする前に範囲を指し示す
/// </summary>
public class MiniGamePaintManager : SingletonMonoBehaviour<MiniGamePaintManager>
{
    [SerializeField] private MiniGamePaintStatus gameStatus;//HPやHPを減らす関数を持つクラス

    [SerializeField] MiniGamePaintToolDataManager toolManager;
    [SerializeField] MiniGamePaintToolData tool;

    [SerializeField] GameObject lifeGage;//揺らすゲームオブジェクトの選択

    [SerializeField] GameObject FrameArrow;
    [SerializeField] GameObject FrameSquare;
    private GameObject NowFrame;
    [SerializeField] GameObject ParameterCollection;

    [SerializeField] Sprite[] WallSprites;

    public int ParamSize = 20;
    public int ConditionCanClick = 8;
    public const int WallLength = 7;
    private int Cost = 0;
    GameObject[,] Wall = new GameObject[WallLength, WallLength];
    TextMeshProUGUI[,] ParameterText = new TextMeshProUGUI[WallLength, WallLength];
    public TextMeshProUGUI m_TextRange;
    public TextMeshProUGUI m_TextCost;
    private string[] Textes = new string[5] { "□", "↑", "→", "↓", "←" };
    Color Brown = new Color(1, 0.7f, 0, 1);
    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };
    // MEMO : 誤差が怖いので足し引きでパラメータをいじるのではなく
    //        あらかじめ決められたパラメータに変更するという方針のためのList
    List<float> ParamList = new List<float>();
    public Range m_Range = Range.Square;

    public enum Range
    {
        Square,
        Up,
        Right,
        Down,
        Left,
    }


    void Start()
    {
        ListInit();
        WallInit();
        ParameterTextInit();
        m_TextRange.text = Textes[(int)m_Range];
        if (m_Range == Range.Square)
            NowFrame = FrameSquare;
        else
            NowFrame = FrameArrow;
    }

    void Update()
    {
        Debug.Log(m_Range);
        //UpdateParameters();
        if (/*左クリックが押されたら*/Input.GetMouseButtonDown(0))
        {
            int raw, column;
            (raw, column) = GetCursorObjectIndex();
            Debug.Log(raw);
            Debug.Log(column); 
            int add, sub;
            (add, sub) = SetAddSub(m_Range, raw, column);
            if (CanClick(raw, column, sub))
                UpdateWall(m_Range, raw, column, add, sub);
        }
        else if (/*右クリックが押されたら*/Input.GetMouseButtonDown(1))
        {
            int raw, column;
            (raw, column) = GetCursorObjectIndex();
            if (gameStatus.life >= 40)
            {
                gameStatus.Damage(40);
                FillSoil(raw, column);
            }
        }
        else if (/*中クリックが押されたら*/Input.GetMouseButtonDown(2))
        {
            ChangeRange();
        }
        else  // クリックが押されていなかったら
        {
            int raw, column;
            (raw, column) = GetCursorObjectIndex();
            int sub;
            (_, sub) = SetAddSub(m_Range, raw, column);
            if (CanClick(raw, column, sub))
                DisplayRangeFrame(raw, column);
            else
                HideRangeFrame();
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
            Wall[i, j] = v;
            SpriteRenderer spriteRenderer = Wall[i, j].GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = WallSprites[Random.Range(0, 7)];
            //spriteRenderer.color = new Color(1, 1, 1, ParamList[Random.Range(0, ParamSize - 1)]);
            i++;
            if (i == WallLength)
            {
                i = 0;
                j++;
            }
        }
    }

    private void ParameterTextInit()
    {
        ParameterCollection.SetActive(true);
        int i = 0, j = 0;
        foreach (GameObject v in GameObject.FindGameObjectsWithTag("WallParameter"))
        {
            ParameterText[i, j] = v.GetComponent<TextMeshProUGUI>();
            i++;
            if (i == WallLength)
            {
                i = 0;
                j++;
            }
        }
        ParameterCollection.SetActive(false);
    }

    private void UpdateParameters()
    {
        for (int i = 0; i < WallLength; i++)
        {
            for (int j = 0; j < WallLength; j++)
            {
                int param;
                if (IsBrown(i, j))
                    param = 16 + 24;
                else
                {
                    float alpha = Wall[i, j].GetComponent<SpriteRenderer>().color.a;
                    param = ParamList.IndexOf(alpha);
                }
                ParameterText[i, j].text = param.ToString();
            }
        }
    }

    private GameObject GetCursorObject()
    {
        GameObject cursorObject;
        cursorObject = null;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2d = Physics2D.Raycast((Vector2)ray.origin, (Vector2)ray.direction);
        if (hit2d)
        {
            cursorObject = hit2d.transform.gameObject;
        }
        return cursorObject;
    }

    // Wallの添え字を返す
    private (int, int) GetCursorObjectIndex()
    {
        GameObject cursorObject = GetCursorObject();
        if (cursorObject == null) return (-1, -1);
        int raw = -100, column = -100;
        for (int i = 0; i < WallLength; i++)
        {
            for (int j = 0; j < WallLength; j++)
            {
                if (cursorObject == Wall[i, j])
                {
                    raw = i;
                    column = j;
                }
            }
        }
        //Debug.Log($"{raw}, {column}");
        return (raw, column);
    }
    
    // 土の変化量を設定する
    private (int, int) SetAddSub(Range direction, int raw, int column)
    {
        if (raw < 0 || column < 0) return (-100, -100);

        int add, sub;
        if (direction == Range.Square)
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
        if (raw < 0 || raw >= WallLength || column < 0 || column >= WallLength) return false;

        return true;
        //return IsEnoughCost(raw, column, sub) && !IsOutOfFrame(m_Range, raw, column);
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

    bool IsOutOfFrame(Range direction, int raw, int column)
    {
        if (direction == Range.Square)
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
            (draw, dcolumn) = SetItr(m_Range);
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

    private int GetWallSpriteItr(Sprite sprite,int raw, int column)
    {
        for (int i = 0; i < 7; i++)
            if (WallSprites[i] == sprite)
                return i;

        return -1;
    }

    void ChangeSprite(int raw,int column,int add)
    {
        SpriteRenderer spriteRenderer = Wall[raw, column].GetComponent<SpriteRenderer>();
        Sprite sprite = spriteRenderer.sprite;
        int wallSpriteItr = GetWallSpriteItr(sprite, raw, column);
        int ni = wallSpriteItr + add;
        if (ni < 0) ni = 0;
        if (ni >= 7) ni = 6;
        sprite = WallSprites[ni];
    }

    private void UpdateWall(Range direction,int raw, int column,int add, int sub)
    {
        if (direction == Range.Square)
        {
            Debug.Log("Update!");
            for(int i = 0; i < 9; i++)
            {
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength) continue;
                if (raw == nraw && column == ncolumn)
                    ChangeSprite(raw, column, 10);
                else
                    ChangeSprite(raw, column, 1);
            }

            //for (int i = 0; i < 9; i++)
            //{
            //    int nraw = raw + dy[i];
            //    int ncolumn = column + dx[i];
            //    if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength) continue;
            //    if (dx[i] == 0 && dy[i] == 0) ChangeSprite(Wall[nraw, ncolumn], sub);
            //    else ChangeSprite(Wall[nraw, ncolumn], add);
            //}
        }
        else
        {
            int draw, dcolumn;
            (draw, dcolumn) = SetItr(m_Range);
            ChangeSprite(Wall[raw, column], sub);
            ChangeSprite(Wall[raw + 1 * draw, column + 1 * dcolumn], 3 * add);
            ChangeSprite(Wall[raw + 2 * draw, column + 2 * dcolumn], 2 * add);
            ChangeSprite(Wall[raw + 3 * draw, column + 3 * dcolumn], 1 * add);
        }
    }

    // 範囲が矢印の時の変化量を設定する
    private (int, int) SetItr(Range direction)
    {
        if (direction == Range.Up)
            return (0, -1);
        else if (direction == Range.Right)
            return (+1, 0);
        else if (direction == Range.Down)
            return (0, +1);
        else /*if (direction == Range.Left)*/
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
        if (IsBrown(raw, column)) return;
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

    void ChangeRange()
    {
        if (m_Range == Range.Up) m_Range = Range.Right;
        else if (m_Range == Range.Right) m_Range = Range.Down;
        else if (m_Range == Range.Down) m_Range = Range.Left;
        else if (m_Range == Range.Left) m_Range = Range.Up;
        //NowFrame.SetActive(false);
        //if (m_Range == Range.Square)
        //    NowFrame = FrameArrow;
        //else if (m_Range == Range.Left)
        //    NowFrame = FrameSquare;

        //m_Range = (Range)(((int)m_Range + 1) % 5);
        //m_TextRange.text = Textes[(int)m_Range];
    }

    private void DisplayRangeFrame(int raw, int column)
    {
        NowFrame.SetActive(true);
        if (m_Range == Range.Square)
        {
            NowFrame.transform.position = Wall[raw, column].transform.position;
        }
        else
        {
            int rotationZ = 90 * (2 - (int)m_Range);
            Quaternion quaternion = Quaternion.Euler(0, 0, rotationZ);
            NowFrame.transform.position = Wall[raw, column].transform.position;
            NowFrame.transform.rotation = quaternion;
        }
    }

    private void HideRangeFrame()
    {
        NowFrame.SetActive(false);
    }

    // ボタンが押されたらこの関数が実行される
    public void DisplayParameter()
    {
        if (ParameterCollection.activeSelf)
            ParameterCollection.SetActive(false);
        else
            ParameterCollection.SetActive(true);
    }

    public void ChangeRange(Range range)
    {
        NowFrame.SetActive(false);
        m_Range = range;
        if (m_Range == Range.Square)
            NowFrame = FrameSquare;
        else
            NowFrame = FrameArrow;
    }
}