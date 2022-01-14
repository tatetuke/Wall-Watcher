using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]

public class setCrntMapNum : MonoBehaviour
{
    public Warp.MAP_NUM mapnum;
    public float x = 0;
    public float y = 0;

    void Start()
    {
        AllMapSet.update_initial_mapdata(mapnum, mapnum, x, y);
    }
}
