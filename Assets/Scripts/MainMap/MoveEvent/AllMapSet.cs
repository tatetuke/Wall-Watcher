using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllMapSet : MonoBehaviour
{
    public static int prevMap = 0;
    public static int currentMap = 0;
    public static (float, float, string)[,] warpMap = new (float, float, string)[,] { { (0, -3, "MainMap3_Room1"), (-6.5f, -2.8f, "MainMap3_Room1") }, { (-25, -3, "MainMap3_CircleWay"), (-1, -1, "None") } };
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
