using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
                AllMapSet.currentMap = toNum;
                Debug.Log(AllMapSet.warpMap[fromNum, toNum].Item3);
                FadeManager.Instance.LoadLevel(AllMapSet.warpMap[fromNum, toNum].Item3, 1f);
            }

        }
    }
}
