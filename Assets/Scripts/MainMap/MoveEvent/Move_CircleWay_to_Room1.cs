using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_CircleWay_to_Room1 : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            bool value = Input.GetKey("up");
            if (value) FadeManager.Instance.LoadLevel("MainMap3_Room1", 1f);
        }
    }
}
