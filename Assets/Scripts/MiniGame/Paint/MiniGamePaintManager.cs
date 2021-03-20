using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamePaintManager : MonoBehaviour
{
    public const int m_Size = 10;
    GameObject[,] Wall = new GameObject[m_Size, m_Size];

    int[] dx = new int[9] { -1, 0, 1, -1, 0, 1, -1, 0, 1 };
    int[] dy = new int[9] { -1, -1, -1, 0, 0, 0, 1, 1, 1 };

    void Start()
    {
        WallInit();
    }


    void Update()
    {
        ClickProcessing();
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
            if (j >= m_Size)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = v;
            i++;
            if (i == m_Size)
            {
                i = 0;
                j++;
            }
        }
    }

    private bool CanDigAround(int raw, int column)
    {
        return false;
    }

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

        int raw = 0, column = 0;

        // 今見ている壁の場所を取得
        for (int i = 0; i < m_Size; i++)
        {
            for (int j = 0; j < m_Size; j++)
            {
                if (clickedGameObject == Wall[i, j])
                {
                    raw = i;
                    column = j;
                }
            }
        }

        if (CanDigAround(raw, column))
        {
            // 壁の厚さを更新する処理を書く
            for (int i = 0; i < 9; i++)
            {
                int nraw = raw + dy[i];
                int ncolumn = column + dx[i];
                if (nraw < 0 || nraw >= m_Size || ncolumn < 0 || ncolumn >= m_Size) continue;
                ChangeSprite(Wall[nraw, ncolumn]);
            }

        }
        else
        {

            Debug.Log("掘れません");
        }

    }
    private void ChangeSprite(GameObject m_Wall)
    {

    }

}