using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpSameMap : MonoBehaviour
{
    public int num = 0;
    public int index = 0;
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
    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            string key_string = move_key.ToString(); 
            bool value = Input.GetKey(key_string);
            if (value)
            {
                if (move)
                {
                    move = false;
                    GameObject player = GameObject.Find("Player");
                    float newx = AllMapSet.warpSameMapPosition[num, (index + 1) % 2].Item1;
                    float newy = AllMapSet.warpSameMapPosition[num, (index + 1) % 2].Item2;
                    FadeManager2.Instance.LoadLevel2(1.2f, player, newx, newy);
                }
            }
        }        
    }
}
