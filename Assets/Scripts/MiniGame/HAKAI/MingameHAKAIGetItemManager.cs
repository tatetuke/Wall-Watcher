using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class MingameHAKAIGetItemManager : MonoBehaviour
{
    [ ReadOnly] public List<GameObject> Item;
    [ ReadOnly] public GameObject[,] Wall;
    private int col_size=11;
    private int row_size=8;
    MinGameHakaiManager2 GameManager;
    private float LineThickness = 1;  // 光らせる際の線の太さ
    // Start is called before the first frame update
    void Start()
    {
        GameManager = this.GetComponent<MinGameHakaiManager2>();
        foreach (GameObject m_Item in GameObject.FindGameObjectsWithTag("Item"))
        {

            Item.Add(m_Item);
        }
        Wall = GameManager.Wall;

    }
    private void Update()
    {

        //SetIndexItem();
    }
    //初期化のための関数
    /// <summary>
    ///  Itemの左上が属する壁の行列番号をセットする。
    /// </summary>
    private void SetIndexItem()
    {
        foreach (GameObject m_Item in Item)
        {
            MinGameHAKAIItem ItemData = m_Item.GetComponent<MinGameHAKAIItem>();
            (ItemData.m_TopLeftRaw,
             ItemData.m_TopLeftColumn) = GetIndex(ItemData.KeyWall);
        }

    }

    /// <summary>
    /// Itemの左上に属する壁を探す
    /// </summary>
    /// <param name="Item"></param>
    /// <returns>行番号、列番号</returns>
    private (int, int) GetIndex(GameObject m_ItemWallData)
    {
        for (int i = 0; i < row_size; i++)
        {
            for (int j = 0; j < col_size; j++)
            {
                if (m_ItemWallData != Wall[i, j]) continue;
                return (i, j);
            }
        }
        Debug.LogError("アイテムに対応する添え字が見つかりませんでした。");
        return (0, 0);
    }
    //初期化のための関数

        /// <summary>
        /// アイテムの情報の更新
        /// </summary>
    public void UpdateItemData(){
        foreach(GameObject m_Item in Item)
        {
            CheckGetItem(m_Item);
            //UpdateGlowImage(m_Item);
        }
    }
    /// <summary>
    /// アイテムが埋め込まれている壁を全て取り除いたかどうか。
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    private void CheckGetItem(GameObject obj){
        MinGameHAKAIItem m_Item;
        m_Item = obj.GetComponent<MinGameHAKAIItem>();
        for(int i = m_Item.m_TopLeftRaw; i < m_Item.m_TopLeftRaw + m_Item.m_Xsize; i++)
        {
            for(int j = m_Item.m_TopLeftColumn; j < m_Item.m_TopLeftColumn + m_Item.m_Ysize; j++)
            {
                if (Wall[i, j].GetComponent<SpriteRenderer>().sprite.name != GameManager.PolutedLevel6)
                {
                    m_Item.CanGetItem = false;
                    return;
                }
            }
        }
        m_Item.CanGetItem = true;
        return;

    }

    /// <summary>
    /// アイテムを取得可能ならばスプライトの周りを光らせる
    /// </summary>
    /// <param name="m_Item"></param>
    private void UpdateGlowImage(GameObject m_Item)
    {
        MinGameHAKAIItem m_ItemData;
        m_ItemData = m_Item.GetComponent<MinGameHAKAIItem>();

        // 前回の対象を光らせなくする
        if (!m_ItemData.CanGetItem)
            SetGlowLine(m_Item.gameObject, 0);
        else
        {
            SetGlowLine(m_Item.gameObject, LineThickness);
       
        }


    }

    private void SetGlowLine(GameObject m_gameObject, float num)
    {
        if (m_gameObject == null) return;
        GameObject image = m_gameObject.transform.GetChild(0).gameObject;
        Material material = image.GetComponent<Renderer>().material;
        material.SetFloat("_Thick", num);
    }
}
