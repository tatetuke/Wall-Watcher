using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1)]

public class setCrntMapNum : MonoBehaviour
{
    public int setNum = 1;
    void Start()
    {
        if (AllMapSet.currentMap == 0 && AllMapSet.prevMap == 0)
        {
            AllMapSet.currentMap = setNum;
            AllMapSet.prevMap = setNum;
        }
    }
}
