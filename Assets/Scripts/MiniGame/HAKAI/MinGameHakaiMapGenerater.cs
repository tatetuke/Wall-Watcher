using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinGameHakaiMapGenerater : MonoBehaviour
{
    public const int Rsize=7;
    public const int Csize=7;
    public bool[,] CanSetItem = new bool[Rsize,Csize ];
    public int RandomRageLengthWall=1000;
    public int RandomRageLengthItem=1000;
    public int itemcount=4;
    public GameObject[,] Wall = new GameObject[Rsize, Csize];
    public List<GameObject> Items;
    public List<MinGameHAKAIItem> ItemData;
    /// <summary>
    /// 確率の低い順に並べる必要があります。
    /// </summary>
    public List<MinGameHakaiMapTileData>tile;
    private void Start()
    {
        for (int i = 0; i < Rsize; i++)
        {
            for (int j = 0; j < Csize; j++)
            {
                CanSetItem[i, j] = new bool();
                CanSetItem[i, j] = true;
            }
        }
        //アイテムデータの初期化
        InitItemData();
        //Debug.Log("HI");
        //最後タイルを選ぶ確率が100%になるようにする。
        tile[tile.Count - 1].P = RandomRageLengthWall;
        //壁の生成
        generateWall();
        //アイテムの生成
        generateItem();
         
    }


    /// <summary>
    /// ItemDataの取得
    /// </summary>
    private void InitItemData()
    {
       for(int i = 0; i < Items.Count; i++)
        {
            ItemData.Add( Items[i].GetComponent<MinGameHAKAIItem>());
        }
    }
    /// <summary>
    /// 盤面の画像を差し替えて盤面を生成する関数
    /// </summary>
    private void generateWall()
    {
            int i = 0,j=0;
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            int RandomNum = Random.Range(0, RandomRageLengthWall);
                //Debug.Log(" "+RandomNum);
            for (int k = 0; k < tile.Count; k++)
            {
                //生成された乱数がタイルを生成する確率より小さければ。
                if (tile[k].P >= RandomNum)
                {
                    wall.GetComponent<SpriteRenderer>().sprite = tile[k].TileImage;
                    break;

                }
            }
            //壁の初期化
            if (j >= Rsize)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = wall;
            i++;
            if (i == Csize)
            {
                i = 0;
                j++;
            }
        }
    }
    private void generateItem()
    {
        //設置できるアイテムの個数分だけ考える
        for(int i = 0; i < itemcount; i++)
        {
            int RandomNum = Random.Range(0, RandomRageLengthItem);
            int RandomRaw = Random.Range(0, Rsize - 1);
            int RandomColumn = Random.Range(0, Csize - 1);
            //j：アイテムのindex、アイテムとアイテムデータのindexは同じ
            for(int j = 0; j < Items.Count; j++)
            {
                //生成確率より大きい場合スキップ
                if (ItemData[j].Prob < RandomNum) continue;
                //アイテムのインデックスが盤面のサイズを超える場合スキップ
                if (RandomRaw + ItemData[j].m_Ysize - 1 >= Rsize && RandomColumn + ItemData[j].m_Xsize - 1 >= Csize) continue;

                bool m_cansetItem = new bool();
                m_cansetItem = true;
                for(int k=0;k< ItemData[j].m_Ysize; k++)
                {
                    for(int l= 0;l< ItemData[j].m_Xsize; l++)
                    {
                        if (CanSetItem[k+RandomRaw, l+RandomColumn] == false)
                        {
                            m_cansetItem = false;
                            break;
                        }
                    }
                    if (m_cansetItem == false) break;
                }
                //既に枠にアイテムが入っていたらスキップ
                if (m_cansetItem == false) continue;

                Debug.Log("アイテムを盤面に配置:" + Items[j].name);
                //盤面にアイテムを格納する;
                for (int k = 0; k < ItemData[j].m_Ysize; k++)
                {
                    for (int l = 0; l < ItemData[j].m_Xsize; l++)
                    {
                        
                        CanSetItem[k+RandomRaw, l+RandomColumn] = false;
                      
                    }
                }
                GameObject CreatItem;
                MinGameHAKAIItem CreatItemData;
                //アイテムの生成
                CreatItem=Instantiate(Items[j],Wall[RandomRaw,RandomColumn].transform);
                CreatItemData = CreatItem.GetComponent<MinGameHAKAIItem>();
                CreatItemData.m_TopLeftColumn = RandomColumn;
                CreatItemData.m_TopLeftRaw = RandomRaw;
                CreatItemData.KeyWall = Wall[RandomRaw, RandomColumn];

            }

        }

    }


}
