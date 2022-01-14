using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using map;
[DefaultExecutionOrder(-1)]

public class setCrntMapNum : MonoBehaviour
{
    public MAP_NUM mpnum;
    public float x = 0;
    public float y = 0;

    void Start()
    {
        AllMapSet.update_initial_mapdata(mpnum, mpnum, x, y);
    }
}
