using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MingameHAKAIGetItemManager : MonoBehaviour
{
    [SerializeField, ReadOnly] List<GameObject> Item;
    [SerializeField, ReadOnly] GameObject[,] Wall;
    private int WallLength;
    MinGameHakaiManager2 GameManager;
    // Start is called before the first frame update
    void Start()
    {
        GameManager = this.GetComponent<MinGameHakaiManager2>();
        foreach (GameObject m_Item in GameObject.FindGameObjectsWithTag("Item"))
        {
            Item.Add(m_Item);
        }
        Wall = GameManager.Wall;
        WallLength = GameManager.Wall.Length;
        SetIndexItem();

    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    ///  Itemの左上が属する壁の行列番号をセットする。
    /// </summary>
    private void SetIndexItem()
    {
        foreach (GameObject v in Item)
        {

            (v.GetComponent<MinGameHAKAIItem>().m_TopLeftRaw,
             v.GetComponent<MinGameHAKAIItem>().m_TopLeftColumn) = GetIndex(v);
        }

    }

    /// <summary>
    /// Itemの左上に属する壁を探す
    /// </summary>
    /// <param name="Item"></param>
    /// <returns>行番号、列番号</returns>
    private (int, int) GetIndex(GameObject Item)
    {
        for (int i = 0; i < WallLength; i++)
        {
            for (int j = 0; j < WallLength; j++)
            {
                if (Item != Wall[i, j]) continue;
                return (i, j);
            }
        }
        return (0, 0);
    }
    /// <summary>
    /// アイテムが埋め込まれている壁を全て取り除いたかどうか。
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private bool CheckGetItem(GameObject obj){
        MinGameHAKAIItem Item;
        Item = obj.GetComponent<MinGameHAKAIItem>();
        for(int i = Item.m_TopLeftRaw; i < Item.m_TopLeftRaw + Item.m_Xsize; i++)
        {
            for(int j = Item.m_TopLeftColumn; j < Item.m_TopLeftColumn + Item.m_Ysize; j++)
            {
                if (Wall[i, j].GetComponent<SpriteRenderer>().sprite.name != GameManager.PolutedLevel2) return false;
            }
        }

        return true;

    }


}
