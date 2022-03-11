using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinGameHAKAIItem : MonoBehaviour
{
    public ItemSO itemSO;
    [ReadOnly]public GameObject KeyWall;
    public int m_Xsize;
    public int m_Ysize;
    public int m_TopLeftRaw;
    public int m_TopLeftColumn;
    public bool CanGetItem;
    public SpriteRenderer sprite;
    /// <summary>
    /// アイテムの出現確率
    /// </summary>
    public int Prob;
    // Start is called before the first frame update
    void Start()
    {
        CanGetItem = false;
        sprite.sprite = itemSO.icon;
 
    }
    //左上の座標を取得
    //void OnTriggerStay(Collision collision)
    //{
    //    Debug.Log("当たり判定の取得に成功しています");
    //    // もし接触している相手オブジェクトの名前が"Wall"でないならばエラー
    //    if (collision.gameObject.tag != "Wall") Debug.LogError("壁以外の当たり判定が取得されています。");
    //    KeyWall = collision.gameObject;
    //}
    // Update is called once per frame
}
