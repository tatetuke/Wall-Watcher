using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using map;

public class Warp : MonoBehaviour
{

    public MAP_NUM fromNum;
    public MAP_NUM toNum;
    public Direction2D autoDirection = Direction2D.Invalid;
    public float x = 0;   // 移動後 x 座標
    public float y = 0;   // 移動後 y 座標

    public enum MOVE_KEY
    {
        left,
        up,
        right,
        down,
        invaid
    }
    public MOVE_KEY move_key;   // 移動のトリガーとなるキー
    private bool move = true;   // マップ移動許可フラグ
    private bool move_same = true;  // 同じマップ内移動の許可フラグ
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            string key_string = move_key.ToString();
            bool value = Input.GetKey(key_string);
            if (value)
            {
                if (!FadeManager2.Instance.isFading) move_same = true;  // 遷移中でない場合は同じマップ内の移動は許可
                /*if (move)
                {
                    if (fromNum != toNum)   // 移動先が異なるマップの場合
                    {
                        move = false;
                        AllMapSet.update_initial_mapdata(fromNum, toNum, x, y);
                        AllMapSet.autoWalkingDirection = autoDirection;
                        FadeManager.Instance.LoadLevel(toNum.ToString(), 1f);
                    } else
                    {
                        GameObject player = GameObject.Find("Player");
                        AllMapSet.autoWalkingDirection = autoDirection;
                        FadeManager2.Instance.LoadLevel2(1.2f, player, x, y);
                    }
                }*/
                if (fromNum != toNum)   // 行先が違うマップへの移動
                {
                    if (move)
                    {
                        move = false;
                        AllMapSet.update_initial_mapdata(fromNum, toNum, x, y);
                        AllMapSet.autoWalkingDirection = autoDirection;
                        FadeManager.Instance.LoadLevel(toNum.ToString(), 1f);
                    }
                } else   // 同じマップ内での移動
                {
                    if (move_same)
                    {
                        move_same = false;
                        GameObject player = GameObject.Find("Player");
                        AllMapSet.autoWalkingDirection = autoDirection;
                        FadeManager2.Instance.LoadLevel2(1.2f, player, x, y);
                    }
                }
            }

        }
    }
}
