using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warp : MonoBehaviour
{
    public int fromNum = 0;
    public int toNum = 0;
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool value = Input.GetKey("up");
            if (value)
            {
                AllMapSet.prevMap = fromNum;
                Debug.Log(fromNum);
                AllMapSet.currentMap = toNum;
                FadeManager.Instance.LoadLevel(AllMapSet.warpMap[AllMapSet.prevMap, AllMapSet.currentMap].Item3, 1f);
            }

        }
    }
}
