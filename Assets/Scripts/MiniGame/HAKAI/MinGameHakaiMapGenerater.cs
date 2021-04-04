using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MinGameHakaiMapGenerater : MonoBehaviour
{
    public int RandomRageLength;
    /// <summary>
    /// 確率の低い順に並べる必要があります。
    /// </summary>
    public List<MinGameHakaiMapTileData>tile;
    private void Start()
    {
        //最後タイルを選ぶ確率が100%になるようにする。
        tile[tile.Count - 1].P = RandomRageLength;
        //壁の生成
        generateWall();
        
         
    }
    /// <summary>
    /// 盤面の画像を差し替えて盤面を生成する関数
    /// </summary>
    private void generateWall()
    {
        
        foreach (GameObject wall in GameObject.FindGameObjectsWithTag("Wall"))
        {
            int RandomNum = Random.Range(0, RandomRageLength);
                Debug.Log(" "+RandomNum);
            for (int k = 0; k < tile.Count; k++)
            {
                //生成された乱数がタイルを生成する確率より小さければ。
                if (tile[k].P >= RandomNum)
                {
                    wall.GetComponent<SpriteRenderer>().sprite = tile[k].TileImage;
                    break;

                }
            }

        }
    }


}
