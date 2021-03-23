using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinGameHAKAIItem : MonoBehaviour
{
    GameObject KeyWall;
    public int m_Xsize;
    public int m_Ysize;
    [HideInInspector] public int m_TopLeftRaw;
    [HideInInspector] public int m_TopLeftColumn;
    public bool CanGetItem;
    // Start is called before the first frame update
    void Start()
    {
        CanGetItem = false;
    }
    void OnCollisionStay(Collision collision)
    {
        // もし接触している相手オブジェクトの名前が"Wall"でないならばエラー
        if (collision.gameObject.tag != "Wall") Debug.LogError("壁以外の当たり判定が取得されています。");
        KeyWall = collision.gameObject;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

}
