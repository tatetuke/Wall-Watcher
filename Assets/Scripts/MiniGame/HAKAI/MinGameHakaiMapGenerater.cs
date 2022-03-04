using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinGameHakaiMapGenerater : MonoBehaviour
{
    public const int Rsize=8;
    public const int Csize=11;
    private bool[,] CanSetItem = new bool[Rsize,Csize ];
    public int RandomRageLengthWall=1000;
    public int RandomRageLengthItem=1000;
    public int itemcount=4;
    public GameObject[,] Wall = new GameObject[Rsize, Csize];
    public List<GameObject> Items;
    [HideInInspector]public List<MinGameHAKAIItem> ItemDatas;
    /// <summary>
    /// 確率の低い順に並べる必要があります。
    /// </summary>
    public List<MinGameHakaiMapTileData>tile;
    public MinGameHakaiManager2 gameManager;

    //各レアリティの確率(あとで累積和をとる)
    public int nprob = 100;
    public int rprob = 30;
    public int srprob = 15;
    public int ssrprob = 5;


    private List<MinGameHAKAIItem> nItems=new List<MinGameHAKAIItem>();
    private List<MinGameHAKAIItem> rItems=new List<MinGameHAKAIItem>();
    private List<MinGameHAKAIItem> srItems=new List<MinGameHAKAIItem>();
    private List<MinGameHAKAIItem> ssrItems=new List<MinGameHAKAIItem>();

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
        //レア度ごとのアイテムリストの初期化
        InitItemList();
        //各レア度の確率を初期化
        CalItemProbCumulativeSum();

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
            ItemDatas.Add( Items[i].GetComponent<MinGameHAKAIItem>());
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

            int RandomNum;
            if (i == 0 && j == 0)
            {
                RandomNum = UnityEngine.Random.Range(tile[tile.Count - 3].P, tile[tile.Count - 1].P);
            }
            else
            {

                int minIndex = 1000;
                int maxIndex = 0;
                if (j - 1 >= 0)
                {
                    int another = GetImageIndex(Wall[i, j - 1].GetComponent<SpriteRenderer>().sprite);
                    minIndex = Math.Min(another, minIndex);
                    maxIndex = Math.Max(another, maxIndex);
                }
                if (i - 1 >= 0)
                {
                    int another = GetImageIndex(Wall[i - 1, j].GetComponent<SpriteRenderer>().sprite);
                    minIndex = Math.Min(another, minIndex);
                    maxIndex = Math.Max(another, maxIndex);
                }
                if (maxIndex - minIndex >= 2)
                {
                    RandomNum = UnityEngine.Random.Range((tile[Math.Max(0, minIndex - 1)].P + tile[Math.Max(0, minIndex - 2)].P) / 2, tile[Math.Min(tile.Count - 1, maxIndex + 1)].P);

                }
                else
                {
                    RandomNum = UnityEngine.Random.Range((tile[Math.Max(0, minIndex - 1)].P + tile[Math.Max(0, minIndex - 2)].P) / 2, tile[Math.Min(tile.Count - 1, maxIndex + 1)].P);
                }
            }
            //RandomNum = UnityEngine.Random.Range(0,1000);
            for (int k = 0; k < tile.Count; k++)
            {

                //生成された乱数がタイルを生成する確率より小さければ。
                if (tile[k].P >= RandomNum)
                {
                    wall.GetComponent<SpriteRenderer>().sprite = tile[k].TileImage;
                    break;

                }
            }
            //Debug.Log(" "+RandomNum);

            //壁の初期化
            if (i >= Rsize)
            {
                Debug.LogError("壁の数が多すぎます");
                break;
            }
            Wall[i, j] = wall;
            j++;
            if (j == Csize)
            {
                j = 0;
                i++;
            }
        }
    }
    private void generateItem()
    {
        //設置できるアイテムの個数分だけ考える
        for(int i = 0; i < itemcount; i++)
        {
            //int RandomNum = UnityEngine.Random.Range(0, RandomRageLengthItem);
            int RandomRaw = UnityEngine.Random.Range(0, Rsize);
            int RandomColumn = UnityEngine.Random.Range(0, Csize);

            //レア度を決める
            List<MinGameHAKAIItem> ItemData = new List<MinGameHAKAIItem>();
            int randProb = UnityEngine.Random.Range(0, nprob+1);
            if (randProb <= ssrprob)
            {
                ItemData = ssrItems;
            }else if (randProb <= srprob)
            {
                ItemData = srItems;
            }
            else if (randProb <= rprob)
            {
                ItemData = rItems;
            }
            else if (randProb <= nprob)
            {
                ItemData = nItems;
            }


            if (ItemData.Count == 0) continue;
            //j：アイテムのindex、アイテムとアイテムデータのindexは同じ

            int j = UnityEngine.Random.Range(0, ItemData.Count);

            
            //生成確率より大きい場合スキップ
            //if (ItemData[j].Prob < RandomNum) continue;
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
                    }
                }
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

            CreatItem = Instantiate(Items[j]);
            //アイテムの生成
            Transform transform_ = CreatItem.transform;
            transform_.position= Wall[RandomRaw, RandomColumn].transform.position;
            transform_.position +=Wall[RandomRaw + ItemData[j].m_Ysize - 1, RandomColumn + ItemData[j].m_Xsize - 1].transform.position;
            transform_.position = transform_.position / 2;
            // transform_.position += new Vector3(0, -2.8f, 0);

            CreatItemData = CreatItem.GetComponent<MinGameHAKAIItem>();

            CreatItemData.transform.parent = Wall[RandomRaw, RandomColumn].transform;

            CreatItemData.m_TopLeftColumn = RandomColumn;
            CreatItemData.m_TopLeftRaw = RandomRaw;
            CreatItemData.KeyWall = Wall[RandomRaw, RandomColumn];


        }

    }

    private int GetImageIndex(Sprite N)
    {
        for(int i = 0; i < tile.Count; i++)
        {
            if (tile[i].TileImage == N)
            {
                return i;
            }
        }
        Debug.LogError("tileのIndexを取得できませんでした。");
        return 0;
    }


    public void InitItemList()
    {

        foreach (MinGameHAKAIItem obj in ItemDatas)
        {
            switch (obj.itemSO.type) { 
                case ItemSO.Rarelity.N:
                    nItems.Add(obj);
                    break;
                case ItemSO.Rarelity.R:
                    rItems.Add(obj);
                    break;
                case ItemSO.Rarelity.SR:
                    srItems.Add(obj);
                    break;
                case ItemSO.Rarelity.SSR:
                    ssrItems.Add(obj);
                    break;
            }



        }

    }

    private void CalItemProbCumulativeSum()
    {
        srprob += ssrprob;
        rprob += srprob;
        nprob += rprob;
    }


}
