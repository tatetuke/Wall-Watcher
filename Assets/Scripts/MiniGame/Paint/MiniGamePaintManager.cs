using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Playables;

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

    [SerializeField] Sprite[] WallSprites;

    [SerializeField] GameObject FrameArrow;
    [SerializeField] GameObject FrameSquare;
    private GameObject NowFrame;
    [SerializeField] GameObject ParameterCollection;

    [SerializeField] private PlayableDirector playableDirector;

    [SerializeField] public Button FinishTaskButton;
    [SerializeField] TextMeshProUGUI HPValue;
    [SerializeField] TextMeshProUGUI SatisfactionValue;
    [SerializeField] TextMeshProUGUI EarnRewardValue;

    public int ParamSize = 15;
    public int ConditionCanClick = 8;
    public const int WallLength = 7;
    private int Cost = 0;
    GameObject[,] Wall = new GameObject[WallLength, WallLength];
    int[,] WallParam = new int[WallLength, WallLength];
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

    public enum State
    {
        Tutorial,
        Playing,
        Finished
    }
    State m_State = State.Tutorial;

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
        if (m_State == State.Playing)
        {
            UpdateParameters();
            if (/*左クリックが押されたら*/Input.GetMouseButtonDown(0))
            {
                int raw, column;
                (raw, column) = GetCursorObjectIndex();
                int add, sub;
                (add, sub) = SetAddSub(m_Range, raw, column);
                if (CanClick(raw, column, sub))
                    UpdateWall(m_Range, raw, column, add, sub);
            }
            else if (/*右クリックが押されたら*/Input.GetMouseButtonDown(1))
            {
                int raw, column;
                (raw, column) = GetCursorObjectIndex();
                if (!(raw < 0 || column < 0))
                {
                    int damage = 14 + 24 - WallParam[raw, column];
                    if (gameStatus.life >= damage && !IsBrown(raw, column))
                    {
                        gameStatus.Damage(40);
                        FillSoil(raw, column);
                    }
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

            for (int i = 0; i < WallLength; i++)
            {
                for (int j = 0; j < WallLength; j++)
                {
                    if (IsBrown(i, j))
                    {
                        if (Random.value <= 0.05)
                        {
                            float x = (Wall[i, j].transform.position.x + Random.Range(-0.5f, 0.5f));
                            float y = (Wall[i, j].transform.position.y + Random.Range(-0.5f, 0.5f));
                            Particle2.Add(x, y);
                        }
                    }
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
            Wall[i, j] = v;
            SpriteRenderer spriteRenderer = Wall[i, j].GetComponent<SpriteRenderer>();
            int rndm = Random.Range(0, ParamSize - 1);
            if (m_State == State.Tutorial) rndm = 0;
            WallParam[i, j] = rndm;
            // 内部のパラメータは[0,14]，スプライトは5段階
            spriteRenderer.sprite = WallSprites[rndm / 3];
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
            ParameterText[i, j].text = WallParam[i, j].ToString();
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
                    param = 14 + 24;
                else
                    param = WallParam[i, j];
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

        return IsEnoughCost(raw, column, sub) && !IsOutOfFrame(m_Range, raw, column);
    }

    bool IsEnoughCost(int raw, int column, int sub)
    {
        //float alpha = Wall[raw, column].GetComponent<SpriteRenderer>().color.a;
        //int Index = ParamList.IndexOf(alpha);
        //if (ParamList.IndexOf(alpha) == -1) Debug.Log("見つかりませんでした");
        //bool res = false;
        //res |= (Wall[raw, column].GetComponent<SpriteRenderer>().color == Brown);
        //res |= (Index + sub >= 0);
        //return res;
        return WallParam[raw, column] + sub >= 0;
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


    private void UpdateWall(Range direction, int raw, int column, int add, int sub)
    {
        if (direction == Range.Square)
        {
            for (int i = 0; i < 9; i++)
            {
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= WallLength || ncolumn < 0 || ncolumn >= WallLength) continue;
                if (dx[i] == 0 && dy[i] == 0) ChangeSprite(nraw, ncolumn, sub);
                else ChangeSprite(nraw, ncolumn, add);
                for (int j = 0; j < 5; j++)
                {
                    Particle.Add(Wall[nraw, ncolumn].transform.position.x, Wall[nraw, ncolumn].transform.position.y);
                }
            }
        }
        else
        {
            int draw, dcolumn;
            (draw, dcolumn) = SetItr(m_Range);
            ChangeSprite(raw, column, sub);
            ChangeSprite(raw + 1 * draw, column + 1 * dcolumn, 3 * add);
            for (int j = 0; j < 7; j++)
                Particle.Add(Wall[raw + 1 * draw, column + 1 * dcolumn].transform.position.x, Wall[raw + 1 * draw, column + 1 * dcolumn].transform.position.y);
            ChangeSprite(raw + 2 * draw, column + 2 * dcolumn, 2 * add);
            for (int j = 0; j < 7; j++)
                Particle.Add(Wall[raw + 2 * draw, column + 2 * dcolumn].transform.position.x, Wall[raw + 2 * draw, column + 2 * dcolumn].transform.position.y);
            ChangeSprite(raw + 3 * draw, column + 3 * dcolumn, 1 * add);
            for (int j = 0; j < 7; j++)
                Particle.Add(Wall[raw + 3 * draw, column + 3 * dcolumn].transform.position.x, Wall[raw + 3 * draw, column + 3 * dcolumn].transform.position.y);

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

    private void ChangeSprite(int raw, int column, int changeAmount)
    {

        SpriteRenderer spriteRenderer = Wall[raw, column].GetComponent<SpriteRenderer>();
        if (spriteRenderer.color == Brown)
        {
            if (changeAmount <= 0)
            {
                spriteRenderer.color = new Color(1, 1, 1, 1);
                WallParam[raw, column] = ParamSize - 1;
            }
        }
        else
        {
            //float alpha = gameObject.GetComponent<SpriteRenderer>().color.a;
            //if (ParamList.IndexOf(alpha) == -1) Debug.Log("見つかりませんでした");
            //int nextIndex = ParamList.IndexOf(alpha) + changeAmount;
            //spriteRenderer.color = new Color(1, 1, 1, ParamList[nextIndex]);

            WallParam[raw, column] = WallParam[raw, column] + changeAmount;
            if (WallParam[raw, column] < 0) WallParam[raw, column] = 0;
            if (WallParam[raw, column] >= ParamSize) WallParam[raw, column] = ParamSize - 1;
            spriteRenderer.sprite = WallSprites[WallParam[raw, column] / 3];
        }
    }

    private void FillSoil(int raw, int column)
    {
        if (raw < 0 || column < 0) return;
        if (IsBrown(raw, column)) return;
        //ChangeCost(24);
        //float alpha = Wall[raw, column].GetComponent<SpriteRenderer>().color.a;
        //int Index = ParamList.IndexOf(alpha);
        //if (ParamList.IndexOf(alpha) == -1) Debug.Log("見つかりませんでした");
        //ChangeCost(ParamSize - 1 - Index);
        SpriteRenderer spriteRenderer = Wall[raw, column].GetComponent<SpriteRenderer>();
        spriteRenderer.color = Brown;
        WallParam[raw, column] = 14 + 24;
        spriteRenderer.sprite = WallSprites[(ParamSize - 1) / 3];
        for (int i = 0; i < 32; i++)
        {
            Particle.Add(Wall[raw, column].transform.position.x, Wall[raw, column].transform.position.y);
        }
    }

    private void ChangeCost(int num)
    {
        Cost += num;
        m_TextCost.text = Cost.ToString();
    }

    void ChangeRange()
    {
        //if (m_Range == Range.Up) m_Range = Range.Right;
        //else if (m_Range == Range.Right) m_Range = Range.Down;
        //else if (m_Range == Range.Down) m_Range = Range.Left;
        //else if (m_Range == Range.Left) m_Range = Range.Up;
        //NowFrame.SetActive(false);
        //if (m_Range == Range.Square)
        //    NowFrame = FrameArrow;
        //else if (m_Range == Range.Left)
        //    NowFrame = FrameSquare;

        NowFrame.SetActive(false);
        m_Range = (Range)(((int)m_Range + 1) % 5);
        if (m_Range == Range.Square)
            NowFrame = FrameSquare;
        else
            NowFrame = FrameArrow;
        m_TextRange.text = Textes[(int)m_Range];
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

    public void ChangeState(State state)
    {
        m_State = state;
    }



    // スコア計算は仮
    private string ValHP()
    {
        string res;
        // 最初のライフ : gameStatus.maxLife
        if (gameStatus.life >= gameStatus.maxLife / 2)
            res = "S";
        else if (gameStatus.life >= gameStatus.maxLife / 3)
            res = "A";
        else if (gameStatus.life >= gameStatus.maxLife / 5)
            res = "B";
        else
            res = "C";

        return res;
    }

    private string ValSatisfaction()
    {
        string res;
        int diff = 0;
        for (int i = 0; i < WallLength; i++)
            for (int j = 0; j < WallLength; j++)
                diff += (WallParam[i, j] - (ParamSize - 1)) / 3;

        if (diff == 0)
            res = "S";
        else if (diff > 10)
            res = "A";
        else if (diff > 30)
            res = "B";
        else
            res = "C";

        return res;
    }

    public void OnClick()
    {
        if (m_State == State.Playing)
        {
            ChangeState(State.Finished);
            SatisfactionValue.text = ValSatisfaction();
            HPValue.text = ValHP();
            FinishTaskButton.interactable = false;
            playableDirector.Play();
        }
    }
}