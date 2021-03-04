using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPositionSet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int prev = AllMapSet.prevMap;
        int crnt = AllMapSet.currentMap;
        float newx = AllMapSet.warpMap[prev, crnt].Item1;
        float newy = AllMapSet.warpMap[prev, crnt].Item2;
        this.transform.position = new Vector3(newx, newy, 0);
    }
}
