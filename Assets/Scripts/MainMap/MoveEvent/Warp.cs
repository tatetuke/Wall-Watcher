using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Warp : MonoBehaviour
{
    public int fromNum = 0;
    public int toNum = 0;

    public enum MOVE_KEY
    {
        left,
        up,
        right,
        down,
        invaid
    }
    public MOVE_KEY move_key;
    private bool move = true;

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            string key_string = move_key.ToString();
            bool value = Input.GetKey(key_string);
            if (value)
            {
                if (move)
                {
                    move = false;
                    AllMapSet.prevMap = fromNum;
                    AllMapSet.currentMap = toNum;
                    //Debug.Log(AllMapSet.warpMap[fromNum, toNum].Item3);
                    FadeManager.Instance.LoadLevel(AllMapSet.warpMap[fromNum, toNum].Item3, 1f);
                }
            }

        }
    }
}
